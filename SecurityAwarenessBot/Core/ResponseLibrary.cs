// ============================================================
//  Manuel security services MSS — Cybersecurity Awareness Chatbot
//  Core/ResponseLibrary.cs
//  Provides all educational response strings, fully integrated with
//  personalisation, sentiment detection, and memory features.
// ============================================================

namespace SecurityAwarenessBot.Core;

/// <summary>
/// Static library containing educational responses for various cybersecurity topics.
/// </summary>
public static class ResponseLibrary
{
    // ── 1. Randomized Phishing Tips ───────────────────────────────────────────

    private static readonly string[] PhishingTipsList =
    {
        "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations like banks, retail chains, or government agencies.",
        "Check the sender's email address closely. Scammers often use addresses that look similar but have minor misspellings (e.g., support@amaz0n-help.ru instead of support@amazon.com).",
        "Never click on links or download attachments in unsolicited emails. If in doubt, manually type the official website address directly into your browser.",
        "Watch out for urgent or threatening language like 'Your account will be suspended in 24 hours!' Scammers try to panic you into acting without thinking.",
        "Enable multi-factor authentication (MFA) on all your accounts. Even if a scammer gets your password, they won't be able to access your account without your physical mobile device."
    };

    private static readonly Random Rng = new();

    /// <summary>
    /// Returns a randomly selected phishing tip to keep interactions varied.
    /// </summary>
    public static string GetRandomPhishingTip(string userName = "Citizen")
    {
        int index = Rng.Next(PhishingTipsList.Length);
        return $"Here is a valuable phishing tip for you, {userName}:\n\n" +
               $"  💡 \"{PhishingTipsList[index]}\"";
    }

    // ── 2. Focused Keyword Responses ──────────────────────────────────────────

    public static string GetPasswordGuidance(string userName = "Citizen") =>
        $"Make sure to use strong, unique passwords for each account, {userName}.\n" +
        "  • Avoid using personal details (like your name, ID, or birthdate).\n" +
        "  • Use passphrases (e.g., 'Springbok!Mountain$2024') which are long and hard to crack.\n" +
        "  • Use a secure password manager like Bitwarden or 1Password to store credentials safely.";

    public static string GetPasswordResponse(string userName = "Citizen") =>
        $"Absolutely, {userName}! Weak passwords are the single biggest entry point\n" +
        "  for cybercriminals worldwide — including here in South Africa.\n" +
        "\n" +
        "  ⚠  Passwords you must NEVER use:\n" +
        "  • Your name, ID number, or date of birth\n" +
        "  • Simple sequences: '123456', 'abcdef', 'qwerty'\n" +
        "  • The word 'password' or 'password123'\n" +
        "  • Your phone number or the name of a family member\n" +
        "\n" +
        "  ✔  What makes a strong password:\n" +
        "  • At least 12 characters long (longer is always better)\n" +
        "  • A mix of UPPERCASE, lowercase, numbers (0–9), and symbols (!@#$)\n" +
        "  • A passphrase is even better: 'Springbok!Dance$2024'\n" +
        "  • Unique password for EVERY account — reusing passwords is dangerous\n" +
        "\n" +
        "  ✔  Tools and habits that help:\n" +
        "  • Use a reputable password manager (Bitwarden, 1Password, KeePass)\n" +
        "  • Enable two-factor authentication (2FA / MFA) wherever possible\n" +
        "  • Change passwords immediately if you suspect a breach\n" +
        "  • Check if your email has been compromised: https://haveibeenpwned.com\n" +
        "\n" +
        "  ⚠  Remember: No legitimate South African bank, government department,\n" +
        "  or reputable company will EVER ask for your password via phone, email,\n" +
        "  or SMS. If someone asks — it is a scam.\n";

    public static string GetScamGuidance(string userName = "Citizen") =>
        $"Scams in South Africa are extremely common, {userName}, posing as Capitec, Capitec Pay, Capitec App alerts, SASSA grants, or DHL/SAPS package issues.\n" +
        "  • Never share your banking OTP with anyone, not even someone claiming to be from your bank.\n" +
        "  • Verify any prize, job offer, or grant through official telephone numbers, never through WhatsApp links.";

