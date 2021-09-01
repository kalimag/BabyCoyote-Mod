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
    internal class CheatPatches
    {

        [HarmonyPrefix, HarmonyPatch(typeof(GameScripts.PlayerHealth), nameof(GameScripts.PlayerHealth.ChangeHealth))]
        private static bool PlayerHealth_ChangeHealth(int amount)
        {
            if (amount < 0 && Components.CheatComponent.Invincibility)
            {
                ModController.AddNotification($"Took {-amount} damage");
                return false;
            }
            else
            {
                return true;
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(GameScripts.PlayerHealth), "OnTriggerEnter2D")]
        private static bool PlayerHealth_OnTriggerEnter2D(Collider2D other)
        {
            if (Components.CheatComponent.Invincibility && other.tag == "FallDector")
            {
                ModController.AddNotification($"Entered death plane");
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}
