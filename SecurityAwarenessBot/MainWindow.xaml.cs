using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using SecurityAwarenessBot.Core;
using SecurityAwarenessBot.Models;
using SecurityAwarenessBot.Utils;

namespace SecurityAwarenessBot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private User? _user;
        private ChatEngine? _engine;
        private readonly string _inputPlaceholder = "Ask me anything... (e.g. phishing, passwords, privacy, links)";
        private bool _isBotTyping = false;

        public MainWindow()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();
            SetupOnboardingState();
        }

        // ── Onboarding ────────────────────────────────────────────────────────────

        private void SetupOnboardingState()
        {
            OnboardingGrid.Visibility = Visibility.Visible;
            MainAppGrid.Visibility = Visibility.Collapsed;
            TxtOnboardingName.Focus();
            TxtOnboardingName.Text = string.Empty;
        }

        private async void BtnStartSession_Click(object sender, RoutedEventArgs e)
        {
            await TryOnboardUserAsync();
        }

        private async void TxtOnboardingName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                await TryOnboardUserAsync();
            }
        }

        private async Task TryOnboardUserAsync()
        {
            string userName = TxtOnboardingName.Text.Trim();
            if (string.IsNullOrWhiteSpace(userName))
            {
                MessageBox.Show("Please enter your name so I can personalise your session.", 
                                "Onboarding Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 1. Create User & Chat Engine instance
            _user = new User { Name = userName };
            _engine = new ChatEngine(_user);

            // 2. Hide overlay, reveal main chat UI
            OnboardingGrid.Visibility = Visibility.Collapsed;
            MainAppGrid.Visibility = Visibility.Visible;

            // 3. Update header status displays
            TxtHeaderUser.Text = $"👤 User: {_user.Name}";

            // 4. Initialize Chat Input Box with placeholder
            TxtChatInput.Text = _inputPlaceholder;
            TxtChatInput.Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88));

            // 5. Play Welcome Audio (Windows sound playback / Programmatic synth tone fallback)
            await AudioPlayer.PlayWelcomeAsync();

            // 6. Add bot opening messages with typewriter animation
            await AddBotMessageAsync($"Hello, {_user.Name}! I am Manuel security services MSS, your professional cybersecurity assistant.");
            await AddBotMessageAsync("My mission is to help South African citizens stay safe online.\n\n" +
                                     "  • Type 'phishing' to learn about mail & message scams.\n" +
                                     "  • Type 'password' to learn about strong password habits.\n" +
                                     "  • Type 'privacy' to learn about app permissions & privacy safety.\n" +
                                     "  • Type 'quiz' to test your cybersecurity knowledge (10 questions!).\n" +
                                     "  • Type 'add task to [something]' to add a task to your Task Panel.\n" +
                                     "  • Type 'activity log' to view recent actions the bot has taken.\n" +
                                     "  • Click the 'TASKS' button in the header to view and manage your tasks.\n\n" +
                                     "How can I assist you today?");
        }

        // ── Input Placeholder & Key Controls ─────────────────────────────────────

        private void TxtChatInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtChatInput.Text == _inputPlaceholder)
            {
                TxtChatInput.Text = string.Empty;
                TxtChatInput.Foreground = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0));
            }
        }

        private void TxtChatInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtChatInput.Text))
            {
                TxtChatInput.Text = _inputPlaceholder;
                TxtChatInput.Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88));
            }
        }

        private async void TxtChatInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                e.Handled = true;
                await SubmitMessageAsync();
            }
        }

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            await SubmitMessageAsync();
        }

        private async Task SubmitMessageAsync()
        {
            if (_user == null || _engine == null || _isBotTyping) return;

            string rawText = TxtChatInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(rawText) || rawText == _inputPlaceholder) return;

            // Clear input box
            TxtChatInput.Text = string.Empty;

            // Render User Bubble (right-aligned, instant)
            AddUserMessage(rawText);

            // Trigger Bot Logic & Typewriter Reply
            _isBotTyping = true;
            BtnSend.IsEnabled = false;

            try
            {
                string reply = await _engine.GetResponseAsync(rawText);

                if (reply == "__EXIT__")
                {
                    await AddBotMessageAsync($"Goodbye, {_user.Name}! Stay vigilant and stay safe online. 🛡️");
                    await AddBotMessageAsync($"Secure session closed. Total duration: {_user.GetSessionDuration()}");
                    await Task.Delay(1500);
                    Application.Current.Shutdown();
                }
                else if (reply == "__HELP__")
                {
                    await AddBotMessageAsync("📋  MSS Bot — Full Feature Menu:\n\n" +
                                             "  📚 Cybersecurity Topics:\n" +
                                             "  • phishing — Fake emails, SMS scams & how to spot them.\n" +
                                             "  • password — Password security best practices.\n" +
                                             "  • links — Spotting malicious URLs safely.\n" +
                                             "  • tips — General device & network hygiene.\n" +
                                             "  • privacy — App permissions & data protection.\n\n" +
                                             "  🎮 Mini-Game:\n" +
                                             "  • quiz — 10-question mixed cybersecurity challenge.\n\n" +
                                             "  📋 Task Assistant (NLP):\n" +
                                             "  • 'Add task to [title]' — Add a cybersecurity task.\n" +
                                             "  • 'Remind me to [title] tomorrow/in 3 days' — Task with reminder.\n" +
                                             "  • Click TASKS button to view, complete, or delete tasks.\n\n" +
                                             "  📜 Activity Log:\n" +
                                             "  • 'Show activity log' or 'What have you done?' — View recent bot actions.\n\n" +
                                             "  • exit — Gracefully end your session.");
                }
                else
                {
                    await AddBotMessageAsync(reply);
                }
            }
            catch (Exception ex)
            {
                await AddBotMessageAsync($"[System Error: {ex.Message}]. Recovering session smoothly.");
            }
            finally
            {
                _isBotTyping = false;
                BtnSend.IsEnabled = true;
                TxtChatInput.Focus();
            }
        }

        // ── Render Chat Bubbles (User / Bot Typewriter) ───────────────────────────

        private void AddUserMessage(string message)
        {
            // Border Container
            var bubbleBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x05, 0x47, 0x35)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x07, 0x5E, 0x47)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12, 12, 2, 12),
                Padding = new Thickness(12, 8, 12, 8),
                Margin = new Thickness(60, 5, 0, 5),
                HorizontalAlignment = HorizontalAlignment.Right,
                MaxWidth = 550,
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 315,
                    ShadowDepth = 1.5,
                    Opacity = 0.3,
                    BlurRadius = 3
                }
            };

            var stack = new StackPanel();

            // Sender Prefix: User
            var prefixBlock = new TextBlock
            {
                Text = $"👤  {_user?.Name ?? "Citizen"}:",
                FontSize = 10,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(0x33, 0xFF, 0x9E)),
                Margin = new Thickness(0, 0, 0, 4)
            };
            stack.Children.Add(prefixBlock);

            // Message content
            var contentBlock = new TextBlock
            {
                Text = message,
                FontSize = 13.5,
                Foreground = Brushes.White,
                TextWrapping = TextWrapping.Wrap
            };
            stack.Children.Add(contentBlock);

            bubbleBorder.Child = stack;
            ChatPanel.Children.Add(bubbleBorder);
            ScrollToBottom();
        }

        private async Task AddBotMessageAsync(string fullMessage)
        {
            // Border Container
            var bubbleBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x1F, 0x1F, 0x20)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x2D, 0x2D, 0x30)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(12, 12, 12, 2),
                Padding = new Thickness(15, 12, 15, 12),
                Margin = new Thickness(0, 5, 60, 5),
                HorizontalAlignment = HorizontalAlignment.Left,
                MaxWidth = 580,
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                    Direction = 225,
                    ShadowDepth = 1.5,
                    Opacity = 0.3,
                    BlurRadius = 3
                }
            };

            var containerStack = new StackPanel();

            // Sender Prefix: Bot
            var prefixBlock = new TextBlock
            {
                Text = "🤖  MSS Assistant:",
                FontSize = 11,
                FontWeight = FontWeights.Bold,
                Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0xD2, 0xFF)),
                Margin = new Thickness(0, 0, 0, 8)
            };
            containerStack.Children.Add(prefixBlock);

            bubbleBorder.Child = containerStack;
            ChatPanel.Children.Add(bubbleBorder);
            ScrollToBottom();

            // Split response text into lines to apply Task 1 color coding
            string[] lines = fullMessage.Split(new[] { "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                // If line is empty, represent it as paragraph vertical spacing
                if (string.IsNullOrWhiteSpace(line))
                {
                    var spacer = new Border { Height = 6 };
                    containerStack.Children.Add(spacer);
                    continue;
                }

                // Determine context-color of text blocks
                SolidColorBrush colorBrush = Brushes.LightGray;
                if (line.TrimStart().StartsWith("•"))
                    colorBrush = Brushes.White;
                else if (line.TrimStart().StartsWith("⚠"))
                    colorBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0xE0, 0x82)); // Amber/Yellow
                else if (line.TrimStart().StartsWith("✔"))
                    colorBrush = new SolidColorBrush(Color.FromRgb(0xA5, 0xD6, 0xA7)); // Pastel Green
                else if (line.TrimStart().StartsWith("❓"))
                    colorBrush = new SolidColorBrush(Color.FromRgb(0xF4, 0x8F, 0xFB)); // Purple/Magenta

                var lineBlock = new TextBlock
                {
                    FontSize = 13.5,
                    Foreground = colorBrush,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 2)
                };

                containerStack.Children.Add(lineBlock);

                // Run smooth Typewriter character-by-character loading
                for (int charIdx = 0; charIdx < line.Length; charIdx++)
                {
                    lineBlock.Text += line[charIdx];
                    ScrollToBottom();
                    await Task.Delay(12); // Sleek typing speed
                }
            }

            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToEnd();
        }

        private void ChatScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange > 0)
            {
                ChatScrollViewer.ScrollToEnd();
            }
        }

        // ── Header Controls ──────────────────────────────────────────────────────

        private async void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            if (_isBotTyping) return;
            AddUserMessage("Help Menu");
            await SubmitSystemCommandAsync("__HELP__");
        }

        private async void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            if (_isBotTyping) return;
            AddUserMessage("Exit Session");
            await SubmitSystemCommandAsync("__EXIT__");
        }

        private async Task SubmitSystemCommandAsync(string command)
        {
            _isBotTyping = true;
            BtnSend.IsEnabled = false;

            if (command == "__HELP__")
            {
                await AddBotMessageAsync("📋  MSS Bot — Full Feature Menu:\n\n" +
                                         "  📚 Cybersecurity Topics:\n" +
                                         "  • phishing — Fake emails, SMS scams & how to spot them.\n" +
                                         "  • password — Password security best practices.\n" +
                                         "  • links — Spotting malicious URLs safely.\n" +
                                         "  • tips — General device & network hygiene.\n" +
                                         "  • privacy — App permissions & data protection.\n\n" +
                                         "  🎮 Mini-Game:\n" +
                                         "  • quiz — 10-question mixed cybersecurity challenge.\n\n" +
                                         "  📋 Task Assistant (NLP):\n" +
                                         "  • 'Add task to [title]' — Add a cybersecurity task.\n" +
                                         "  • 'Remind me to [title] tomorrow/in 3 days' — Task with reminder.\n" +
                                         "  • Click TASKS button to view, complete, or delete tasks.\n\n" +
                                         "  📜 Activity Log:\n" +
                                         "  • 'Show activity log' or 'What have you done?' — View recent bot actions.\n\n" +
                                         "  • exit — Gracefully end your session.");
            }
            else if (command == "__EXIT__")
            {
                await AddBotMessageAsync($"Goodbye, {_user?.Name ?? "Citizen"}! Stay vigilant and stay safe online. 🛡️");
                await Task.Delay(1500);
                Application.Current.Shutdown();
            }

            _isBotTyping = false;
            BtnSend.IsEnabled = true;
        }

        // ── Task Panel Controls ──────────────────────────────────────────────────

        private void BtnToggleTasks_Click(object sender, RoutedEventArgs e)
        {
            if (TasksPanel.Visibility == Visibility.Visible)
            {
                TasksPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                TasksPanel.Visibility = Visibility.Visible;
                LoadTasks();
            }
        }

        private void LoadTasks()
        {
            var tasks = DatabaseHelper.GetTasks();
            TasksItemsControl.ItemsSource = tasks;
        }

        private void BtnCompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int taskId)
            {
                DatabaseHelper.MarkTaskCompleted(taskId);
                LoadTasks();
            }
        }

        private void BtnDeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int taskId)
            {
                DatabaseHelper.DeleteTask(taskId);
                LoadTasks();
            }
        }
    }
}
