# 🛡️ Manuel security services MSS

[![Build and Validate](https://github.com/GuioMav/Manuel_security_services/actions/workflows/ci.yml/badge.svg)](https://github.com/GuioMav/Manuel_security_services/actions/workflows/ci.yml)

> A premium WPF Graphical User Interface (GUI) application developed for the South African Department of Cybersecurity's public education campaign. Manuel security services MSS educates citizens on **phishing**, **password safety**, **suspicious links**, **scams**, and **privacy** through an engaging, highly interactive conversational interface.

---

## 🖥️ Features

| Feature | Description |
|---|---|
| 🎨 Full ASCII Brandmark | Renders the complete multi-line block ASCII logo and green tagline directly in the header and onboarding overlay |
| 🔊 Welcome Audio | Plays a WAV welcome tone on startup, with cross-platform programmatic sine-wave generator fallbacks |
| ⌨️ Typewriter Effect | Characters print progressively inside chat bubbles via async WPF routines for an authentic chat feel |
| 🖼️ Custom Speech Bubbles | Pastel-colored user (right) and bot (left) bubbles with rounded borders and shadow effects |
| 🛡️ Keyword Recognition | Identifies cybersecurity keywords naturally (e.g. `"password"`, `"scam"`, `"privacy"`) |
| 🎲 Randomized Responses | Selects randomly from lists of phishing tips to maintain varied and engaging interactions |
| 🔄 Context-Based Flow | Retains conversational context so follow-ups like *"tell me more"* or *"explain"* keep discussing the active topic |
| 🧠 Memory & Recall | Recognizes stated user interests (e.g. `"I'm interested in privacy"`), stores them in memory, and recalls them later |
| 🎭 Sentiment Detection | Detects simple moods (`worried`, `curious`, `frustrated`) to prepend empathetic replies and dedicated fallback options |
| 🏆 Quiz Score Tracker | An interactive 5-question trivia challenge with running scorecards and final scorecard summaries |
| 📋 Input Validation | Prevents empty inputs and handles invalid tokens gracefully |

---

## 🏗️ Project Architecture

```
SecurityAwarenessBot/
├── .github/
│   └── workflows/
│       └── ci.yml              ← GitHub Actions: .NET 9.0 SDK build & validate
├── SecurityAwarenessBot/
│   ├── SecurityAwarenessBot.csproj ← Target: net9.0-windows, UseWPF: true
│   ├── App.xaml                ← Application configuration
│   ├── App.xaml.cs             ← Application startup logic
│   ├── MainWindow.xaml         ← Glassmorphic dark-theme chat layout
│   ├── MainWindow.xaml.cs      ← Onboarding popup, typewriter animation, UI actions
│   ├── Models/
│   │   └── User.cs             ← Session state: Name, SessionId, FavoriteTopic, QuizScore
│   ├── Core/
│   │   ├── ChatEngine.cs       ← Classification, sentiment triggers, memory logic, quiz scoring
│   │   ├── InputValidator.cs   ← Sanitisation, exit, and help checks
│   │   └── ResponseLibrary.cs  ← Sentiment prepends, memory templates, and educational responses
│   └── Utils/
│       └── AudioPlayer.cs      ← System.Media SoundPlayer + programmatic WAV tone synthesizer
└── README.md
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) or later
- Visual Studio 2022 (v17.12 or higher), JetBrains Rider 2024+, or VS Code with C# Dev Kit
- Operating System: **Windows** (to compile and execute WPF Desktop frameworks)

### Run from Visual Studio
1. Open the folder in Visual Studio.
2. Select the `SecurityAwarenessBot` startup configuration.
3. Make sure it targets `.NET 9.0` and press **Start (▶)** or hit **F5**.

### Run from Terminal

```bash
cd SecurityAwarenessBot
dotnet run
```

---

## 💬 Using the Chatbot

Once running, type naturally to explore cybersecurity topics:

| Intent | Sample Input | Bot Response Logic |
|---|---|---|
| **Passwords** | *"Can you help me make a strong password?"* | Specific password criteria and tool safety |
| **Scams** | *"Tell me about SASSA/payment scams"* | Guidance on Banking OTP frauds in SA |
| **Privacy** | *"I want to know about privacy"* | Explains app permissions and locking profiles |
| **Random Tip** | *"Give me a phishing tip"* | Randomly selects a phishing prevention guideline |
| **Continuation**| *"explain more"* or *"tell me more"* | Continues context for the last active topic |
| **Memory Store**| *"I'm interested in privacy"* | Remembers preference and recalls it in later messages |
| **Sentiment** | *"I'm scared I might get hacked"* | EMPATHETIC prepending + immediate security tips |
| **Assessment**| *"I want to take the cybersecurity quiz"* | Multi-turn trivia sequence with scorecards |
| **Help Menu** | *Press Help Button / type "help"* | Shows available commands and exit options |

---

## 🧱 Class Responsibilities

### `User.cs` — Models
Stores session-specific metadata:
- `Name` — entered during initial overlay onboarding.
- `SessionId` — auto-generated 8-char identifier.
- `FavoriteTopic` — stores interest preferences stated by the user.
- `QuizScore` — tracks the number of correct quiz answers.

### `MainWindow.xaml.cs` — Presentation View
Manages all visual actions:
- **Onboarding Grid**: Displays overlay prompts on start.
- **Typewriter Effect**: Sequentially appends characters with color-coded paragraphs (e.g. green success `✔`, yellow warning `⚠`).
- **Dynamic Scrolling**: Scrolls chat logs to the bottom automatically when a new text bubble loads.

### `ChatEngine.cs` — Conversational Intelligence
The core decision-making loop:
- **Keyword Classifier**: Maps input sentences to `Topic` enums.
- **Sentiment Parser**: Scans for worried, curious, or frustrated terms.
- **Memory Recaller**: Injects personalization statements if discussing the favorited interest category.
- **Context Router**: Routes continuation keywords to the `_lastTopic` context.

### `AudioPlayer.cs` — Sound Services
Plays welcome audio triggers asynchronously, guarding calls to Windows-only platforms and generating pure-C# 440 Hz sound chimes as a runtime fallback if asset folders are missing.

---

## 🔄 Release Version Tags

This repository formally marks development progress via two Git release tags:
1.  **`v1.0.0-terminal`**: Represents the complete, working terminal console-based edition of the chatbot.
2.  **`v2.0.0-gui`**: Represents the finished WPF-based GUI app with advanced conversational intelligence.

---

*Built with ❤️ Manuel security services MSS, 2026*
