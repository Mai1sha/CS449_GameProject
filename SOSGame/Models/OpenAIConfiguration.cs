using System;
using System.IO;
using System.Text.Json;

namespace SOSGame.Models
{
    public static class OpenAIConfiguration
    {
        private const string ENV_VAR_NAME = "OPENAI_API_KEY";
        private const string CONFIG_FILE_NAME = "appsettings.json";
        private const string USER_SETTINGS_FILE = "openai_settings.json";
        private static string? _cachedApiKey;

        public static string? GetApiKey()
        {
            if (_cachedApiKey != null)
            {
                return _cachedApiKey;
            }

            string? apiKey = LoadApiKeyFromUserSettings();
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                _cachedApiKey = apiKey;
                return apiKey;
            }

            apiKey = Environment.GetEnvironmentVariable(ENV_VAR_NAME);
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                _cachedApiKey = apiKey;
                return apiKey;
            }

            apiKey = LoadApiKeyFromConfigFile();
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                _cachedApiKey = apiKey;
                return apiKey;
            }

            return null;
        }

        public static bool IsApiKeyConfigured()
        {
            string? apiKey = GetApiKey();
            return !string.IsNullOrWhiteSpace(apiKey);
        }

        public static void SetApiKey(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
            }

            _cachedApiKey = apiKey;
        }

        public static void ClearCache()
        {
            _cachedApiKey = null;
        }

        public static string GetConfigurationInstructions()
        {
            return $"To use the OpenAI opponent, you need to configure your API key:\n\n" +
                   $"Option 1: Use Settings menu\n" +
                   $"  Click 'Settings' button to enter your API key\n\n" +
                   $"Option 2: Set environment variable\n" +
                   $"  Set '{ENV_VAR_NAME}' to your OpenAI API key\n\n" +
                   $"Option 3: Create configuration file\n" +
                   $"  Create '{CONFIG_FILE_NAME}' in the application directory with:\n" +
                   $"  {{\n" +
                   $"    \"OpenAIApiKey\": \"your-api-key-here\"\n" +
                   $"  }}\n\n" +
                   $"Get your API key from: https://platform.openai.com/api-keys";
        }

        public static void SaveApiKeyToUserSettings(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("API key cannot be null or empty.", nameof(apiKey));
            }

            string userSettingsPath = GetUserSettingsPath();
            string directory = Path.GetDirectoryName(userSettingsPath)!;
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var settings = new { OpenAIApiKey = apiKey };
            string jsonContent = JsonSerializer.Serialize(settings, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            File.WriteAllText(userSettingsPath, jsonContent);
            _cachedApiKey = apiKey;
        }

        private static string GetUserSettingsPath()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolder = Path.Combine(appDataPath, "SOSGame");
            return Path.Combine(appFolder, USER_SETTINGS_FILE);
        }

        private static string? LoadApiKeyFromUserSettings()
        {
            try
            {
                string userSettingsPath = GetUserSettingsPath();
                
                if (!File.Exists(userSettingsPath))
                {
                    return null;
                }

                string jsonContent = File.ReadAllText(userSettingsPath);
                using JsonDocument document = JsonDocument.Parse(jsonContent);
                
                if (document.RootElement.TryGetProperty("OpenAIApiKey", out JsonElement apiKeyElement))
                {
                    return apiKeyElement.GetString();
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string? LoadApiKeyFromConfigFile()
        {
            try
            {
                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CONFIG_FILE_NAME);
                
                if (!File.Exists(configPath))
                {
                    return null;
                }

                string jsonContent = File.ReadAllText(configPath);
                using JsonDocument document = JsonDocument.Parse(jsonContent);
                
                if (document.RootElement.TryGetProperty("OpenAIApiKey", out JsonElement apiKeyElement))
                {
                    return apiKeyElement.GetString();
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