    public static string GetPrivacyGuidance(string userName = "Citizen") =>
        $"Privacy is your first line of defense online, {userName}.\n" +
        "  • Go to your social media settings (Facebook, Instagram, LinkedIn) and make your profile private.\n" +
        "  • Shred all physical documents containing personal details before throwing them away.\n" +
        "  • Review the app permissions on your phone — turn off location and contacts access for apps that don't need them.";

    // ── 3. Comprehensive Classic Responses (from Task 1) ──────────────────────

    public static string GetPhishingResponse(string userName = "Citizen") =>
        $"Great question, {userName}! Phishing is one of South Africa's biggest cyber threats.\n\n" +
        "  Phishing is when cybercriminals impersonate trusted organisations — your bank, " +
        "SARS, Home Affairs, or even a retailer — via email, SMS, or WhatsApp, to trick " +
        "you into revealing personal or financial information.\n\n" +
        "  ⚠  Common phishing red flags to watch for:\n" +
        "  • Urgent language: 'Your account will be SUSPENDED in 24 hours!'\n" +
        "  • Requests for passwords, OTPs, or banking details via email or SMS\n" +
        "  • Links that look almost — but not exactly — like real sites (e.g. 'absa-secure-login.net' instead of 'absa.co.za')\n" +
        "  • Poor grammar, spelling mistakes, or unusual sender addresses\n\n" +
        "  ✔  How to protect yourself:\n" +
        "  • Never click links in unsolicited emails — go directly to the official website\n" +
        "  • Hover over links to preview where they actually lead before clicking\n" +
        "  • Report phishing emails to your provider and the SA CERT: www.cert.org.za\n\n" +
        "  ⚠  South African context: The South African Banking Risk Information Centre " +
        "(SABRIC) reports thousands of phishing incidents annually. Never share your " +
        "OTP — not even with someone claiming to be from your bank.";

    public static string GetSuspiciousLinksResponse(string userName = "Citizen") =>
        $"Good thinking, {userName}! Malicious links are the primary delivery method " +
        "for phishing attacks, ransomware, and identity theft.\n\n" +
        "  ⚠  Signs a link or URL is suspicious:\n" +
        "  • The domain has extra words or hyphens: 'nedbank-secure.co.info'\n" +
        "  • It uses HTTP instead of HTTPS (no padlock icon in browser)\n" +
        "  • A shortened URL (bit.ly, tinyurl) hides the real destination\n" +
        "  • It arrives unsolicited via WhatsApp, SMS, or email\n\n" +
        "  ✔  How to verify a link safely:\n" +
        "  • Hover over the link to see its true destination (desktop browsers)\n" +
        "  • Use a URL checker before clicking: https://www.virustotal.com\n" +
        "  • On mobile: long-press a link to preview the URL before tapping\n\n" +
        "  ⚠  In South Africa, fraudulent links are often distributed posing as " +
        "SASSA grant notifications, Capitec or FNB alerts, or DSTV prize winners. " +
        "Always verify through official channels before clicking.";

    public static string GetGeneralTipsResponse(string userName = "Citizen") =>
        $"Here are some essential cybersecurity habits for you, {userName}:\n\n" +
        "  ✔  Device & Software Security:\n" +
        "  • Always keep your operating system and apps up to date\n" +
        "  • Install a reputable antivirus and lock your screen with a PIN or biometrics\n\n" +
        "  ✔  Network Safety:\n" +
        "  • Avoid using public Wi-Fi for internet banking or sensitive tasks\n" +
        "  • Change your home Wi-Fi password from the factory default\n\n" +
        "  ✔  Account & Identity Protection:\n" +
        "  • Enable 2FA on email, banking, and social media accounts\n" +
        "  • Review your app permissions and shred physical documents\n\n" +
        "  ✔  South Africa-specific resources:\n" +
        "  • SA Cyber Incident Response: www.cert.org.za\n" +
        "  • SABRIC (banking fraud): www.sabric.co.za\n" +
        "  • Report cybercrime to the SAPS: 10111";

