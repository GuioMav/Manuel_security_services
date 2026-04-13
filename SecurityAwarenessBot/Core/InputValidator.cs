// ============================================================
//  Monkey Bot — Cybersecurity Awareness Chatbot
//  Core/InputValidator.cs
//  Validates and sanitises raw user input before it reaches the
//  ChatEngine, ensuring graceful handling of edge cases:
//    • Empty / whitespace-only strings
//    • Exit commands
//    • Help commands
//    • Unsupported / unrecognised input (fallback message)
// ============================================================

namespace SecurityAwarenessBot.Core;

/// <summary>
/// Provides static validation and sanitisation helpers for user input.
/// </summary>
public static class InputValidator
{
    // ── Known command sets ────────────────────────────────────────────────────

    private static readonly string[] ExitCommands =
        { "exit", "quit", "bye", "goodbye", "q", "close", "end" };

    private static readonly string[] HelpCommands =
        { "help", "topics", "menu", "?", "commands", "options", "what can you do" };

    // ── Guards ────────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns <see langword="true"/> when the input is null, empty, or consists
    /// entirely of whitespace characters.
    /// </summary>
    public static bool IsEmpty(string input) =>
        string.IsNullOrWhiteSpace(input);

    /// <summary>
    /// Returns <see langword="true"/> when the trimmed, lowercase input matches
    /// any known exit command.
    /// </summary>
    public static bool IsExitCommand(string input) =>
        ExitCommands.Contains(input.Trim().ToLower());

    /// <summary>
    /// Returns <see langword="true"/> when the input contains a help keyword.
    /// </summary>
    public static bool IsHelpCommand(string input) =>
        HelpCommands.Any(cmd =>
            input.Trim().ToLower().Contains(cmd, StringComparison.OrdinalIgnoreCase));

    // ── Sanitisation ──────────────────────────────────────────────────────────

    /// <summary>
    /// Normalises the input: trims surrounding whitespace, converts to lowercase,
    /// and collapses any internal runs of whitespace to single spaces.
    /// </summary>
    /// <example>"  Hello   World " → "hello world"</example>
    public static string Sanitise(string input) =>
        string.Join(
            ' ',
            input.Trim()
                 .ToLower()
                 .Split(' ', StringSplitOptions.RemoveEmptyEntries));

    // ── Fallback response ─────────────────────────────────────────────────────

    /// <summary>
    /// Returns a friendly fallback message when the input cannot be matched to
    /// any known topic. Personalises the message with the user's name.
    /// </summary>
    /// <param name="userName">The user's display name for personalisation.</param>
    public static string GetFallbackMessage(string userName = "Citizen") =>
        $"I'm sorry, {userName}, I didn't quite understand that.\n" +
        "  Try typing one of the following:\n" +
        "  • 'phishing'  to learn about phishing scams\n" +
        "  • 'password'  for password safety advice\n" +
        "  • 'links'     for tips on spotting suspicious links\n" +
        "  • 'help'      for the full topic menu";
}
