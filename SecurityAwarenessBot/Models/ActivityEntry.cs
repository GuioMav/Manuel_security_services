// ============================================================
//  Manuel security services MSS — Cybersecurity Awareness Chatbot
//  Models/ActivityEntry.cs
//  Typed model for a single activity log entry, with a formatted
//  timestamp and display string for the chat UI.
// ============================================================

namespace SecurityAwarenessBot.Models;

/// <summary>
/// Represents a single timestamped entry in the activity log.
/// </summary>
public class ActivityEntry
{
    public DateTime Timestamp { get; init; } = DateTime.Now;
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Formatted display string shown to the user, e.g. "[10:32] Task added: Enable 2FA"
    /// </summary>
    public string Display => $"[{Timestamp:HH:mm}] {Description}";
}
