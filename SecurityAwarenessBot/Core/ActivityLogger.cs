using System;
using System.Collections.Generic;

namespace SecurityAwarenessBot.Core;

public static class ActivityLogger
{
    private static readonly List<string> _logs = new();

    public static void LogAction(string description)
    {
        string entry = $"[{DateTime.Now:HH:mm}] {description}";
        _logs.Add(entry);
    }

    public static string GetRecentLogs(int count = 5)
    {
        if (_logs.Count == 0)
            return "No recent actions recorded.";

        int startIndex = Math.Max(0, _logs.Count - count);
        var recent = _logs.GetRange(startIndex, _logs.Count - startIndex);
        
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Here’s a summary of recent actions:");
        
        for (int i = 0; i < recent.Count; i++)
        {
            sb.AppendLine($"{i + 1}. {recent[i]}");
        }

        return sb.ToString().TrimEnd();
    }
}
