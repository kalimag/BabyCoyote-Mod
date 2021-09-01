extern alias GameScripts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace Kalimag.Modding.BabyCoyote.Mod.Patches
{
    [HarmonyPatch()]
    internal class InitPatches
    {

        [HarmonyPrefix, HarmonyPatch(typeof(GameScripts.Settings), "Start")]
        private static void Settings_Start()
        {
            ModController.TryGameInit();
        }

    }
}
