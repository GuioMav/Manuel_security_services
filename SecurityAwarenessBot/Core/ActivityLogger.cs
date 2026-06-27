// ============================================================
//  Manuel security services MSS — Cybersecurity Awareness Chatbot
//  Core/ActivityLogger.cs
//  Thread-safe activity log using typed ActivityEntry records.
//  Supports GetRecent, GetAll, and a formatted summary string.
// ============================================================

using SecurityAwarenessBot.Models;

namespace SecurityAwarenessBot.Core;

public static class ActivityLogger
{
    private static readonly object _syncRoot = new();
    private static readonly List<ActivityEntry> _entries = new();

    // ── Write ─────────────────────────────────────────────────────────────────

    /// <summary>
    /// Adds a new timestamped entry to the log.
    /// </summary>
    public static void LogAction(string description)
    {
        if (string.IsNullOrWhiteSpace(description)) return;

        lock (_syncRoot)
        {
            _entries.Add(new ActivityEntry { Description = description.Trim() });
        }
    }

    // ── Read ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns the most recent <paramref name="count"/> entries, newest first.
    /// </summary>
    public static IReadOnlyList<ActivityEntry> GetRecent(int count = 10)
    {
        int safe = Math.Max(0, count);
        lock (_syncRoot)
        {
            return _entries
                .OrderByDescending(e => e.Timestamp)
                .Take(safe)
                .ToList();
        }
    }

    /// <summary>
    /// Returns all entries, newest first.
    /// </summary>
    public static IReadOnlyList<ActivityEntry> GetAll()
    {
        lock (_syncRoot)
        {
            return _entries
                .OrderByDescending(e => e.Timestamp)
                .ToList();
        }
    }

    // ── Formatted Summary ─────────────────────────────────────────────────────

    /// <summary>
    /// Builds a numbered summary string of the last 10 actions for chat display.
    /// </summary>
    public static string GetRecentLogs(int count = 10)
    {
        var recent = GetRecent(count);

        if (recent.Count == 0)
            return "No recent actions have been recorded yet.";

        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Here's a summary of recent actions:");
        sb.AppendLine();

        for (int i = 0; i < recent.Count; i++)
        {
            sb.AppendLine($"  {i + 1}. {recent[i].Display}");
        }

        return sb.ToString().TrimEnd();
    }
}
