extern alias GameScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kalimag.Modding.BabyCoyote.Mod.Components
{
    internal class SkipCutsceneComponent : MonoBehaviour
    {

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                var video = GetComponent<GameScripts.StreamVideo>();
                if (video)
                    SceneManager.LoadScene(video.sceneName);
            }
        }

    }
}
