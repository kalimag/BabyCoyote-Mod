extern alias GameScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace Kalimag.Modding.BabyCoyote.Mod.Patches
{
    [HarmonyPatch]
    internal class StreamVideoPatches
    {

        [HarmonyPrefix, HarmonyPatch(typeof(GameScripts.StreamVideo), "Start")]
        private static bool StreamVideo_Start(GameScripts.StreamVideo __instance)
        {
            ModController.TryGameInit();

            if (ModController.Config.AllowCutsceneSkipping)
                __instance.gameObject.AddComponent<Components.SkipCutsceneComponent>();

            if (ModController.Config.ImproveCutscenePlayback && !__instance.GetComponent<Components.CutscenePlaybackComponent>())
            {
                if (!__instance.GetComponent<Components.CutscenePlaybackComponent>())
                    __instance.gameObject.AddComponent<Components.CutscenePlaybackComponent>();
                return false;
            }

            return true;
        }

    }
}
