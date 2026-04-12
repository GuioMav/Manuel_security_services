// ============================================================
//  CyberShield SA — Cybersecurity Awareness Chatbot
//  UI/UserInterface.cs
//  Centralises all console presentation logic:
//    • Colour helpers (ForegroundColor management)
//    • ASCII art logo
//    • Decorative border rendering
//    • Typing / typewriter effect via Task.Delay
//    • Input prompt
//    • Status message helpers
//    • Help menu
// ============================================================

namespace SecurityAwarenessBot.UI;

/// <summary>
/// Static helper class responsible for every visual element rendered to the console.
/// Keeps all presentation concerns out of Program.cs and the domain classes.
/// </summary>
public static class UserInterface
{
    // ── 1. Colour helpers ─────────────────────────────────────────────────────

    /// <summary>Sets the console foreground to the specified colour.</summary>
    public static void SetColour(ConsoleColor colour) =>
        Console.ForegroundColor = colour;

    /// <summary>Resets the console colour to its default.</summary>
    public static void ResetColour() => Console.ResetColor();

    /// <summary>Writes text in the given colour without appending a newline.</summary>
    public static void WriteColoured(string text, ConsoleColor colour)
    {
        SetColour(colour);
        Console.Write(text);
        ResetColour();
    }

    /// <summary>Writes text in the given colour and appends a newline.</summary>
    public static void WriteLineColoured(string text, ConsoleColor colour)
    {
        SetColour(colour);
        Console.WriteLine(text);
        ResetColour();
    }

    // ── 2. Typing / typewriter effect ─────────────────────────────────────────

    /// <summary>
    /// Prints each character individually with a configurable delay between them,
    /// creating a typewriter / "chat typing" animation effect.
    /// Uses <see cref="Task.Delay"/> so the UI remains responsive.
    /// </summary>
    /// <param name="text">The text to display character by character.</param>
    /// <param name="colour">Foreground colour while typing.</param>
    /// <param name="delayMs">Milliseconds between each character (default 28 ms).</param>
    public static async Task TypeWriteAsync(
        string text,
        ConsoleColor colour = ConsoleColor.White,
        int delayMs = 28)
    {
        SetColour(colour);
        foreach (char c in text)
        {
            Console.Write(c);
            await Task.Delay(delayMs);
        }
        ResetColour();
        Console.WriteLine();
    }

    // ── 3. Decorative borders ─────────────────────────────────────────────────

    /// <summary>
    /// Draws a full-width horizontal border using the specified character and colour.
    /// </summary>
    /// <param name="width">Total width of the border in characters (default 72).</param>
    /// <param name="ch">The character used to fill the border (default '═').</param>
    /// <param name="colour">Foreground colour of the border.</param>
    public static void DrawBorder(
        int width = 72,
        char ch = '═',
        ConsoleColor colour = ConsoleColor.Cyan)
    {
        WriteLineColoured(new string(ch, width), colour);
    }

    /// <summary>
    /// Draws a centred box: top border, centred title, bottom border.
    /// </summary>
    public static void DrawBox(
        string title,
        int width = 72,
        ConsoleColor borderColour = ConsoleColor.Cyan,
        ConsoleColor titleColour = ConsoleColor.Yellow)
    {
        DrawBorder(width, '═', borderColour);

        // Centre the title within the box width
        int contentWidth = width - 4;                   // leave 2 chars on each side
        int leftPad = (contentWidth - title.Length) / 2;
        int rightPad = contentWidth - title.Length - leftPad;
        string centred = $"║  {new string(' ', leftPad)}{title}{new string(' ', rightPad)}  ║";

        WriteLineColoured(centred, titleColour);
        DrawBorder(width, '═', borderColour);
    }

    // ── 4. Input prompt ───────────────────────────────────────────────────────

    /// <summary>
    /// Displays a stylised input prompt and returns the raw string the user typed.
    /// </summary>
    /// <param name="promptLabel">Label shown in the prompt (e.g. the user's name).</param>
    public static string ReadInput(string promptLabel = "You")
    {
        WriteColoured($"\n  [{promptLabel}] › ", ConsoleColor.Green);
        SetColour(ConsoleColor.White);
        string? input = Console.ReadLine();
        ResetColour();
        return input ?? string.Empty;
    }

