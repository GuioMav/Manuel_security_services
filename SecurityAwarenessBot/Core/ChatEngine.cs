// ============================================================
//  Manuel security services MSS — Cybersecurity Awareness Chatbot
//  Core/ChatEngine.cs
//  The logical core. Responsible for classifying inputs, tracking
//  multiturn quiz state (with scoring), maintaining conversational memory
//  (topics and sentiments), and constructing personalized, dynamic replies.
// ============================================================

using SecurityAwarenessBot.Models;

namespace SecurityAwarenessBot.Core;

// ── Topic enum ────────────────────────────────────────────────────────────────

/// <summary>
/// Represents all conversational topics the chatbot can handle.
/// </summary>
public enum Topic
{
    Phishing,
    Password,
    SuspiciousLinks,
    Purpose,
    Tips,
    Quiz,
    Help,
    Exit,
    SmallTalk,
    Scam,
    Privacy,
    Unknown
}

// ── Quiz state enum ───────────────────────────────────────────────────────────

/// <summary>
/// Tracks which stage of the 5-question quiz the user is currently at.
/// </summary>
public enum QuizState
{
    NotStarted,
    Question1,
    Question2,
    Question3,
    Question4,
    Question5,
    Complete
}

// ── ChatEngine ────────────────────────────────────────────────────────────────

/// <summary>
/// Core message-processing engine. Maps user input to educational responses
/// and manages multi-turn quiz state, user preferences, and emotional status.
/// </summary>
public class ChatEngine
{
    // ── Dependencies ──────────────────────────────────────────────────────────

    private readonly User _user;

    // ── State ─────────────────────────────────────────────────────────────────

    private QuizState _quizState = QuizState.NotStarted;
    private Topic _lastTopic = Topic.Unknown;

    // ── Constructor ───────────────────────────────────────────────────────────

    public ChatEngine(User user)
    {
        _user = user;
    }

    // ── Public interface ──────────────────────────────────────────────────────

