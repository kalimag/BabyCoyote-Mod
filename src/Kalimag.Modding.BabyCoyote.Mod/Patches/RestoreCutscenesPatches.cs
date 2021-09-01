extern alias GameScripts;
using System;
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
    internal class RestoreCutscenesPatches
    {
        private static readonly Dictionary<(string currentScene, string nextScene), string> ReplacementScenes = new Dictionary<(string currentScene, string nextScene), string>
        {
            [("TroubleInParadise", "CatsOuttheBag")] = "Meet",
            [("TheCatophre", "Boss1")] = "BossIntro",
            [("Boss1", "LevelSelection")] = "EndW1",
        };


        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameScripts.GoalFlag), "<OnTriggerEnter2D>g__LoadScene|3_0")]
        [HarmonyPatch(typeof(GameScripts.BossOne), "<ChangeHealth>g__LoadScene|15_0")]
        public static void PrefixLoadScene(ref string ___sceneName)
        {
            if (ReplacementScenes.TryGetValue((SceneManager.GetActiveScene().name, ___sceneName), out string replacementScene))
            {
                Debug.Log($"[RestoreCutscenesPatches] Replace {___sceneName} with {replacementScene}");
                ___sceneName = replacementScene;
            }
        }
    }
}
