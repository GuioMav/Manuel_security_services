// ============================================================
//  Manuel security services MSS — Cybersecurity Awareness Chatbot
//  Core/ChatEngine.cs
//  The main conversational engine. Responsibilities:
//    • Classify sanitised user input into a Topic
//    • Manage quiz state (5 sequential questions with answer routing)
//    • Return appropriate responses from ResponseLibrary
//    • Personalise all output via the injected User object
//    • Signal control codes (__EXIT__, __HELP__) to Program.cs
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
    QuizAnswer,
    Help,
    Exit,
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
/// and manages multi-turn quiz state.
/// </summary>
public class ChatEngine
{
    // ── Dependencies ──────────────────────────────────────────────────────────

    private readonly User _user;

    // ── State ─────────────────────────────────────────────────────────────────

    /// <summary>Current position in the quiz sequence.</summary>
    private QuizState _quizState = QuizState.NotStarted;

    // ── Constructor ───────────────────────────────────────────────────────────

    /// <summary>
    /// Creates a new ChatEngine bound to the given <paramref name="user"/> session.
    /// </summary>
    public ChatEngine(User user)
    {
        _user = user;
    }

    // ── Public interface ──────────────────────────────────────────────────────

    /// <summary>
    /// Processes raw user input and returns the appropriate response string.
    /// Returns "__EXIT__" to signal a graceful shutdown, "__HELP__" to show the
    /// help menu. All other returns are displayable response text.
    /// </summary>
    /// <param name="rawInput">The unmodified string read from the console.</param>
    public async Task<string> GetResponseAsync(string rawInput)
    {
        // Empty input is handled by Program.cs before reaching here, but guard anyway
        if (InputValidator.IsEmpty(rawInput))
            return InputValidator.GetFallbackMessage(_user.Name);

        string clean = InputValidator.Sanitise(rawInput);

        // ── Check control commands first ─────────────────────────────────────
        if (InputValidator.IsExitCommand(clean)) return "__EXIT__";
        if (InputValidator.IsHelpCommand(clean)) return "__HELP__";

        // ── If a quiz is in progress, route to quiz answer handler ─────────
        if (_quizState != QuizState.NotStarted && _quizState != QuizState.Complete)
            return await HandleQuizAnswerAsync(clean);

        // ── Classify the input and respond ────────────────────────────────────
        Topic topic = ClassifyInput(clean);

        // Introduce a brief async pause to simulate "thinking"
        await Task.Delay(350);

        return topic switch
        {
            Topic.Phishing        => ResponseLibrary.GetPhishingResponse(_user.Name),
            Topic.Password        => ResponseLibrary.GetPasswordResponse(_user.Name),
            Topic.SuspiciousLinks => ResponseLibrary.GetSuspiciousLinksResponse(_user.Name),
            Topic.Purpose         => ResponseLibrary.GetPurposeResponse(_user.Name),
            Topic.Tips            => ResponseLibrary.GetGeneralTipsResponse(_user.Name),
            Topic.Quiz            => StartOrAdvanceQuiz(),
            _                     => InputValidator.GetFallbackMessage(_user.Name)
        };
    }

    // ── Input classification ──────────────────────────────────────────────────

    /// <summary>
    /// Maps a sanitised input string to the most appropriate <see cref="Topic"/>
    /// using keyword matching via the <see cref="StringExtensions.ContainsAny"/> helper.
    /// </summary>
    public Topic ClassifyInput(string sanitisedInput)
    {
        if (sanitisedInput.ContainsAny(
                "phishing", "phish", "scam", "fraud",
                "fake email", "spoofing", "smishing", "vishing"))
            return Topic.Phishing;

        if (sanitisedInput.ContainsAny(
                "password", "passcode", "pin", "passphrase",
                "credential", "login", "log in", "log-in"))
            return Topic.Password;

        if (sanitisedInput.ContainsAny(
                "link", "url", "click", "suspicious",
                "website", "site", "http", "https", "web address"))
            return Topic.SuspiciousLinks;

        if (sanitisedInput.ContainsAny(
                "purpose", "what are you", "who are you",
                "what do you do", "about", "introduce", "yourself"))
            return Topic.Purpose;

        if (sanitisedInput.ContainsAny(
                "tip", "advice", "protect", "safe",
                "security", "cyber", "secure", "best practice"))
            return Topic.Tips;

        if (sanitisedInput.ContainsAny(
                "quiz", "test", "question", "challenge", "trivia"))
            return Topic.Quiz;

        return Topic.Unknown;
    }

