using System.Xml.Linq;

namespace Application.Common.Helpers
{
    public static class LocalizationHelper
    {
        private static readonly Dictionary<string, string> _messages
       = new();

        static LocalizationHelper()
        {
            var localizationPath =
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Localization",
                    "en");

            if (!Directory.Exists(localizationPath))
                return;

            var xmlFiles =
                Directory.GetFiles(
                    localizationPath,
                    "*.xml",
                    SearchOption.TopDirectoryOnly);

            foreach (var file in xmlFiles)
            {
                var document = XDocument.Load(file);

                var texts =
                    document.Descendants("text");

                foreach (var text in texts)
                {
                    var key =
                        text.Attribute("name")?.Value;

                    var value =
                        text.Value.Trim();

                    if (!string.IsNullOrWhiteSpace(key)
                        && !_messages.ContainsKey(key))
                    {
                        _messages.Add(key, value);
                    }
                }
            }
        }

        public static string L(
            string key,
            params object[] args)
        {
            if (_messages.TryGetValue(key, out var value))
            {
                return string.Format(value, args);
            }

            return key;
        }
    }
}
