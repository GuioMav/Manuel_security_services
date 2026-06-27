using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SecurityAwarenessBot.Models;

namespace SecurityAwarenessBot.Core;

public class DatabaseHelper
{
    private const string MasterConnectionString = "Server=127.0.0.1;Uid=root;Pwd=;";
    private const string ConnectionString = "Server=127.0.0.1;Uid=root;Pwd=;Database=mss_db;";

    public static void InitializeDatabase()
    {
        try
        {
            using (var connection = new MySqlConnection(MasterConnectionString))
            {
                connection.Open();
                using var cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS mss_db;", connection);
                cmd.ExecuteNonQuery();
            }

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using var cmd = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Title VARCHAR(255) NOT NULL,
                        Description TEXT,
                        ReminderDate DATETIME NULL,
                        IsCompleted BOOLEAN NOT NULL DEFAULT 0
                    );", connection);
                cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex.Message}");
        }
    }

    public static void AddTask(TaskItem task)
    {
        try
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            using var cmd = new MySqlCommand(
                "INSERT INTO Tasks (Title, Description, ReminderDate, IsCompleted) VALUES (@t, @d, @r, @c)", connection);
            cmd.Parameters.AddWithValue("@t", task.Title);
            cmd.Parameters.AddWithValue("@d", task.Description);
            cmd.Parameters.AddWithValue("@r", task.ReminderDate);
            cmd.Parameters.AddWithValue("@c", task.IsCompleted);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex.Message}");
        }
    }

    public static List<TaskItem> GetTasks()
    {
        var tasks = new List<TaskItem>();
        try
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            using var cmd = new MySqlCommand("SELECT Id, Title, Description, ReminderDate, IsCompleted FROM Tasks ORDER BY Id DESC", connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(new TaskItem
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    ReminderDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                    IsCompleted = reader.GetBoolean(4)
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex.Message}");
        }
        return tasks;
    }

    public static void MarkTaskCompleted(int id)
    {
        try
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            using var cmd = new MySqlCommand("UPDATE Tasks SET IsCompleted = 1 WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex.Message}");
        }
    }

    public static void DeleteTask(int id)
    {
        try
        {
            using var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            using var cmd = new MySqlCommand("DELETE FROM Tasks WHERE Id = @id", connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"DB Error: {ex.Message}");
        }
    }
}