    public static string GetPurposeResponse(string userName = "Citizen") =>
        $"Hello, {userName}! Here's a bit about me:\n\n" +
        "  I am Manuel security services MSS — a cybersecurity awareness chatbot built to support " +
        "the South African Department of Cybersecurity's public education campaign.\n\n" +
        "  ✔  My purpose:\n" +
        "  • To educate South African citizens about common online threats\n" +
        "  • To provide practical, easy-to-follow advice in plain language\n" +
        "  • To translate complex cybersecurity concepts into everyday guidance\n\n" +
        "  I do not collect, store, or transmit your personal information. " +
        "Everything stays on your device — your privacy is respected.\n\n" +
        "  Type 'help' to explore all available topics, or simply ask me anything!";

    public static string GetSmallTalkResponse(string userName = "Citizen") =>
        $"I am doing very well, {userName}, thank you for asking!\n\n" +
        "  As an AI security assistant, I don't have feelings, but I am fully " +
        "operational and ready to help you strengthen your digital defences.\n\n" +
        "  Is there a specific cybersecurity topic you'd like to discuss? " +
        "(e.g., passwords, phishing, privacy, or suspicious links)";

    // ── 4. Sentiment Prepend Responses ────────────────────────────────────────

    public static string GetSentimentPrepend(string sentiment, string userName = "Citizen") =>
        sentiment.ToLower() switch
        {
            "worried" =>
                $"  It's completely understandable to feel worried, {userName}. Scammers can be very convincing, " +
                "and cyber threats can seem overwhelming. But you're doing the right thing by learning how to protect yourself! " +
                "Let me share this guidance to help you stay safe:\n\n",
            "frustrated" =>
                $"  I know it is incredibly frustrating to deal with these constant online risks and complex technical terms, {userName}. " +
                "Let's break this down into simple, actionable steps to put you back in control:\n\n",
            "curious" =>
                $"  That's a fantastic, curious question, {userName}! Inquiring minds build the strongest defenses. " +
                "Here is the detailed breakdown of how this works:\n\n",
            _ => string.Empty
        };

    // ── 5. Memory Recall Prepends ─────────────────────────────────────────────

    public static string GetMemoryRecallPrepend(string topic, string userName = "Citizen") =>
        $"  As someone interested in {topic}, {userName}, you'll find this especially useful:\n\n";

    // ── 6. Quiz Implementation (Cleaned up from contradictory prompts) ──────────

    public static string GetQuizIntroResponse(string userName = "Citizen") =>
        $"Excellent, {userName}! Let's test your cybersecurity knowledge.\n\n" +
        "  ❓  QUIZ — Cybersecurity Awareness Challenge\n\n" +
        "  Question 1 of 5:\n" +
        "  You receive an SMS from 'ABSA Bank' asking you to click a link to " +
        "verify your account. The link reads: 'absa-secure-verify.net/login'.\n\n" +
        "  What should you do?\n" +
        "  • A) Click the link and log in to verify your account\n" +
        "  • B) Delete the SMS and contact ABSA directly via their official app\n" +
        "  • C) Reply to the SMS with your account number to confirm your identity\n" +
        "  • D) Forward the SMS to your contacts to warn them\n\n" +
        "  Type A, B, C, or D to answer!";

    public static string GetQuizAnswerResponse(string answer, string userName = "Citizen")
    {
        return answer.ToUpper().Trim() switch
        {
            "B" =>
                $"✔  Correct, {userName}! Well done!\n\n" +
                "  'B) Delete the SMS and contact ABSA directly via their official app' " +
                "is the right answer. Real banks NEVER send links via SMS asking you to log in. " +
                "Always contact your bank via their official app or website.",

            "A" or "C" or "D" =>
                $"⚠  Not quite, {userName}. The correct answer is B.\n\n" +
                "  You should DELETE the SMS and contact ABSA directly. " +
                "The link 'absa-secure-verify.net' is NOT absa.co.za — it is a fake site " +
                "designed to steal your credentials. This is a classic phishing attack.",

            _ => $"Please enter A, B, C, or D to answer the quiz question, {userName}."
        };
    }

    public static string GetQuizQuestion2(string userName = "Citizen") =>
        $"  ❓  Question 2 of 5 — {userName}:\n\n" +
        "  Which of the following is the STRONGEST password?\n" +
        "  • A) Password123\n" +
        "  • B) MyDog2015\n" +
        "  • C) Springbok!Mountain$2024\n" +
        "  • D) 123456789\n\n" +
        "  Type A, B, C, or D:";

