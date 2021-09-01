extern alias GameScripts;
using HarmonyLib;
using UnityEngine;

namespace Kalimag.Modding.BabyCoyote.Mod.Patches
{
    [HarmonyPatch()]
    internal class LevelReachedPatches
    {

        [HarmonyPrefix, HarmonyPatch(typeof(PlayerPrefs), nameof(PlayerPrefs.SetInt))]
        private static void PlayerPrefs_SetInt(string key, int value)
        {
            Debug.Log($"[PlayerPrefs] {key} = {value}");

            if (key == "levelReached")
            {
                var prevLevel = PlayerPrefs.GetInt("levelReached", 1);
                if (prevLevel != value)
                    ModController.AddNotification($"Level reached: {value}");
                else
                    ModController.AddNotification($"Level reached: {value} (unchanged)");
            }
        }

    }
}
