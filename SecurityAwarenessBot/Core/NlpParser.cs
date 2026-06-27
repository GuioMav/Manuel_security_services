// ============================================================
//  Manuel security services MSS — Cybersecurity Awareness Chatbot
//  Core/NlpParser.cs
//  Simulates Natural Language Processing using keyword detection
//  and regex patterns to extract task intent, task text, and
//  reminder dates from free-form user input.
//
//  Inspired by and expanded upon patterns from ChatBot3 (JeciraMiguel).
// ============================================================

using System.Globalization;
using System.Text.RegularExpressions;

namespace SecurityAwarenessBot.Core;

// ── Intent Enum ───────────────────────────────────────────────────────────────

/// <summary>
/// Represents a recognised user intent extracted by the NLP parser.
/// </summary>
public enum UserIntent
{
    None,
    AddTask,
    ShowTasks,
    SetReminder,
    StartQuiz,
    ShowActivityLog
}

// ── NLP Result ────────────────────────────────────────────────────────────────

/// <summary>
/// Encapsulates the result of an NLP analysis, including the intent,
/// extracted task text, and optional reminder date.
/// </summary>
public class NlpResult
{
    public UserIntent Intent    { get; init; }
    public string?   TaskText  { get; init; }
    public DateTime? ReminderAt { get; init; }
}

// ── NLP Parser ────────────────────────────────────────────────────────────────

/// <summary>
/// Provides NLP simulation via keyword detection and multi-pass regex.
/// Understands varied phrasing for adding tasks, setting reminders,
/// viewing tasks, starting the quiz, and viewing the activity log.
/// </summary>
public static class NlpParser
{
    // ── Phrase dictionaries (case-insensitive matching) ───────────────────────

    private static readonly string[] AddTaskPhrases =
    {
        "add task", "add a task", "create task", "create a task",
        "make task", "make a task", "new task", "remember to",
        "i need to", "set up", "add to my tasks"
    };

    private static readonly string[] ReminderPhrases =
    {
        "remind me", "set a reminder", "set reminder",
        "add a reminder", "add reminder"
    };

    private static readonly string[] ShowTaskPhrases =
    {
        "show tasks", "show my tasks", "view tasks",
        "list tasks", "list my tasks", "my tasks", "open tasks", "show all tasks"
    };

    private static readonly string[] QuizPhrases =
    {
        "start quiz", "play quiz", "begin quiz",
        "open quiz", "cybersecurity quiz", "start the quiz", "take the quiz"
    };

    private static readonly string[] ActivityPhrases =
    {
        "show activity log", "activity log", "what have you done",
        "recent actions", "show log", "open activity", "show activity",
        "what actions", "history"
    };

    // ── Public Entry Point ────────────────────────────────────────────────────

    /// <summary>
    /// Analyses raw user input and returns a structured <see cref="NlpResult"/>.
    /// Returns <see cref="UserIntent.None"/> when no intent is detected.
    /// </summary>
    public static NlpResult Analyse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new NlpResult { Intent = UserIntent.None };

        // Normalise whitespace, keep original casing for extraction
        string clean = Regex.Replace(input.Trim(), @"\s+", " ");
        string lower = clean.ToLowerInvariant();

        // Priority order: most-specific intents first
        if (ContainsAny(lower, ActivityPhrases))
            return new NlpResult { Intent = UserIntent.ShowActivityLog };

        if (ContainsAny(lower, QuizPhrases))
            return new NlpResult { Intent = UserIntent.StartQuiz };

        if (ContainsAny(lower, ShowTaskPhrases))
            return new NlpResult { Intent = UserIntent.ShowTasks };

        // Parse reminder date before checking add-task and remind-me
        DateTime? reminderAt = ParseReminder(lower);

        bool asksToAdd =
            ContainsAny(lower, AddTaskPhrases) ||
            (lower.Contains("task") &&
             (lower.Contains("add") || lower.Contains("create") || lower.Contains("make")));

        if (asksToAdd)
        {
            return new NlpResult
            {
                Intent    = UserIntent.AddTask,
                TaskText  = ExtractTaskText(clean),
                ReminderAt = reminderAt
            };
        }

        if (ContainsAny(lower, ReminderPhrases))
        {
            return new NlpResult
            {
                Intent    = UserIntent.SetReminder,
                TaskText  = ExtractTaskText(clean),
                ReminderAt = reminderAt
            };
        }

