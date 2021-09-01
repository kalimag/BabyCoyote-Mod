using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HarmonyLib;

namespace Kalimag.Modding.BabyCoyote.Mod
{
    internal class EntryPoint
    {

        public static void Main()
        {
            var harmonyThread = new Thread(Init) { IsBackground = true };
            harmonyThread.Start();
        }

        private static void Init()
        {
            UnityEngine.CrashReportHandler.CrashReportHandler.enableCaptureExceptions = false;

            ModConfig config;
            try
            {
                config = ModConfig.LoadConfig();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError($"[Mod] Could not read config: {ex.Message}");
                return;
            }
            ModController.Config = config;
            UnityEngine.Debug.Log($"[Mod] Mod loaded");

#if DEBUG
            HarmonyLib.Tools.Logger.ChannelFilter = HarmonyLib.Tools.Logger.LogChannel.All;
            HarmonyLib.Tools.HarmonyFileLog.Enabled = true;
#endif
            var harmony = new Harmony("net.kalimag.modding.babycoyote");

            ApplyPatches<Patches.InitPatches>();
            ApplyPatches<Patches.StreamVideoPatches>();

            if (config.RestoreCutscenes)
                ApplyPatches<Patches.RestoreCutscenesPatches>();
            if (config.QuickRetry)
                ApplyPatches<Patches.QuickRetryPatches>();
            if (config.Cheats)
                ApplyPatches<Patches.CheatPatches>();
            if (config.GUI && config.LevelReachedNotification)
                ApplyPatches<Patches.LevelReachedPatches>();

            void ApplyPatches<T>() => harmony.CreateClassProcessor(typeof(T)).Patch();
        }

    }
}
