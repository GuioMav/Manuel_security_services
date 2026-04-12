// ============================================================
//  CyberShield SA — Cybersecurity Awareness Chatbot
//  Core/ResponseLibrary.cs
//  Static library of educational response strings covering:
//    • Phishing awareness
//    • Password safety
//    • Suspicious links
//    • General cybersecurity tips
//    • Chatbot purpose / about
//    • Quiz introduction
//
//  All responses are personalised using the caller-supplied name.
//  Multi-line strings use '\n' as a paragraph separator; Program.cs
//  splits on '\n' and routes each line to the typed display engine.
// ============================================================

namespace SecurityAwarenessBot.Core;

/// <summary>
/// Provides static factory methods that return formatted, educational
/// response strings for each cybersecurity topic.
/// </summary>
public static class ResponseLibrary
{
    // ── 1. Phishing ───────────────────────────────────────────────────────────

    /// <summary>
    /// Returns a comprehensive phishing-awareness response.
    /// </summary>
    public static string GetPhishingResponse(string userName = "Citizen") =>
        $"Great question, {userName}! Phishing is one of South Africa's biggest cyber threats.\n" +
        "\n" +
        "  Phishing is when cybercriminals impersonate trusted organisations — your bank,\n" +
        "  SARS, Home Affairs, or even a retailer — via email, SMS, or WhatsApp, to trick\n" +
        "  you into revealing personal or financial information.\n" +
        "\n" +
        "  ⚠  Common phishing red flags to watch for:\n" +
        "  • Urgent language: 'Your account will be SUSPENDED in 24 hours!'\n" +
        "  • Requests for passwords, OTPs, or banking details via email or SMS\n" +
        "  • Links that look almost — but not exactly — like real sites\n" +
        "     (e.g. 'absa-secure-login.net' instead of 'absa.co.za')\n" +
        "  • Poor grammar, spelling mistakes, or unusual sender addresses\n" +
        "  • Attachments you did not expect (they may contain malware)\n" +
        "\n" +
        "  ✔  How to protect yourself:\n" +
        "  • Never click links in unsolicited emails — go directly to the official website\n" +
        "  • Hover over links to preview where they actually lead before clicking\n" +
        "  • Call your bank directly if you receive a suspicious message\n" +
        "  • Report phishing emails to your provider and the SA CERT: www.cert.org.za\n" +
        "  • Enable two-factor authentication (2FA) on all accounts\n" +
        "\n" +
        "  ⚠  South African context: The South African Banking Risk Information Centre\n" +
        "  (SABRIC) reports thousands of phishing incidents annually. Never share your\n" +
        "  OTP — not even with someone claiming to be from your bank.\n";

    // ── 2. Password Safety ────────────────────────────────────────────────────

    /// <summary>
    /// Returns a password-safety response with practical SA-relevant advice.
    /// </summary>
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

    // ── 3. Suspicious Links ───────────────────────────────────────────────────

    /// <summary>
    /// Returns guidance on identifying and handling suspicious URLs.
    /// </summary>
    public static string GetSuspiciousLinksResponse(string userName = "Citizen") =>
        $"Good thinking, {userName}! Malicious links are the primary delivery method\n" +
        "  for phishing attacks, ransomware, and identity theft.\n" +
        "\n" +
        "  ⚠  Signs a link or URL is suspicious:\n" +
        "  • The domain has extra words or hyphens: 'nedbank-secure.co.info'\n" +
        "  • It uses HTTP instead of HTTPS (no padlock icon in browser)\n" +
        "  • A shortened URL (bit.ly, tinyurl) hides the real destination\n" +
        "  • The URL contains random characters or numbers: 'xk29.ru/redirect'\n" +
        "  • It arrives unsolicited via WhatsApp, SMS, or email\n" +
        "\n" +
        "  ✔  How to verify a link safely:\n" +
        "  • Hover over the link to see its true destination (desktop browsers)\n" +
        "  • Use a URL checker before clicking: https://www.virustotal.com/gui/home/url\n" +
        "  • Manually type official addresses into your browser address bar\n" +
        "  • Never open .exe, .zip, or .dmg files downloaded via unexpected links\n" +
        "  • On mobile: long-press a link to preview the URL before tapping\n" +
        "\n" +
        "  ✔  After clicking a suspicious link — act fast:\n" +
        "  • Disconnect from the internet immediately\n" +
        "  • Change passwords for any accounts you may have accessed\n" +
        "  • Run a full antivirus scan on your device\n" +
        "  • Report the incident to the SA CERT: www.cert.org.za or 012 399-4862\n" +
        "\n" +
        "  ⚠  In South Africa, fraudulent links are often distributed posing as\n" +
        "  SASSA grant notifications, Capitec or FNB alerts, or DSTV prize winners.\n" +
        "  Always verify through official channels before clicking.\n";