    // ── Quiz state machine ────────────────────────────────────────────────────

    /// <summary>
    /// Advances the quiz to the next question when the user types 'quiz',
    /// or returns the intro if the quiz has not started.
    /// </summary>
    private string StartOrAdvanceQuiz()
    {
        // If quiz is complete, restart from the beginning
        if (_quizState == QuizState.Complete)
            _quizState = QuizState.NotStarted;

        return _quizState switch
        {
            QuizState.NotStarted =>
                AdvanceTo(QuizState.Question1, ResponseLibrary.GetQuizIntroResponse(_user.Name)),

            QuizState.Question1 =>
                // User typed 'quiz' again — they missed the instruction; re-show Q1
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

    /// <summary>
    /// Routes the user's single-letter answer (A–D) to the correct question's
    /// answer handler and advances the quiz state on a valid answer.
    /// </summary>
    private async Task<string> HandleQuizAnswerAsync(string clean)
    {
        await Task.Delay(250);   // Simulate marking delay

        // Extract a single character answer — accept with or without surrounding text
        string answer = ExtractMultipleChoiceAnswer(clean);

        if (string.IsNullOrEmpty(answer))
        {
            // Non-answer input while quiz is active
            return $"  ❓  You're currently in the quiz, {_user.Name}.\n" +
                   "  Please type A, B, C, or D to answer the current question,\n" +
                   "  or type 'exit' to leave the quiz and return to the main menu.\n";
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
        string fb = ResponseLibrary.GetQuizAnswerResponse(answer, _user.Name);
        _quizState = QuizState.Question2;
        return fb + "\n" + ResponseLibrary.GetQuizQuestion2(_user.Name);
    }

    private string AnswerQ2(string answer)
    {
        string fb = ResponseLibrary.GetQuizQuestion2Answer(answer, _user.Name);
        _quizState = QuizState.Question3;
        return fb + "\n" + ResponseLibrary.GetQuizQuestion3(_user.Name);
    }

    private string AnswerQ3(string answer)
    {
        string fb = ResponseLibrary.GetQuizQuestion3Answer(answer, _user.Name);
        _quizState = QuizState.Question4;
        return fb + "\n" + ResponseLibrary.GetQuizQuestion4(_user.Name);
    }

    private string AnswerQ4(string answer)
    {
        string fb = ResponseLibrary.GetQuizQuestion4Answer(answer, _user.Name);
        _quizState = QuizState.Question5;
        return fb + "\n" + ResponseLibrary.GetQuizQuestion5(_user.Name);
    }

    private string AnswerQ5(string answer)
    {
        string fb = ResponseLibrary.GetQuizQuestion5Answer(answer, _user.Name);
        _quizState = QuizState.Complete;
        return fb;
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>
    /// Advances the quiz state and returns the provided response text.
    /// </summary>
    private string AdvanceTo(QuizState nextState, string response)
    {
        _quizState = nextState;
        return response;
    }

    /// <summary>
    /// Extracts a single multiple-choice letter (A, B, C, or D) from the input.
    /// Handles inputs like "b", "B", "option b", "I think c", etc.
    /// </summary>
    private static string ExtractMultipleChoiceAnswer(string clean)
    {
        // Single character input
        if (clean.Length == 1 && "abcd".Contains(clean))
            return clean.ToUpper();

        // Scan for isolated a/b/c/d word token
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
    public static bool ContainsAny(this string source, params string[] keywords) =>
        keywords.Any(k => source.Contains(k, StringComparison.OrdinalIgnoreCase));
}
