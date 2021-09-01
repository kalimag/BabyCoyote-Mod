using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kalimag.Modding.BabyCoyote.Mod.Components
{
    internal class VisualsComponent : MonoBehaviour
    {

        private const int DeathplaneVisualOrder = 100;
        private static readonly Color DeathplaneVisualColor = new Color(1f, 0f, 0f, 0.75f);

        private bool showDeathplanes;
        private readonly List<GameObject> deathplaneVisuals = new List<GameObject>();


        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.V) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                ToggleDeathplanes();
        }



        public void ToggleDeathplanes()
        {
            showDeathplanes ^= true;
            ModController.AddNotification(showDeathplanes ? "Death planes visible" : "Death planes hidden");

            if (showDeathplanes)
                VisualizeDeathplanes();
            else
                ClearDeathplanes();
        }

        private void VisualizeDeathplanes()
        {
            var deathplanes = GameObject.FindGameObjectsWithTag("FallDector");
            foreach (var deathplane in deathplanes)
                Visuals.VisualHelper.AddVisuals(deathplane, true, DeathplaneVisualColor, DeathplaneVisualOrder, deathplaneVisuals);
        }

        private void ClearDeathplanes()
        {
            foreach (var visual in deathplaneVisuals)
                if (visual)
                    Destroy(visual);
            deathplaneVisuals.Clear();
        }



        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            deathplaneVisuals.Clear();
            if (showDeathplanes)
                VisualizeDeathplanes();
        }
    }
}