    // ── 5. ASCII art logo ─────────────────────────────────────────────────────

    /// <summary>
    /// Clears the console and renders the CyberShield SA ASCII-art banner.
    /// Uses distinct colours to split the wordmark from the tagline.
    /// </summary>
    public static void DisplayLogo()
    {
        Console.Clear();

        // Primary wordmark — cyan
        SetColour(ConsoleColor.Cyan);
        Console.WriteLine();
        Console.WriteLine(@"   ██████╗██╗   ██╗██████╗ ███████╗██████╗ ███████╗██╗  ██╗██╗███████╗██╗     ██████╗ ");
        Console.WriteLine(@"  ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔════╝██║  ██║██║██╔════╝██║     ██╔══██╗");
        Console.WriteLine(@"  ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝███████╗███████║██║█████╗  ██║     ██║  ██║");
        Console.WriteLine(@"  ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗╚════██║██╔══██║██║██╔══╝  ██║     ██║  ██║");
        Console.WriteLine(@"  ╚██████╗   ██║   ██████╔╝███████╗██║  ██║███████║██║  ██║██║███████╗███████╗██████╔╝");
        Console.WriteLine(@"   ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═╝╚══════╝╚══════╝╚═════╝ ");
        ResetColour();

        // "SA" accent — yellow
        SetColour(ConsoleColor.Yellow);
        Console.WriteLine();
        Console.WriteLine(@"                          ███████╗ █████╗ ");
        Console.WriteLine(@"                          ██╔════╝██╔══██╗");
        Console.WriteLine(@"                          ███████╗███████║");
        Console.WriteLine(@"                          ╚════██║██╔══██║");
        Console.WriteLine(@"                          ███████║██║  ██║");
        Console.WriteLine(@"                          ╚══════╝╚═╝  ╚═╝");
        ResetColour();

        Console.WriteLine();

        // Shield tagline — dark cyan
        SetColour(ConsoleColor.DarkCyan);
        Console.WriteLine("          🛡️  Protecting South Africa — One Click at a Time  🛡️");
        ResetColour();

        DrawBorder(72, '─', ConsoleColor.DarkGray);
        Console.WriteLine();
    }

    // ── 6. Status message helpers ─────────────────────────────────────────────

    /// <summary>Displays an informational (cyan) message.</summary>
    public static void PrintInfo(string message) =>
        WriteLineColoured($"  ℹ  {message}", ConsoleColor.Cyan);

    /// <summary>Displays a warning (yellow) message.</summary>
    public static void PrintWarning(string message) =>
        WriteLineColoured($"  ⚠  {message}", ConsoleColor.Yellow);

    /// <summary>Displays a success (green) confirmation message.</summary>
    public static void PrintSuccess(string message) =>
        WriteLineColoured($"  ✔  {message}", ConsoleColor.Green);

    /// <summary>Displays an error (red) message.</summary>
    public static void PrintError(string message) =>
        WriteLineColoured($"  ✖  {message}", ConsoleColor.Red);

    // ── 7. Help menu ──────────────────────────────────────────────────────────

    /// <summary>
    /// Prints a formatted, colour-coded list of all topics the chatbot supports.
    /// </summary>
    public static void PrintHelpMenu()
    {
        Console.WriteLine();
        DrawBorder(72, '─', ConsoleColor.DarkGray);
        WriteLineColoured("  📋  Available Topics — type any keyword to explore:", ConsoleColor.Cyan);
        DrawBorder(72, '─', ConsoleColor.DarkGray);
        WriteLineColoured("  • phishing    — Learn about phishing scams & how to spot them", ConsoleColor.White);
        WriteLineColoured("  • password    — Password safety tips & best practices", ConsoleColor.White);
        WriteLineColoured("  • links       — How to identify suspicious URLs & links", ConsoleColor.White);
        WriteLineColoured("  • tips        — General cybersecurity advice for South Africans", ConsoleColor.White);
        WriteLineColoured("  • purpose     — What this chatbot is and who built it", ConsoleColor.White);
        WriteLineColoured("  • quiz        — Test your cybersecurity knowledge", ConsoleColor.White);
        WriteLineColoured("  • exit / quit — End your session safely", ConsoleColor.White);
        DrawBorder(72, '─', ConsoleColor.DarkGray);
        Console.WriteLine();
    }
}