    /// <summary>
    /// Processes raw user input and returns the appropriate response string.
    /// Supports keyword matches, sentiment pre-processing, memory store/recall,
    /// and topic-continuation flows.
    /// </summary>
    public async Task<string> GetResponseAsync(string rawInput)
    {
        if (InputValidator.IsEmpty(rawInput))
            return InputValidator.GetFallbackMessage(_user.Name);

        string clean = InputValidator.Sanitise(rawInput);

        // ── 1. Check control commands first ───────────────────────────────────
        if (InputValidator.IsExitCommand(clean)) return "__EXIT__";
        if (InputValidator.IsHelpCommand(clean)) return "__HELP__";

        // ── 2. Handle Quiz cancel/exit command ───────────────────────────────
        if (_quizState != QuizState.NotStarted && _quizState != QuizState.Complete)
        {
            if (clean.ContainsAnyKeyword("cancel", "leave", "quit quiz", "stop", "exit quiz"))
            {
                _quizState = QuizState.NotStarted;
                await Task.Delay(250);
                return "  ✔  You have successfully left the quiz and returned to the main menu. " +
                       "What cybersecurity topic would you like to explore next?";
            }
            return await HandleQuizAnswerAsync(clean);
        }

        // ── 3. Memory Capture: Detect user interests/favorites ────────────────
        if (clean.ContainsAnyKeyword("interested in", "my favorite", "i like", "i care about", "i love"))
        {
            string? detectedFav = null;
            if (clean.Contains("privacy")) detectedFav = "privacy";
            else if (clean.Contains("password")) detectedFav = "passwords";
            else if (clean.Contains("scam") || clean.Contains("phishing")) detectedFav = "phishing scams";
            else if (clean.Contains("link")) detectedFav = "suspicious links";

            if (detectedFav != null)
            {
                _user.FavoriteTopic = detectedFav;
                _user.HasRecalledFavorite = false; // Reset to allow fresh recall trigger
                await Task.Delay(300);
                return $"Great! I'll remember that you're interested in {detectedFav}. " +
                       "It's a crucial part of staying safe online.";
            }
        }

        // ── 4. Sentiment Detection ───────────────────────────────────────────
        string detectedSentiment = string.Empty;
        if (clean.ContainsAnyKeyword("worried", "scared", "fear", "afraid", "paranoid", "nervous", "worry"))
        {
            detectedSentiment = "worried";
        }
        else if (clean.ContainsAnyKeyword("frustrated", "angry", "annoyed", "irritated", "sick of", "tired of", "hate"))
        {
            detectedSentiment = "frustrated";
        }
        else if (clean.ContainsAnyKeyword("curious", "wonder", "want to know", "explain", "how do", "why does"))
        {
            detectedSentiment = "curious";
        }

        // ── 5. Conversation Flow: Context-based Continuation Routing ────────
        bool isContinuation = clean.ContainsAnyKeyword("another", "more", "explain", "continue", "detail", "next");
        Topic currentTopic;

        if (isContinuation && _lastTopic != Topic.Unknown && _lastTopic != Topic.Quiz)
        {
            currentTopic = _lastTopic;
        }
        else
        {
            currentTopic = ClassifyInput(clean);
        }

        // Introduce a brief async pause to simulate artificial "thinking"
        await Task.Delay(350);

        // ── 6. Select base educational response ──────────────────────────────
        string baseResponse = string.Empty;

        switch (currentTopic)
        {
            case Topic.Phishing:
                _lastTopic = Topic.Phishing;
                // If they ask for "tips" or "another tip", give randomized single tips
                if (clean.ContainsAnyKeyword("tip", "another", "more"))
                    baseResponse = ResponseLibrary.GetRandomPhishingTip(_user.Name);
                else
                    baseResponse = ResponseLibrary.GetPhishingResponse(_user.Name);
                break;

            case Topic.Password:
                _lastTopic = Topic.Password;
                if (clean.ContainsAnyKeyword("strong", "make", "create") || isContinuation)
                    baseResponse = ResponseLibrary.GetPasswordGuidance(_user.Name);
                else
                    baseResponse = ResponseLibrary.GetPasswordResponse(_user.Name);
                break;

            case Topic.SuspiciousLinks:
                _lastTopic = Topic.SuspiciousLinks;
                baseResponse = ResponseLibrary.GetSuspiciousLinksResponse(_user.Name);
                break;

            case Topic.Scam:
                _lastTopic = Topic.Scam;
                baseResponse = ResponseLibrary.GetScamGuidance(_user.Name);
                break;

            case Topic.Privacy:
                _lastTopic = Topic.Privacy;
                baseResponse = ResponseLibrary.GetPrivacyGuidance(_user.Name);
                break;

            case Topic.Purpose:
                _lastTopic = Topic.Purpose;
                baseResponse = ResponseLibrary.GetPurposeResponse(_user.Name);
                break;

            case Topic.Tips:
                _lastTopic = Topic.Tips;
                baseResponse = ResponseLibrary.GetGeneralTipsResponse(_user.Name);
                break;

            case Topic.Quiz:
                _lastTopic = Topic.Quiz;
                baseResponse = StartOrAdvanceQuiz();
                break;

            case Topic.SmallTalk:
                baseResponse = ResponseLibrary.GetSmallTalkResponse(_user.Name);
                break;

            case Topic.Unknown:
            default:
                // If sentiment was detected on an unknown topic, show a dedicated empathetic option menu!
                if (!string.IsNullOrEmpty(detectedSentiment))
                {
                    baseResponse = ResponseLibrary.GetSentimentSupportiveMessage(detectedSentiment, _user.Name);
                }
                else
                {
                    baseResponse = InputValidator.GetFallbackMessage(_user.Name);
                }
                break;
        }

        // ── 7. Build prepends (Sentiment + Memory recall) ────────────────────
        string finalPrepend = string.Empty;

        // Apply sentiment prepends for emotional empathy ONLY if we matched a known topic!
        if (!string.IsNullOrEmpty(detectedSentiment) && currentTopic != Topic.Unknown)
        {
            finalPrepend += ResponseLibrary.GetSentimentPrepend(detectedSentiment, _user.Name);
        }

        // Apply memory recall if discussing their favorited category
        if (_user.FavoriteTopic != null && !_user.HasRecalledFavorite && currentTopic != Topic.Unknown && currentTopic != Topic.Quiz)
        {
            bool matchesFavorite = false;
            if (_user.FavoriteTopic == "privacy" && currentTopic == Topic.Privacy) matchesFavorite = true;
            else if (_user.FavoriteTopic == "passwords" && currentTopic == Topic.Password) matchesFavorite = true;
            else if (_user.FavoriteTopic == "phishing scams" && (currentTopic == Topic.Phishing || currentTopic == Topic.Scam)) matchesFavorite = true;
            else if (_user.FavoriteTopic == "suspicious links" && currentTopic == Topic.SuspiciousLinks) matchesFavorite = true;

            if (matchesFavorite)
            {
                finalPrepend += ResponseLibrary.GetMemoryRecallPrepend(_user.FavoriteTopic, _user.Name);
                _user.HasRecalledFavorite = true; // Prevent spamming recall
            }
        }

        return finalPrepend + baseResponse;
    }