    // ── 4. General Tips ───────────────────────────────────────────────────────

    /// <summary>
    /// Returns a collection of general cybersecurity tips relevant to South Africans.
    /// </summary>
    public static string GetGeneralTipsResponse(string userName = "Citizen") =>
        $"Here are some essential cybersecurity habits for you, {userName}:\n" +
        "\n" +
        "  ✔  Device & Software Security:\n" +
        "  • Always keep your operating system and apps up to date\n" +
        "  • Install a reputable antivirus (Avast, Malwarebytes, Windows Defender)\n" +
        "  • Encrypt your device storage — especially your phone\n" +
        "  • Lock your screen with a PIN, fingerprint, or face ID\n" +
        "\n" +
        "  ✔  Network Safety:\n" +
        "  • Avoid using public Wi-Fi for internet banking or sensitive tasks\n" +
        "  • Use a VPN when connecting via public or untrusted networks\n" +
        "  • Change your home Wi-Fi password from the factory default\n" +
        "  • Enable your router's built-in firewall\n" +
        "\n" +
        "  ✔  Account & Identity Protection:\n" +
        "  • Enable 2FA on email, banking, and social media accounts\n" +
        "  • Review your app permissions — revoke access you no longer need\n" +
        "  • Do not overshare personal information on social media\n" +
        "  • Shred physical documents containing personal information\n" +
        "\n" +
        "  ✔  South Africa-specific resources:\n" +
        "  • SA Cyber Incident Response: www.cert.org.za\n" +
        "  • SABRIC (banking fraud): www.sabric.co.za\n" +
        "  • Report cybercrime to the SAPS: 10111 or your nearest police station\n";

    // ── 5. Purpose / About ────────────────────────────────────────────────────

    /// <summary>
    /// Returns a description of the chatbot's purpose and origin.
    /// </summary>
    public static string GetPurposeResponse(string userName = "Citizen") =>
        $"Hello, {userName}! Here's a bit about me:\n" +
        "\n" +
        "  I am CyberShield SA — a cybersecurity awareness chatbot built to support\n" +
        "  the South African Department of Cybersecurity's public education campaign.\n" +
        "\n" +
        "  ✔  My purpose:\n" +
        "  • To educate South African citizens about common online threats\n" +
        "  • To provide practical, easy-to-follow advice in plain language\n" +
        "  • To translate complex cybersecurity concepts into everyday guidance\n" +
        "\n" +
        "  ✔  What I can help with:\n" +
        "  • Recognising and avoiding phishing scams\n" +
        "  • Creating and managing strong passwords\n" +
        "  • Identifying suspicious links and websites\n" +
        "  • General digital hygiene and online safety habits\n" +
        "  • A cybersecurity knowledge quiz to test what you've learnt\n" +
        "\n" +
        "  I do not collect, store, or transmit your personal information.\n" +
        "  Everything stays on your device — your privacy is respected.\n" +
        "\n" +
        "  Type 'help' to explore all available topics, or simply ask me anything!\n";

    // ── 6. Quiz Introduction ──────────────────────────────────────────────────

