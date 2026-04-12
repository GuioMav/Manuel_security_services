// ============================================================
//  CyberShield SA — Cybersecurity Awareness Chatbot
//  Program.cs — Application Entry Point
//  Department of Cybersecurity Public Awareness Campaign
// ============================================================

using SecurityAwarenessBot.Core;
using SecurityAwarenessBot.Models;
using SecurityAwarenessBot.UI;
using SecurityAwarenessBot.Utils;

// ── 1. Boot: Display ASCII logo & play welcome audio ────────────────────────
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.Title = "CyberShield SA — Cybersecurity Awareness Chatbot";

UserInterface.DisplayLogo();
await AudioPlayer.PlayWelcomeAsync();

await Task.Delay(500);

// ── 2. Session header ────────────────────────────────────────────────────────
UserInterface.DrawBox("🇿🇦  CyberShield SA — Cybersecurity Awareness Chatbot  🇿🇦");
Console.WriteLine();

await UserInterface.TypeWriteAsync(
    "  Initialising secure session...", ConsoleColor.DarkGray, 40);
await Task.Delay(700);

await UserInterface.TypeWriteAsync(
    "  Hello! I am CyberShield, your personal cybersecurity assistant.",
    ConsoleColor.Cyan, 28);
await UserInterface.TypeWriteAsync(
    "  My mission is to help South African citizens stay safe online.",
    ConsoleColor.Cyan, 28);
Console.WriteLine();

// ── 3. Onboarding — capture user name ────────────────────────────────────────
string userName;
while (true)
{
    UserInterface.WriteColoured("  👤  What is your name? › ", ConsoleColor.Green);
    Console.ForegroundColor = ConsoleColor.White;
    userName = Console.ReadLine()?.Trim() ?? string.Empty;
    Console.ResetColor();

    if (!string.IsNullOrWhiteSpace(userName)) break;

    UserInterface.PrintWarning("Please enter your name so I can personalise your session.");
    Console.WriteLine();
}

// ── 4. Create User and greet ────────────────────────────────────────────────
var user = new User { Name = userName };
Console.WriteLine();

await UserInterface.TypeWriteAsync(
    $"  Welcome, {user.Name}! 🛡️   Your session ID is: {user.SessionId}",
    ConsoleColor.Yellow, 25);
await Task.Delay(300);
await UserInterface.TypeWriteAsync(
    "  Type 'help' at any time to see what topics I can assist with.",
    ConsoleColor.DarkGray, 20);

UserInterface.PrintHelpMenu();

// ── 5. Main conversation loop ────────────────────────────────────────────────
var engine = new ChatEngine(user);
bool running = true;

while (running)
{
    string rawInput = UserInterface.ReadInput(user.Name);

    // Empty input guard
    if (InputValidator.IsEmpty(rawInput))
    {
        Console.WriteLine();
        UserInterface.PrintWarning(
            "It looks like you didn't type anything. " +
            "Try typing 'help' to see available topics.");
        continue;
    }

    string response = await engine.GetResponseAsync(rawInput);

    switch (response)
    {
        case "__EXIT__":
            Console.WriteLine();
            UserInterface.DrawBorder(72, '═', ConsoleColor.Cyan);
            await UserInterface.TypeWriteAsync(
                $"  Goodbye, {user.Name}! Stay vigilant and stay safe online. 🛡️",
                ConsoleColor.Yellow, 30);
            await UserInterface.TypeWriteAsync(
                $"  Session duration: {user.GetSessionDuration()}",
                ConsoleColor.DarkGray, 20);
            UserInterface.DrawBorder(72, '═', ConsoleColor.Cyan);
            Console.WriteLine();
            running = false;
            break;

        case "__HELP__":
            UserInterface.PrintHelpMenu();
            break;

        default:
            Console.WriteLine();
            UserInterface.DrawBorder(72, '─', ConsoleColor.DarkCyan);
            await UserInterface.TypeWriteAsync(
                "  🤖  CyberShield:", ConsoleColor.Cyan, 18);
            Console.WriteLine();

            // Render each line with colour-coded typing effect
            foreach (string line in response.Split('\n', StringSplitOptions.RemoveEmptyEntries))
            {
                if (line.TrimStart().StartsWith('•'))
                    await UserInterface.TypeWriteAsync(line, ConsoleColor.White, 14);
                else if (line.TrimStart().StartsWith('⚠'))
                    await UserInterface.TypeWriteAsync(line, ConsoleColor.Yellow, 18);
                else if (line.TrimStart().StartsWith('✔'))
                    await UserInterface.TypeWriteAsync(line, ConsoleColor.Green, 18);
                else if (line.TrimStart().StartsWith('❓'))
                    await UserInterface.TypeWriteAsync(line, ConsoleColor.Magenta, 18);
                else
                    await UserInterface.TypeWriteAsync(line, ConsoleColor.Gray, 20);
            }

            UserInterface.DrawBorder(72, '─', ConsoleColor.DarkCyan);
            break;
    }
}