    // ── Input classification ──────────────────────────────────────────────────

    /// <summary>
    /// Maps a sanitised input string to the most appropriate <see cref="Topic"/>
    /// using robust keyword checking.
    /// </summary>
    public Topic ClassifyInput(string sanitisedInput)
    {
        if (sanitisedInput.ContainsAnyKeyword("phishing", "phish", "fake email", "spoofing", "smishing", "vishing"))
            return Topic.Phishing;

        if (sanitisedInput.ContainsAnyKeyword("password", "passcode", "pin", "passphrase", "credential", "login"))
            return Topic.Password;

        if (sanitisedInput.ContainsAnyKeyword("link", "url", "click", "suspicious", "website", "site", "http", "https"))
            return Topic.SuspiciousLinks;

        if (sanitisedInput.ContainsAnyKeyword("scam", "fraud", "sassa", "prize", "won", "voucher"))
            return Topic.Scam;

        if (sanitisedInput.ContainsAnyKeyword("privacy", "private", "personal data", "shred", "app permissions"))
            return Topic.Privacy;

        if (sanitisedInput.ContainsAnyKeyword("purpose", "what are you", "who are you", "what do you do", "introduce", "about"))
            return Topic.Purpose;

        // Expanded tips mapping to include general security threats like 'hack', 'breach', 'compromised', etc.
        if (sanitisedInput.ContainsAnyKeyword("tip", "advice", "protect", "safe", "security", "cyber", "secure", "hack", "hacked", "hacking", "breach", "compromise", "compromised", "attack", "attacked"))
            return Topic.Tips;

        if (sanitisedInput.ContainsAnyKeyword("quiz", "test", "challenge", "trivia"))
            return Topic.Quiz;

        if (sanitisedInput.ContainsAnyKeyword("how are you", "how's it going", "hello", "hi", "hey", "greetings"))
            return Topic.SmallTalk;

        return Topic.Unknown;
    }

    // ── Quiz state machine ────────────────────────────────────────────────────

    private string StartOrAdvanceQuiz()
    {
        if (_quizState == QuizState.Complete)
        {
            _quizState = QuizState.NotStarted;
            _user.QuizScore = 0;
        }

        return _quizState switch
        {
            QuizState.NotStarted =>
                AdvanceTo(QuizState.Question1, ResponseLibrary.GetQuizIntroResponse(_user.Name)),

            QuizState.Question1 =>
                ResponseLibrary.GetQuizIntroResponse(_user.Name),

            QuizState.Question2 =>
                ResponseLibrary.GetQuizQuestion2(_user.Name),

            QuizState.Question3 =>
                ResponseLibrary.GetQuizQuestion3(_user.Name),

            QuizState.Question4 =>
                ResponseLibrary.GetQuizQuestion4(_user.Name),

            QuizState.Question5 =>
                ResponseLibrary.GetQuizQuestion5(_user.Name),

            _ => ResponseLibrary.GetQuizIntroResponse(_user.Name)
        };
    }

