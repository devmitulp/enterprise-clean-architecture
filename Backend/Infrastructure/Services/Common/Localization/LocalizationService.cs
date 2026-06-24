using Application.Common.Interfaces.Localization;
using System.Xml.Linq;

namespace Infrastructure.Services.Common.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly Dictionary<string, string> _messages = new(StringComparer.OrdinalIgnoreCase);
        private readonly object _lock = new();
        private bool _isLoaded;

        public LocalizationService()
        {
            LoadMessages();
        }

        public string L(string key, params object[] args)
        {
            if (!_messages.TryGetValue(key, out var value))
            {
                return key;
            }

            try
            {
                return args.Length > 0
                    ? string.Format(value, args)
                    : value;
            }
            catch (FormatException)
            {
                return value;
            }
        }

        private void LoadMessages()
        {
            if (_isLoaded)
            {
                return;
            }

            lock (_lock)
            {
                if (_isLoaded)
                {
                    return;
                }

                var localizationPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Localization",
                    "en");

                if (!Directory.Exists(localizationPath))
                {
                    _isLoaded = true;
                    return;
                }

                var xmlFiles = Directory.GetFiles(
                    localizationPath,
                    "*.xml",
                    SearchOption.TopDirectoryOnly);

                foreach (var file in xmlFiles)
                {
                    var document = XDocument.Load(file);

                    foreach (var text in document.Descendants("text"))
                    {
                        var key = text.Attribute("name")?.Value;

                        if (string.IsNullOrWhiteSpace(key))
                        {
                            continue;
                        }

                        var value = text.Value.Trim();
                        _messages.TryAdd(key, value);
                    }
                }

                _isLoaded = true;
            }
        }
    }
}
