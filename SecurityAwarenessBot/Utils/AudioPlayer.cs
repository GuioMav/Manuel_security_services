// ============================================================
//  CyberShield SA — Cybersecurity Awareness Chatbot
//  Utils/AudioPlayer.cs
//  Wraps System.Media.SoundPlayer for welcome audio playback.
//
//  Design notes:
//  • System.Media is Windows-only; a runtime OS check guards the call
//    so the project compiles and runs on macOS/Linux (CI runs ubuntu).
//  • If no WAV file exists, one is generated programmatically via a
//    pure-C# WAV encoder (440 Hz sine wave, 2 s, fade in/out).
//  • Any audio error is caught and logged without crashing the bot.
// ============================================================

using System.Runtime.InteropServices;

namespace SecurityAwarenessBot.Utils;

/// <summary>
/// Handles welcome audio playback using <see cref="System.Media.SoundPlayer"/>.
/// Audio is silently skipped on non-Windows operating systems.
/// </summary>
public static class AudioPlayer
{
    // Resolve the Audio/ subdirectory relative to the executing assembly
    private static readonly string AudioDirectory =
        Path.Combine(AppContext.BaseDirectory, "Audio");

    private static readonly string WavFilePath =
        Path.Combine(AudioDirectory, "welcome.wav");

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>
    /// Plays the welcome WAV file asynchronously.
    /// On non-Windows platforms the call returns immediately without error.
    /// </summary>
    public static async Task PlayWelcomeAsync()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Gracefully skip audio on macOS / Linux
            await Task.CompletedTask;
            return;
        }

        try
        {
            EnsureWavFileExists();

#pragma warning disable CA1416  // Validated above — Windows only
            using var player = new System.Media.SoundPlayer(WavFilePath);
            player.Play();       // Non-blocking: returns while audio plays
#pragma warning restore CA1416
        }
        catch (Exception ex)
        {
            // Audio failure must never bring down the chatbot
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  [Audio unavailable: {ex.Message}]");
            Console.ResetColor();
        }

        await Task.CompletedTask;
    }

    // ── WAV generation ────────────────────────────────────────────────────────

    /// <summary>
    /// Creates a 440 Hz (A4) sine-wave WAV file at <see cref="WavFilePath"/>
    /// if the file does not already exist.
    /// The wave uses a short fade-in and fade-out envelope.
    /// </summary>
    private static void EnsureWavFileExists()
    {
        if (File.Exists(WavFilePath)) return;

        Directory.CreateDirectory(AudioDirectory);

        const int    sampleRate      = 44100;
        const int    durationSeconds = 2;
        const double frequency       = 440.0;   // A4 note
        const double amplitude       = 0.4;     // 40 % max volume

        int numSamples = sampleRate * durationSeconds;

        using var memStream = new MemoryStream();
        using var writer    = new BinaryWriter(memStream);

        // ── RIFF / WAVE header ───────────────────────────────────────────────
        writer.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
        writer.Write(36 + numSamples * 2);                          // Chunk size
        writer.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));

        // ── fmt sub-chunk ────────────────────────────────────────────────────
        writer.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
        writer.Write(16);           // Sub-chunk size (PCM = 16)
        writer.Write((short)1);     // Audio format: 1 = PCM (uncompressed)
        writer.Write((short)1);     // Channels: 1 = mono
        writer.Write(sampleRate);   // Sample rate: 44 100 Hz
        writer.Write(sampleRate * 2); // Byte rate = SampleRate * BlockAlign
        writer.Write((short)2);     // Block align = channels * (bits/8)
        writer.Write((short)16);    // Bits per sample

        // ── data sub-chunk ───────────────────────────────────────────────────
        writer.Write(System.Text.Encoding.ASCII.GetBytes("data"));
        writer.Write(numSamples * 2);

        for (int i = 0; i < numSamples; i++)
        {
            double t = (double)i / sampleRate;

            // Linear fade-in (first 0.1 s) and fade-out (last 0.1 s)
            double fadeIn  = Math.Min(1.0, t / 0.1);
            double fadeOut = Math.Min(1.0, (durationSeconds - t) / 0.1);
            double envelope = Math.Min(fadeIn, fadeOut);

            double sample = amplitude * envelope
                            * Math.Sin(2 * Math.PI * frequency * t);

            writer.Write((short)(short.MaxValue * sample));
        }

        File.WriteAllBytes(WavFilePath, memStream.ToArray());
    }
}
