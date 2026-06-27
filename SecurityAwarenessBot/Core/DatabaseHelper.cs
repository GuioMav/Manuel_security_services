// ============================================================
//  Manuel security services MSS — Cybersecurity Awareness Chatbot
//  Core/DatabaseHelper.cs
//  Manages all MySQL database interactions for the Task Assistant.
//  Auto-creates the mss_db database and Tasks table on first run.
// ============================================================

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SecurityAwarenessBot.Models;

namespace SecurityAwarenessBot.Core;

public static class DatabaseHelper
{
    // ── Connection Strings ────────────────────────────────────────────────────
    // Update Pwd= below if your MySQL root account has a password set.

    private const string MasterConnectionString =
        "server=localhost;user=root;password=;";

    private const string ConnectionString =
        "server=localhost;user=root;password=;database=mss_db;";

    // ── Initialization ────────────────────────────────────────────────────────

    /// <summary>
    /// Creates the mss_db database and Tasks table if they do not already exist.
    /// Called once at application startup.
    /// </summary>
    public static void InitializeDatabase()
    {
        try
        {
            // Step 1: Create the database if missing
            using (var conn = new MySqlConnection(MasterConnectionString))
            {
                conn.Open();
                using var cmd = new MySqlCommand(
                    "CREATE DATABASE IF NOT EXISTS mss_db;", conn);
                cmd.ExecuteNonQuery();
            }

            // Step 2: Create the Tasks table if missing
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                using var cmd = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id           INT           AUTO_INCREMENT PRIMARY KEY,
                        Title        VARCHAR(255)  NOT NULL,
                        Description  TEXT,
                        ReminderDate DATETIME      NULL,
                        IsCompleted  BOOLEAN       NOT NULL DEFAULT 0,
                        CreatedAt    DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );", conn);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DB Init Error] {ex.Message}");
        }
    }

    // ── Create ────────────────────────────────────────────────────────────────

    /// <summary>
    /// Inserts a new task into the database.
    /// </summary>
    public static void AddTask(TaskItem task)
    {
        try
        {
            using var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new MySqlCommand(@"
                INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted, CreatedAt)
                VALUES (@title, @desc, @reminder, @completed, @created)", conn);

            cmd.Parameters.AddWithValue("@title",     task.Title);
            cmd.Parameters.AddWithValue("@desc",      task.Description);
            cmd.Parameters.AddWithValue("@reminder",  (object?)task.ReminderDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@completed", task.IsCompleted);
            cmd.Parameters.AddWithValue("@created",   DateTime.Now);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DB AddTask Error] {ex.Message}");
        }
    }

    // ── Read ──────────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns all tasks ordered by newest first, incomplete tasks first.
    /// </summary>
    public static List<TaskItem> GetTasks()
    {
        var tasks = new List<TaskItem>();
        try
        {
            using var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new MySqlCommand(@"
                SELECT Id, Title, Description, ReminderDate, IsCompleted
                FROM   Tasks
                ORDER  BY IsCompleted ASC, CreatedAt DESC", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(new TaskItem
                {
                    Id           = reader.GetInt32(0),
                    Title        = reader.GetString(1),
                    Description  = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    ReminderDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                    IsCompleted  = reader.GetBoolean(4)
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DB GetTasks Error] {ex.Message}");
        }
        return tasks;
    }

    // ── Update ────────────────────────────────────────────────────────────────

    /// <summary>
    /// Marks a task as completed by ID.
    /// </summary>
    public static void MarkTaskCompleted(int id)
    {
        try
        {
            using var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new MySqlCommand(
                "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DB Complete Error] {ex.Message}");
        }
    }

    // ── Delete ────────────────────────────────────────────────────────────────

    /// <summary>
    /// Permanently deletes a task by ID.
    /// </summary>
    public static void DeleteTask(int id)
    {
        try
        {
            using var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new MySqlCommand(
                "DELETE FROM Tasks WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[DB Delete Error] {ex.Message}");
        }
    }
}
