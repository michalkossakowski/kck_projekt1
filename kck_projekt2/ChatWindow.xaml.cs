using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics.Metrics;

namespace kck_projekt2
{
    public partial class ChatWindow : Window
    {
        private readonly string _geminiApiKey;

        public ChatWindow()
        {
            InitializeComponent();
            _geminiApiKey = GetApiKeyFromUserSecrets();

            if (string.IsNullOrEmpty(_geminiApiKey))
            {
                throw new Exception("Gemini API key not found in User Secrets.");
            }
        }

        private string GetApiKeyFromUserSecrets()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<ChatWindow>();

            IConfiguration config = builder.Build();

            return config["GeminiApiKey"];
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Loading.Visibility = Visibility.Visible;
            try
            {
                string prompt = PromptTextBox.Text.Trim();
                if (string.IsNullOrEmpty(prompt))
                {
                    HintAssist.SetHelperText(PromptTextBox, "Message cannot be empty");
                    PromptTextBox.Foreground = new SolidColorBrush(Colors.Red);
                    Loading.Visibility = Visibility.Collapsed;
                    return;
                }

                SubmitButton.IsEnabled = false;
                ResponseTextBlock.Text = "Processing...";

                using (var client = new HttpClient())
                {
                    var requestBody = new
                    {
                        contents = new[]
                        {
                            new
                            {
                                parts = new[] { new { text = prompt } }
                            }
                        }
                    };
                    var json = JsonConvert.SerializeObject(requestBody);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_geminiApiKey}";
                    var response = await client.PostAsync(url, content);

                    var responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<dynamic>(responseString);

                    string generatedText = result.candidates[0].content.parts[0].text.ToString();
                    ResponseTextBlock.Text = generatedText;
                }
            }
            catch (Exception ex)
            {
                ResponseTextBlock.Text = $"Error: {ex.Message}";
            }
            finally
            {
                SubmitButton.IsEnabled = true;
            }
            Loading.Visibility = Visibility.Collapsed;
        }

        private void ChatContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PromptTextBox.Text.Length > 0)
            {
                HintAssist.SetHelperText(PromptTextBox, "Enter your question for AI");
                PromptTextBox.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxColor"];
            }
        }
    }
}