    public static string GetQuizQuestion2Answer(string answer, string userName = "Citizen") =>
        answer.ToUpper().Trim() switch
        {
            "C" =>
                $"✔  Correct, {userName}! 'Springbok!Mountain$2024' is strong because " +
                "it is long (23 chars), uses uppercase, lowercase, symbols, and numbers, " +
                "and is not a standard dictionary word.",

            _ =>
                $"⚠  Not quite, {userName}. The answer is C: 'Springbok!Mountain$2024'. " +
                "Length + complexity + unpredictability = a strong password."
        };

    public static string GetQuizQuestion3(string userName = "Citizen") =>
        $"  ❓  Question 3 of 5 — {userName}:\n\n" +
        "  What does HTTPS in a website address mean?\n" +
        "  • A) The site is operated by the government\n" +
        "  • B) The connection between you and the site is encrypted\n" +
        "  • C) The website has been certified as safe by Google\n" +
        "  • D) No personal data is stored on the website\n\n" +
        "  Type A, B, C, or D:";

    public static string GetQuizQuestion3Answer(string answer, string userName = "Citizen") =>
        answer.ToUpper().Trim() switch
        {
            "B" =>
                $"✔  Correct, {userName}! HTTPS means the data transmitted between " +
                "your browser and the web server is encrypted. " +
                "However, HTTPS alone does NOT guarantee a site is safe — " +
                "scammers also use HTTPS on fake sites.",

            _ =>
                $"⚠  Not quite, {userName}. The correct answer is B. HTTPS encrypts your connection, " +
                "but it does not mean the site is automatically safe. Always verify the domain!"
        };

    public static string GetQuizQuestion4(string userName = "Citizen") =>
        $"  ❓  Question 4 of 5 — {userName}:\n\n" +
        "  You receive a WhatsApp message: 'Congratulations! You have won a R5 000 " +
        "Shoprite voucher. Click here to claim: bit.ly/voucher-win-SA'\n\n" +
        "  What is the most appropriate action?\n" +
        "  • A) Click the link to claim your prize\n" +
        "  • B) Ignore and delete — this is a scam\n" +
        "  • C) Share it with friends and family so they can also benefit\n" +
        "  • D) Reply to ask if it is legitimate\n\n" +
        "  Type A, B, C, or D:";

    public static string GetQuizQuestion4Answer(string answer, string userName = "Citizen") =>
        answer.ToUpper().Trim() switch
        {
            "B" =>
                $"✔  Correct, {userName}! This is a classic 'prize scam' designed to " +
                "harvest your personal data or install malware on your device. " +
                "Shortened URLs (bit.ly) hide the true destination. Shoprite does not distribute prizes via WhatsApp.",

            _ =>
                $"⚠  Not quite, {userName}. The correct answer is B — ignore and delete. " +
                "Prize scams are extremely common on WhatsApp in South Africa."
        };

    public static string GetQuizQuestion5(string userName = "Citizen") =>
        $"  ❓  Final Question — 5 of 5 — {userName}:\n\n" +
        "  Which TWO of the following are signs of a phishing email?\n" +
        "  • A) The email is from ceo@company.com and has no grammar mistakes\n" +
        "  • B) The email urges you to 'ACT NOW' and asks for your banking login details\n" +
        "  • C) The email has a sender address like 'support@amaz0n-help.ru'\n" +
        "  • D) Both B and C\n\n" +
        "  Type A, B, C, or D:";

    public static string GetQuizQuestion5Answer(string answer, string userName = "Citizen")
    {
        bool correct = answer.ToUpper().Trim() == "D";

        string feedback = correct
            ? $"✔  Brilliant, {userName}! D is correct.\n" +
              "  Both 'urgent action required' language AND a suspicious sender domain " +
              "('amaz0n-help.ru' with a zero instead of 'o', and the .ru TLD) are major phishing red flags."
            : $"⚠  Not quite, {userName}. The correct answer is D — Both B and C.\n" +
              "  Urgency + requests for credentials + suspicious sender domains are the most reliable indicators of phishing.";

        return feedback;
    }
}
