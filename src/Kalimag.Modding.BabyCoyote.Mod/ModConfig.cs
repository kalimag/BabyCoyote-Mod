using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using IO = System.IO;

namespace Kalimag.Modding.BabyCoyote.Mod
{
    internal class ModConfig
    {

        private static readonly HashSet<string> ImportantSettings = new HashSet<string>
        {
            nameof(Cheats), nameof(Camera), nameof(QuickRetry), nameof(Make62Beatable), nameof(Camera),
            nameof(AllowCutsceneSkipping), nameof(ImproveCutscenePlayback), nameof(RestoreCutscenes)
        };

        public bool LevelReachedNotification { get; private set; }
        public float NotificationDisplayTime { get; private set; }

        public bool Camera { get; set; }

        public bool Visuals { get; private set; }

        public bool Cheats { get; private set; }

        public bool QuickRetry { get; private set; }

        public bool Make62Beatable { get; private set; }

        public bool AllowCutsceneSkipping { get; private set; }
        public bool ImproveCutscenePlayback { get; private set; }
        public bool RestoreCutscenes { get; private set; }



        public string ActiveSettings { get; }

        private static readonly Regex KeyValueRegex = new Regex(@"^\s*(?<key>[^=#\[][^=]+?)\s*=\s*(?<value>.*?)\s*$");

        private ModConfig(string path)
        {
            var changedProps = new Dictionary<PropertyInfo, object>();

            using (var reader = new IO.StreamReader(path, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var match = KeyValueRegex.Match(line);
                    if (!match.Success)
                        continue;

                    string key = match.Groups["key"].Value;
                    string value = match.Groups["value"].Value;

                    var prop = typeof(ModConfig).GetProperty(key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                    if (prop != null && prop.CanWrite)
                    {
                        var convertedValue = Convert.ChangeType(value, prop.PropertyType, CultureInfo.InvariantCulture);
                        if (!Equals(prop.GetValue(this), convertedValue))
                            changedProps[prop] = convertedValue;
                        prop.SetValue(this, Convert.ChangeType(value, prop.PropertyType, CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        Debug.Log($"[ModConfig] Unknown config key \"{key}\"");
                    }
                }
            }

            ActiveSettings = String.Join(" ", changedProps
                .Where(kvp => ImportantSettings.Contains(kvp.Key.Name))
                .OrderBy(kvp => kvp.Key.Name)
                .Select(kvp => true.Equals(kvp.Value) ? kvp.Key.Name : $"{kvp.Key.Name}={kvp.Value?.ToString()}")
            );
        }

        public static ModConfig LoadConfig()
        {
            var path = IO.Path.Combine(IO.Path.GetDirectoryName(Environment.GetEnvironmentVariable("DOORSTOP_PROCESS_PATH")), "mod.config");
            return new ModConfig(path);
        }
    }
}
