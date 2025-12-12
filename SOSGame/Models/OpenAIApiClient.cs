using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SOSGame.Models
{
    public class OpenAIApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string OPENAI_API_URL = "https://api.openai.com/v1/chat/completions";
        private const string MODEL = "gpt-4o-mini";
        private const int TIMEOUT_SECONDS = 15;

        public OpenAIApiClient(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
            }

            _apiKey = apiKey;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(TIMEOUT_SECONDS)
            };
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GenerateContentAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(prompt))
            {
                throw new ArgumentException("Prompt cannot be null or empty.", nameof(prompt));
            }

            try
            {
                string requestBody = BuildRequestBody(prompt);
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(OPENAI_API_URL, content);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                    response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new OpenAIApiException("Authentication failed. Please check your API key.", 
                        (int)response.StatusCode);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    throw new OpenAIApiException("Rate limit exceeded. Please wait a moment before trying again.", 
                        (int)response.StatusCode);
                }

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new OpenAIApiException(
                        $"API request failed with status {response.StatusCode}: {errorContent}",
                        (int)response.StatusCode);
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                return ParseResponse(responseBody);
            }
            catch (TaskCanceledException ex)
            {
                throw new TaskCanceledException($"Request timed out after {TIMEOUT_SECONDS} seconds.", ex);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Network error occurred while communicating with OpenAI API.", ex);
            }
        }

        private string BuildRequestBody(string prompt)
        {
            var requestObject = new
            {
                model = MODEL,
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = "You are an expert SOS game player. Your goal is to create S-O-S sequences (3 consecutive cells: S-O-S).\n\n" +
                                 "GAME RULES:\n" +
                                 "- SOS = 3 consecutive cells spelling S-O-S (horizontal, vertical, or diagonal)\n" +
                                 "- In SIMPLE mode: First player to create ANY SOS wins immediately\n" +
                                 "- In GENERAL mode: Player with MOST SOS sequences wins when board is full\n\n" +
                                 "STRATEGY PRIORITY:\n" +
                                 "1. COMPLETE SOS: If you can complete an SOS sequence, DO IT NOW\n" +
                                 "2. BLOCK OPPONENT: If opponent can complete SOS next turn, BLOCK them\n" +
                                 "3. CREATE OPPORTUNITIES: Place letters that set up multiple future SOS possibilities\n" +
                                 "4. STRATEGIC PLACEMENT: Place near existing letters to maximize options\n\n" +
                                 "RESPONSE FORMAT:\n" +
                                 "First line: Brief reasoning (one sentence)\n" +
                                 "Second line: Row# Col# Letter\n\n" +
                                 "Example:\n" +
                                 "Completing SOS horizontally at row 0\n" +
                                 "0 2 S"
                    },
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                },
                max_tokens = 100,
                temperature = 0.3
            };

            return JsonSerializer.Serialize(requestObject);
        }

        private string ParseResponse(string jsonResponse)
        {
            try
            {
                using JsonDocument document = JsonDocument.Parse(jsonResponse);
                JsonElement root = document.RootElement;

                if (root.TryGetProperty("choices", out JsonElement choices) &&
                    choices.GetArrayLength() > 0)
                {
                    JsonElement firstChoice = choices[0];
                    
                    if (firstChoice.TryGetProperty("message", out JsonElement message) &&
                        message.TryGetProperty("content", out JsonElement content))
                    {
                        string? textValue = content.GetString();
                        if (!string.IsNullOrWhiteSpace(textValue))
                        {
                            return textValue;
                        }
                    }
                }

                throw new OpenAIApiException("Response does not contain expected text content.", 0);
            }
            catch (JsonException ex)
            {
                throw new OpenAIApiException("Failed to parse API response JSON.", 0, ex);
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }

    public class OpenAIApiException : Exception
    {
        public int StatusCode { get; }

        public OpenAIApiException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public OpenAIApiException(string message, int statusCode, Exception innerException) 
            : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