    /// <summary>
    /// Returns the quiz introduction and first question.
    /// The ChatEngine manages subsequent questions via state.
    /// </summary>
    public static string GetQuizIntroResponse(string userName = "Citizen") =>
        $"Excellent, {userName}! Let's test your cybersecurity knowledge.\n" +
        "\n" +
        "  ❓  QUIZ — Cybersecurity Awareness Challenge\n" +
        "\n" +
        "  Question 1 of 5:\n" +
        "  You receive an SMS from 'ABSA Bank' asking you to click a link to\n" +
        "  verify your account. The link reads: 'absa-secure-verify.net/login'.\n" +
        "\n" +
        "  What should you do?\n" +
        "  • A) Click the link and log in to verify your account\n" +
        "  • B) Delete the SMS and contact ABSA directly via their official app\n" +
        "  • C) Reply to the SMS with your account number to confirm your identity\n" +
        "  • D) Forward the SMS to your contacts to warn them\n" +
        "\n" +
        "  Type A, B, C, or D to answer and I will tell you if you are correct!\n";

    // ── 7. Quiz Answers ───────────────────────────────────────────────────────

    /// <summary>
    /// Returns feedback for a quiz answer. <paramref name="answer"/> is expected
    /// to be a single uppercase letter (A–D).
    /// </summary>
    public static string GetQuizAnswerResponse(string answer, string userName = "Citizen")
    {
        return answer.ToUpper().Trim() switch
        {
            "B" =>
                $"✔  Correct, {userName}! Well done!\n" +
                "\n" +
                "  'B) Delete the SMS and contact ABSA directly via their official app'\n" +
                "  is the right answer.\n" +
                "\n" +
                "  Real banks NEVER send links via SMS asking you to log in.\n" +
                "  Always contact your bank via their official app or website.\n" +
                "\n" +
                "  Type 'quiz' to continue with the next question!\n",

            "A" or "C" or "D" =>
                $"⚠  Not quite, {userName}. The correct answer is B.\n" +
                "\n" +
                "  You should DELETE the SMS and contact ABSA directly.\n" +
                "  The link 'absa-secure-verify.net' is NOT absa.co.za — it is a fake site\n" +
                "  designed to steal your credentials. This is a classic phishing attack.\n" +
                "\n" +
                "  Type 'quiz' to continue with the next question!\n",

            _ =>
                $"Please type A, B, C, or D to answer the quiz question, {userName}.\n"
        };
    }

    // ── 8. Quiz Question 2 ────────────────────────────────────────────────────

    public static string GetQuizQuestion2(string userName = "Citizen") =>
        $"  ❓  Question 2 of 5 — {userName}:\n" +
        "\n" +
        "  Which of the following is the STRONGEST password?\n" +
        "  • A) Password123\n" +
        "  • B) MyDog2015\n" +
        "  • C) Springbok!Mountain$2024\n" +
        "  • D) 123456789\n" +
        "\n" +
        "  Type A, B, C, or D:\n";

    public static string GetQuizQuestion2Answer(string answer, string userName = "Citizen") =>
        answer.ToUpper().Trim() switch
        {
            "C" =>
                $"✔  Correct, {userName}! 'Springbok!Mountain$2024' is strong because\n" +
                "  it is long (23 chars), uses uppercase, lowercase, symbols, and numbers,\n" +
                "  and is not a dictionary word.\n" +
                "\n" +
                "  Type 'quiz' for Question 3!\n",

            _ =>
                $"⚠  Not quite. The answer is C: 'Springbok!Mountain$2024'.\n" +
                "  Length + complexity + unpredictability = a strong password.\n" +
                "\n" +
                "  Type 'quiz' for Question 3!\n"
        };

    // ── 9. Quiz Question 3 ────────────────────────────────────────────────────

    public static string GetQuizQuestion3(string userName = "Citizen") =>
        $"  ❓  Question 3 of 5 — {userName}:\n" +
        "\n" +
        "  What does HTTPS in a website address mean?\n" +
        "  • A) The site is operated by the government\n" +
        "  • B) The connection between you and the site is encrypted\n" +
        "  • C) The website has been certified as safe by Google\n" +
        "  • D) No personal data is stored on the website\n" +
        "\n" +
        "  Type A, B, C, or D:\n";

