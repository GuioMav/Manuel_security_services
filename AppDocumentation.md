# 🛡️ Manuel security services MSS - Project Documentation

## 🌟 Overview
**Manuel security services MSS** is a C# console-based Cybersecurity Awareness Chatbot. It was developed to support public education campaigns (specifically the South African Department of Cybersecurity) by providing citizens with an interactive and engaging platform to learn about digital safety.

The primary goal of the application is to translate complex cybersecurity concepts into simple, actionable guidance for non-technical users.

---

## 🛠️ What it Does
The app acts as a personal security assistant that users can interact with via "keywords" or natural-language phrases.

### Key Capabilities:
1. **Educational Chat**: Provides detailed advice on specific threats:
    - **Phishing**: How to spot fake emails and SMS scams.
    - **Password Safety**: Best practices for creating strong, unhackable passwords.
    - **Suspicious Links**: Identifying malicious URLs before clicking.
    - **General Hygiene**: Device encryption, network safety, and 2FA.
2. **Interactive Quiz**: A 5-question challenge that tests the user's knowledge on the topics covered, providing immediate feedback.
3. **Immersive UI**:
    - **ASCII Art Branding**: A custom "MSS" wordmark on launch.
    - **Typewriter Effect**: Messages appear character-by-character to simulate a real chat experience.
    - **Audio Feedback**: Plays a custom welcome intro (`MSS_audio_intro.wav`) on Windows systems.
    - **Personalization**: Tracks user names and session IDs for a tailored experience.

---

## 🏗️ Architecture
The project follows a **Modular Layered Architecture**, separating presentation, logic, and data.

### 🧩 Core Components:

#### 1. Orchestration Layer (`Program.cs`)
The entry point of the application. It handles the boot sequence (audio, logo), user onboarding, and the main conversational loop. It coordinates between the UI and the Chat Engine.

#### 2. Presentation Layer (`UI/`)
-   **`UserInterface.cs`**: The only class allowed to write to the console. It manages all colors, unicode borders, ASCII art rendering, and the asynchronous "typing" effect.

#### 3. Intelligence Layer (`Core/`)
-   **`ChatEngine.cs`**: The "brain" of the bot. It uses keyword-based classification to map user input to educational topics. It also manages the **State Machine** for the interactive quiz.
-   **`ResponseLibrary.cs`**: A static repository of educational content. It returns formatted strings (personalized with the user's name) for various security topics.
-   **`InputValidator.cs`**: Sanitizes and validates user input (trimming, lowercasing, empty-check) before it reaches the intelligence layer.

#### 4. Data Layer (`Models/`)
-   **`User.cs`**: A simple data model that stores session metadata, including the user's name, a unique Session ID, and the session start time for duration tracking.

#### 5. Utility Layer (`Utils/` & `audio/`)
-   **`AudioPlayer.cs`**: Manages audio playback. It includes a programmatic fallback to generate a tone if the physical `.wav` file is missing.
-   **`audio/`**: Stores custom high-quality audio assets like `MSS_audio_intro.wav`.

---

## 🚀 Technology Stack
-   **Language**: C# 10+
-   **Framework**: .NET 10 (Cross-platform support)
-   **Output**: High-fidelity Console Interface (UTF-8 enabled)
-   **Deployment**: Portable executable with embedded assets.

---

> *Built for Security Excellence — Manuel security services MSS*
