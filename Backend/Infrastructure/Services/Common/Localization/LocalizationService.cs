using Application.Common.Interfaces.Localization;
using System.Xml.Linq;
using System.Collections.Concurrent;
using System.Globalization;
using System.IO;
using System;

namespace Infrastructure.Services.Common.Localization
{
    public class LocalizationService : ILocalizationService
    {
        // Cache: culture -> (key -> value)
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _cache = new(StringComparer.OrdinalIgnoreCase);

        public string L(string key, params object[] args)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var resources = GetResourcesForCulture(culture);

            if (!resources.TryGetValue(key, out var value))
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

        public IDictionary<string, string> GetResources(string? culture = null)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            }
            var resources = GetResourcesForCulture(culture);
            return new Dictionary<string, string>(resources);
        }

        private ConcurrentDictionary<string, string> GetResourcesForCulture(string culture)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                culture = "en";
            }

            return _cache.GetOrAdd(culture, c =>
            {
                var messages = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var localizationPath = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "Localization",
                    c);

                if (!Directory.Exists(localizationPath))
                {
                    // Fallback to "en" if the requested culture folder does not exist
                    if (!c.Equals("en", StringComparison.OrdinalIgnoreCase))
                    {
                        return GetResourcesForCulture("en");
                    }
                    return messages;
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
                        messages.TryAdd(key, value);
                    }
                }

                return messages;
            });
        }
    }
}