        return new NlpResult { Intent = UserIntent.None };
    }

    // ── Date Parsing ──────────────────────────────────────────────────────────

    /// <summary>
    /// Extracts a reminder <see cref="DateTime"/> from natural-language phrases
    /// such as "tomorrow", "in 3 days", "next week", "on 15/07", "at 3pm".
    /// Returns null if no date is found.
    /// </summary>
    private static DateTime? ParseReminder(string lower)
    {
        DateTime? baseDate = null;

        // ── Relative day keywords ─────────────────────────────────────────────
        if (lower.Contains("tomorrow"))
            baseDate = DateTime.Today.AddDays(1);
        else if (lower.Contains("today"))
            baseDate = DateTime.Today;
        else if (lower.Contains("next week"))
            baseDate = DateTime.Today.AddDays(7);

        // ── "in N days / weeks" ───────────────────────────────────────────────
        var relativeMatch = Regex.Match(lower,
            @"\bin\s+(\d+)\s+(day|days|week|weeks)\b",
            RegexOptions.IgnoreCase);

        if (relativeMatch.Success &&
            int.TryParse(relativeMatch.Groups[1].Value, out int amount))
        {
            string unit = relativeMatch.Groups[2].Value.ToLowerInvariant();
            int days = unit.StartsWith("week") ? amount * 7 : amount;
            baseDate = DateTime.Today.AddDays(days);
        }

        // ── Exact numeric date: dd/MM or dd/MM/yyyy ───────────────────────────
        var numericDate = Regex.Match(lower,
            @"\b(\d{1,2})[/\-](\d{1,2})(?:[/\-](\d{2,4}))?\b");

        if (numericDate.Success)
        {
            int day   = int.Parse(numericDate.Groups[1].Value);
            int month = int.Parse(numericDate.Groups[2].Value);
            int year  = numericDate.Groups[3].Success
                ? int.Parse(numericDate.Groups[3].Value)
                : DateTime.Today.Year;

            if (year < 100) year += 2000;

            if (DateTime.TryParseExact(
                    $"{day:00}/{month:00}/{year:0000}",
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsed))
            {
                baseDate = parsed.Date;
            }
        }

        // ── Time of day: "at 3pm", "at 14:30" ────────────────────────────────
        TimeSpan? time = ParseTime(lower);

        if (baseDate.HasValue)
        {
            // Combine date + time (default 09:00 if no time given)
            return baseDate.Value.Date.Add(time ?? TimeSpan.FromHours(9));
        }

        if (time.HasValue)
        {
            // Time only — assume today; if already past, push to tomorrow
            DateTime candidate = DateTime.Today.Add(time.Value);
            if (candidate <= DateTime.Now) candidate = candidate.AddDays(1);
            return candidate;
        }

        return null;
    }

    /// <summary>
    /// Parses 12-hour (3pm, 3:30pm) and 24-hour (14:30) time expressions.
    /// </summary>
    private static TimeSpan? ParseTime(string lower)
    {
        // 12-hour: at 3pm / at 3:30pm
        var m12 = Regex.Match(lower,
            @"\bat\s+(\d{1,2})(?::(\d{2}))?\s*(am|pm)\b",
            RegexOptions.IgnoreCase);

        if (m12.Success)
        {
            int hour   = int.Parse(m12.Groups[1].Value);
            int minute = m12.Groups[2].Success ? int.Parse(m12.Groups[2].Value) : 0;
            string period = m12.Groups[3].Value.ToLowerInvariant();

            if (hour == 12) hour = 0;
            if (period == "pm") hour += 12;

            if (hour is >= 0 and <= 23 && minute is >= 0 and <= 59)
                return new TimeSpan(hour, minute, 0);
        }

        // 24-hour: at 14:30
        var m24 = Regex.Match(lower,
            @"\bat\s+([01]?\d|2[0-3]):([0-5]\d)\b",
            RegexOptions.IgnoreCase);

        if (m24.Success)
        {
            int hour   = int.Parse(m24.Groups[1].Value);
            int minute = int.Parse(m24.Groups[2].Value);
            return new TimeSpan(hour, minute, 0);
        }

        return null;
    }

    // ── Task Text Extraction ──────────────────────────────────────────────────

    /// <summary>
    /// Strips NLP trigger words from the input to produce a clean task title.
    /// Uses multiple sequential regex passes for maximum flexibility.
    /// </summary>
    private static string ExtractTaskText(string input)
    {
        string result = input;

        // Strip polite prefixes: "please can you...", "could you..."
        result = Regex.Replace(result,
            @"(?i)^\s*(please\s+)?(can|could|would)\s+you\s+", "");

        // Strip add/create/make task triggers
        result = Regex.Replace(result,
            @"(?i)\b(add|create|make|new)\s+(a\s+)?task\s*(to|for|:|-|about)?\s*", "");

        // Strip "remember to ..."
        result = Regex.Replace(result,
            @"(?i)\bremember\s+to\s+", "");

        // Strip "set a reminder for/to ..."
        result = Regex.Replace(result,
            @"(?i)\bset\s+(a\s+)?reminder\s+(for|to)?\s*", "");

        // Strip "remind me to ..."
        result = Regex.Replace(result,
            @"(?i)\bremind\s+me\s+(to\s+)?", "");

        // Strip "i need to ..."
        result = Regex.Replace(result,
            @"(?i)\bi\s+need\s+to\s+", "");

        // Strip date/time references so they don't appear in the title
        result = Regex.Replace(result,
            @"(?i)\b(today|tomorrow|next\s+week)\b", "");
        result = Regex.Replace(result,
            @"(?i)\bin\s+\d+\s+(day|days|week|weeks)\b", "");
        result = Regex.Replace(result,
            @"(?i)\bon\s+\d{1,2}[/\-]\d{1,2}(?:[/\-]\d{2,4})?\b", "");
        result = Regex.Replace(result,
            @"(?i)\bat\s+\d{1,2}(?::\d{2})?\s*(am|pm)?\b", "");

        // Collapse extra whitespace and trim punctuation
        result = Regex.Replace(result, @"\s+", " ");
        return result.Trim(' ', '.', ',', ';', ':', '-', '!', '?');
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static bool ContainsAny(string input, IEnumerable<string> phrases) =>
        phrases.Any(p => input.Contains(p, StringComparison.OrdinalIgnoreCase));
}
