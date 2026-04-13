# 🛡️ Manuel security services MSS

[![Build and Validate](https://github.com/GuioMav/Manuel_security_services/actions/workflows/ci.yml/badge.svg)](https://github.com/GuioMav/Manuel_security_services/actions/workflows/ci.yml)

> A C# console application developed for the South African Department of Cybersecurity's public education campaign. Manuel security services MSS educates citizens on **phishing**, **password safety**, and **suspicious links** through an engaging, interactive conversational interface.

---

## 🖥️ Features

| Feature | Description |
|---|---|
| 🎨 ASCII Art Logo | Manuel security services MSS banner rendered in colour on launch |
| 🔊 Welcome Audio | Plays a WAV welcome tone on startup (Windows) |
| ⌨️ Typing Effect | Characters appear one-by-one via `Task.Delay` for an authentic chat feel |
| 🎨 Colour Theming | `ForegroundColor` changes categorise message types |
| 🖼️ Decorative Borders | Box-drawing unicode characters frame sections |
| 🛡️ Phishing Guidance | In-depth phishing awareness with SA-specific context |
| 🔑 Password Safety | Strong password tips and tools recommended |
| 🔗 Suspicious Links | How to spot and verify URLs safely |
| 💡 General Tips | Device, network, and account protection advice |
| 📋 Input Validation | Empty strings and unknown inputs handled gracefully |
| 🎯 5-Question Quiz | Interactive multiple-choice cybersecurity quiz |
| 🧑 Personalisation | Name-based greeting and session ID throughout |

---

## 🏗️ Project Architecture

```
SecurityAwarenessBot/
├── .github/
│   └── workflows/
│       └── ci.yml              ← GitHub Actions: build on every push
├── SecurityAwarenessBot/
│   ├── SecurityAwarenessBot.csproj
│   ├── Program.cs              ← Entry point: launch, onboarding, chat loop
│   ├── Models/
│   │   └── User.cs             ← Auto-properties: Name, SessionId, SessionStart
│   ├── UI/
│   │   └── UserInterface.cs    ← All console output: colours, borders, typing effect
│   ├── Core/
│   │   ├── ChatEngine.cs       ← Input classification, quiz state machine
│   │   ├── InputValidator.cs   ← Empty string, exit, help, sanitisation
│   │   └── ResponseLibrary.cs  ← All educational response strings
│   └── Utils/
│       └── AudioPlayer.cs      ← System.Media SoundPlayer (Windows) + WAV generator
└── README.md
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or later
- JetBrains Rider, Visual Studio 2022+, or VS Code with C# Dev Kit

### Run from Rider

1. Open the `SecurityAwarenessBot` folder in JetBrains Rider
2. Select the `SecurityAwarenessBot` run configuration
3. Press **Run** (▶)

### Run from Terminal

```bash
cd SecurityAwarenessBot
dotnet run
```

### Build in Release mode

```bash
dotnet build SecurityAwarenessBot.csproj --configuration Release
```

---

## 💬 Using the Chatbot

Once running, type any of the following keywords (or related phrases) to explore topics:

| Keyword | Topic |
|---|---|
| `phishing` | Learn about phishing scams, red flags, and how to report them |
| `password` | Strong password guidance and recommended tools |
| `links` | Identifying suspicious URLs & what to do after clicking one |
| `tips` | General cybersecurity hygiene for South Africans |
| `purpose` | What Manuel security services MSS is and how it protects your privacy |
| `quiz` | Start a 5-question cybersecurity knowledge challenge |
| `help` | Display the full topic menu |
| `exit` | Gracefully end your session |

---

## 🧱 Class Responsibilities

### `User.cs` — Model
Uses **automatic properties** to store:
- `Name` — entered during onboarding
- `SessionId` — auto-generated 8-char hex identifier
- `SessionStart` — `DateTime.Now` captured on construction
- `GetSessionDuration()` — derived elapsed time string

### `UserInterface.cs` — Presentation Layer
Centralises **all** console output:
- `SetColour()` / `WriteColoured()` — `Console.ForegroundColor` management
- `TypeWriteAsync()` — typewriter effect using `await Task.Delay(delayMs)`
- `DrawBorder()` / `DrawBox()` — unicode box-drawing characters
- `DisplayLogo()` — multi-colour ASCII art (Manuel security services MSS)
- `PrintHelpMenu()` / `PrintWarning()` / `PrintSuccess()` — status helpers

### `InputValidator.cs` — Validation
- `IsEmpty()` — guards against empty/whitespace input
- `IsExitCommand()` / `IsHelpCommand()` — keyword detection
- `Sanitise()` — trim, lowercase, collapse whitespace
- `GetFallbackMessage()` — helpful fallback for unknown commands

### `ChatEngine.cs` — Business Logic
- `ClassifyInput()` — maps sanitised input → `Topic` enum via `ContainsAny()`
- `GetResponseAsync()` — orchestrates routing, quiz state, control codes
- Quiz state machine: `QuizState` enum, 5-question sequential flow

### `AudioPlayer.cs` — Audio
- Windows runtime guard via `RuntimeInformation.IsOSPlatform()`
- `EnsureWavFileExists()` — programmatic WAV generator (no external assets needed)
- Wrapped in `try/catch` — audio failures never crash the bot

---

## 🔄 CI/CD — GitHub Actions

The workflow at `.github/workflows/ci.yml` triggers on every `push` and `pull_request`:

1. **Checkout** repository
2. **Setup .NET 6** via `actions/setup-dotnet@v3`
3. **Restore** packages
4. **Build** in Release mode
5. **Publish** to confirm no missing assets

---

## 🔐 Privacy Statement

Manuel security services MSS does **not** collect, store, or transmit any personal information. All data (your name and session details) exists only in memory for the duration of the session and is discarded on exit.

---

## 📚 South African Cybersecurity Resources

| Resource | Contact |
|---|---|
| SA CERT (Cyber Incident Response) | www.cert.org.za |
| SABRIC (Banking Fraud) | www.sabric.co.za |
| SAPS Cybercrime Reporting | 10111 or nearest police station |
| Have I Been Pwned? | https://haveibeenpwned.com |
| VirusTotal URL Scanner | https://www.virustotal.com |

---

## 📝 Commit History

This project follows a highly granular development process with **26 commits**, each representing a discrete unit of work. See the full commit log via `git log --oneline`.

---

*Built with ❤️ for South Africa — Manuel security services MSS, PROG6211, 2024*
