using System.Xml.Linq;

namespace Application.Common.Helpers
{
    public static class LocalizationHelper
    {
        private static readonly Dictionary<string, string> _messages = new(StringComparer.OrdinalIgnoreCase);

        static LocalizationHelper()
        {
            LoadMessages();
        }

        public static string L(string key, params object[] args)
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

        private static void LoadMessages()
        {
            var localizationPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Localization",
                "en");

            if (!Directory.Exists(localizationPath))
            {
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
        }
    }
}