    public static string GetQuizQuestion3Answer(string answer, string userName = "Citizen") =>
        answer.ToUpper().Trim() switch
        {
            "B" =>
                $"✔  Correct, {userName}! HTTPS means the data transmitted between\n" +
                "  your browser and the web server is encrypted via TLS/SSL.\n" +
                "  However, HTTPS alone does NOT guarantee a site is safe or legitimate —\n" +
                "  scammers also use HTTPS on fake sites.\n" +
                "\n" +
                "  Type 'quiz' for Question 4!\n",

            _ =>
                $"⚠  The correct answer is B. HTTPS encrypts your connection,\n" +
                "  but it does not mean the site is trustworthy. Always check the domain!\n" +
                "\n" +
                "  Type 'quiz' for Question 4!\n"
        };

    // ── 10. Quiz Question 4 ───────────────────────────────────────────────────

    public static string GetQuizQuestion4(string userName = "Citizen") =>
        $"  ❓  Question 4 of 5 — {userName}:\n" +
        "\n" +
        "  You receive a WhatsApp message: 'Congratulations! You have won a R5 000\n" +
        "  Shoprite voucher. Click here to claim: bit.ly/voucher-win-SA'\n" +
        "\n" +
        "  What is the most appropriate action?\n" +
        "  • A) Click the link to claim your prize\n" +
        "  • B) Ignore and delete — this is a scam\n" +
        "  • C) Share it with friends and family so they can also benefit\n" +
        "  • D) Reply to ask if it is legitimate\n" +
        "\n" +
        "  Type A, B, C, or D:\n";

    public static string GetQuizQuestion4Answer(string answer, string userName = "Citizen") =>
        answer.ToUpper().Trim() switch
        {
            "B" =>
                $"✔  Correct, {userName}! This is a classic 'prize scam' designed to\n" +
                "  harvest your personal data or install malware on your device.\n" +
                "  Shortened URLs (bit.ly) hide the true destination — never click them\n" +
                "  from unsolicited messages. Shoprite does not distribute prizes via WhatsApp.\n" +
                "\n" +
                "  Type 'quiz' for Question 5 — the final question!\n",

            _ =>
                $"⚠  The correct answer is B — ignore and delete.\n" +
                "  Prize scams are extremely common on WhatsApp in South Africa.\n" +
                "  The link may steal your information or infect your device.\n" +
                "\n" +
                "  Type 'quiz' for Question 5 — the final question!\n"
        };

    // ── 11. Quiz Question 5 (Final) ───────────────────────────────────────────

    public static string GetQuizQuestion5(string userName = "Citizen") =>
        $"  ❓  Final Question — 5 of 5 — {userName}:\n" +
        "\n" +
        "  Which TWO of the following are signs of a phishing email? (choose the letter\n" +
        "  that shows the best combination)\n" +
        "\n" +
        "  • A) The email is from ceo@company.com and has no grammar mistakes\n" +
        "  • B) The email urges you to 'ACT NOW' and asks for your banking login details\n" +
        "  • C) The email has a sender address like 'support@amaz0n-help.ru'\n" +
        "  • D) Both B and C\n" +
        "\n" +
        "  Type A, B, C, or D:\n";

    public static string GetQuizQuestion5Answer(string answer, string userName = "Citizen")
    {
        bool correct = answer.ToUpper().Trim() == "D";

        string feedback = correct
            ? $"✔  Brilliant, {userName}! D is correct.\n" +
              "  Both 'urgent action required' language AND a suspicious sender domain\n" +
              "  ('amaz0n-help.ru' with a zero instead of 'o', and the .ru TLD) are\n" +
              "  major phishing red flags.\n"
            : $"⚠  The correct answer is D — Both B and C.\n" +
              "  Urgency + requests for credentials + suspicious sender domains are\n" +
              "  the most reliable indicators of phishing emails.\n";

        return feedback +
               "\n" +
               $"  🎉  Quiz complete, {userName}! Well done for completing the challenge.\n" +
               "  Remember: knowledge is your best defence against cybercrime.\n" +
               "\n" +
               "  Type 'phishing', 'password', or 'links' to learn more,\n" +
               "  or type 'exit' when you are ready to end your session.\n";
    }
}
