using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kalimag.Modding.BabyCoyote.Mod
{
    internal static class ModController
    {

        private static bool _initialized;

        public static ModConfig Config { get; set; }

        public static GameObject RootObject { get; private set; }
        private static Components.UIComponent UI { get; set; }



        public static void TryGameInit()
        {
            if (_initialized)
                return;
            _initialized = true;

            try
            {
                GameInit();
            }
            catch (Exception ex)
            {
                Debug.Log("[ModController] Could not initialize mod");
                Debug.LogException(ex);
            }

        }

        private static void GameInit()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            RootObject = new GameObject("Mod Root Object");
            UnityEngine.Object.DontDestroyOnLoad(RootObject);

            if (Config.GUI)
                UI = RootObject.AddComponent<Components.UIComponent>();
            if (Config.Cheats)
                RootObject.AddComponent<Components.CheatComponent>();
            if (Config.Visuals)
                RootObject.AddComponent<Components.VisualsComponent>();
        }




        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
#if DEBUG
            Debug.Log($"[Scene] Loaded {scene.name} (#{scene.buildIndex} {scene.path})");
#endif

            if (UI)
                UI.DisplayVersion = scene.name == "MainMenu";

            if (Config.Make62Beatable)
                Make62Beatable(scene);

            if (Config.Camera)
                Components.FreeCameraComponent.CreateFreeCamera();
        }



        private static void Make62Beatable(Scene scene)
        {
            if (scene.name != "6-2")
                return;

            var problemDeathplanePos = new Vector3(142.529587f, 56.0566711f, 0f);
            bool problemDeathplaneFound = false;
            foreach (var deathplane in GameObject.FindGameObjectsWithTag("FallDector"))
            {
                if (Vector3.Distance(deathplane.transform.position, problemDeathplanePos) < 0.01)
                {
                    Debug.Log($"[ModController] Adjust position of {deathplane.name} (was x={deathplane.transform.position.x:G17} y={deathplane.transform.position.y:G17})");
                    deathplane.transform.Translate(Vector3.down * 2);
                    problemDeathplaneFound = true;
                    break;
                }
            }

            if (!problemDeathplaneFound)
                Debug.Log($"[ModController] Did not find problematic death plane in 6-2");
        }



        public static void AddNotification(string message)
        {
#if DEBUG
            Debug.Log($"[ModController] AddNotification({message})");
#endif
            if (UI)
                UI.AddNotification(message);
        }
    }
}
