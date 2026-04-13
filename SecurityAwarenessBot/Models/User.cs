// ============================================================
//  Manuel security services MSS — Cybersecurity Awareness Chatbot
//  Models/User.cs
//  Represents a chatbot session user with automatic properties
//  for personalisation throughout the conversation.
// ============================================================

namespace SecurityAwarenessBot.Models;

/// <summary>
/// Stores session-specific user data.
/// All properties use C# automatic property syntax with default values.
/// </summary>
public class User
{
    // ── Automatic Properties ─────────────────────────────────────────────────

    /// <summary>The display name entered by the user during onboarding.</summary>
    public string Name { get; set; } = "Citizen";

    /// <summary>
    /// A short, unique identifier for the session (8 hex chars, uppercase).
    /// Generated automatically at object creation.
    /// </summary>
    public string SessionId { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpper();

    /// <summary>Timestamp captured when the User object is instantiated.</summary>
    public DateTime SessionStart { get; set; } = DateTime.Now;

    // ── Derived / helper members ─────────────────────────────────────────────

    /// <summary>
    /// Returns a formatted welcome string using the user's name and session ID.
    /// </summary>
    public string GetGreeting() =>
        $"Welcome, {Name}! Your session ID is {SessionId}.";

    /// <summary>
    /// Calculates and formats the elapsed session time at the moment of calling.
    /// </summary>
    public string GetSessionDuration()
    {
        TimeSpan elapsed = DateTime.Now - SessionStart;
        int minutes = (int)elapsed.TotalMinutes;
        int seconds = elapsed.Seconds;
        return $"{minutes} minute(s) and {seconds} second(s)";
    }
}