    private async Task<string> HandleQuizAnswerAsync(string clean)
    {
        await Task.Delay(250);

        string answer = ExtractMultipleChoiceAnswer(clean);

        if (string.IsNullOrEmpty(answer))
        {
            return $"  ❓  You're currently in the quiz, {_user.Name}.\n" +
                   "  Please type A, B, C, or D to answer the current question,\n" +
                   "  or type 'cancel' to leave the quiz and return to the main menu.\n";
        }

        return _quizState switch
        {
            QuizState.Question1 => AnswerQ1(answer),
            QuizState.Question2 => AnswerQ2(answer),
            QuizState.Question3 => AnswerQ3(answer),
            QuizState.Question4 => AnswerQ4(answer),
            QuizState.Question5 => AnswerQ5(answer),
            _ => InputValidator.GetFallbackMessage(_user.Name)
        };
    }

    private string AnswerQ1(string answer)
    {
        if (answer.ToUpper() == "B") _user.QuizScore++;
        string fb = ResponseLibrary.GetQuizAnswerResponse(answer, _user.Name);
        _quizState = QuizState.Question2;
        return fb + "\n\n" + ResponseLibrary.GetQuizQuestion2(_user.Name);
    }

    private string AnswerQ2(string answer)
    {
        if (answer.ToUpper() == "C") _user.QuizScore++;
        string fb = ResponseLibrary.GetQuizQuestion2Answer(answer, _user.Name);
        _quizState = QuizState.Question3;
        return fb + "\n\n" + ResponseLibrary.GetQuizQuestion3(_user.Name);
    }

    private string AnswerQ3(string answer)
    {
        if (answer.ToUpper() == "B") _user.QuizScore++;
        string fb = ResponseLibrary.GetQuizQuestion3Answer(answer, _user.Name);
        _quizState = QuizState.Question4;
        return fb + "\n\n" + ResponseLibrary.GetQuizQuestion4(_user.Name);
    }

    private string AnswerQ4(string answer)
    {
        if (answer.ToUpper() == "B") _user.QuizScore++;
        string fb = ResponseLibrary.GetQuizQuestion4Answer(answer, _user.Name);
        _quizState = QuizState.Question5;
        return fb + "\n\n" + ResponseLibrary.GetQuizQuestion5(_user.Name);
    }

    private string AnswerQ5(string answer)
    {
        if (answer.ToUpper() == "D") _user.QuizScore++;
        string fb = ResponseLibrary.GetQuizQuestion5Answer(answer, _user.Name);
        _quizState = QuizState.Complete;
        
        string completionSummary = 
            $"\n\n  🎉  Quiz complete, {_user.Name}! Well done for completing the challenge.\n" +
            $"      Your final score is: {_user.QuizScore} / 5\n\n" +
            "  Remember: knowledge is your best defence against cybercrime.\n" +
            "  Type 'tips', 'password', or 'links' to learn more, or 'exit' to end your session.";

        return fb + completionSummary;
    }

    private string AdvanceTo(QuizState nextState, string response)
    {
        _quizState = nextState;
        return response;
    }

    private static string ExtractMultipleChoiceAnswer(string clean)
    {
        if (clean.Length == 1 && "abcd".Contains(clean))
            return clean.ToUpper();

        foreach (string word in clean.Split(' '))
        {
            string w = word.Trim().ToUpper();
            if (w is "A" or "B" or "C" or "D")
                return w;
        }

        return string.Empty;
    }
}

// ── String extension ──────────────────────────────────────────────────────────

/// <summary>
/// Extension methods for <see cref="string"/> to support cleaner multi-keyword
/// matching in the ChatEngine classification logic.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns <see langword="true"/> if <paramref name="source"/> contains any of the
    /// specified <paramref name="keywords"/> (case-insensitive, ordinal comparison).
    /// </summary>
    public static bool ContainsAnyKeyword(this string source, params string[] keywords) =>
        keywords.Any(k => source.Contains(k, StringComparison.OrdinalIgnoreCase));
}
