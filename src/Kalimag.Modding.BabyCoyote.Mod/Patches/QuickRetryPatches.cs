extern alias GameScripts;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kalimag.Modding.BabyCoyote.Mod.Patches
{
    [HarmonyPatch]
    internal class QuickRetryPatches
    {

        [HarmonyPrefix, HarmonyPatch(typeof(GameScripts.PlayerHealth), "LoadScene")]
        public static bool PlayerHealth_LoadScene(Animator ___animator, ref IEnumerator __result)
        {
            __result = RestartRoutine(___animator);
            return false;
        }

        private static IEnumerator RestartRoutine(Animator animator)
        {
            animator.SetTrigger("FadeEnd");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
