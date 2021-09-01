extern alias GameScripts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Kalimag.Modding.BabyCoyote.Mod.Components
{
    internal class CutscenePlaybackComponent : MonoBehaviour
    {

        private GameScripts.StreamVideo streamVideo;

        private void Awake()
        {
            streamVideo = GetComponent<GameScripts.StreamVideo>();
        }

        private void Start()
        {
            streamVideo.videoPlayer.prepareCompleted += OnPrepareCompleted;
            streamVideo.videoPlayer.loopPointReached += OnLoopPointReached;
            streamVideo.videoPlayer.Prepare();
        }

        private void OnPrepareCompleted(VideoPlayer source)
        {
            streamVideo.rawImage.texture = streamVideo.videoPlayer.texture;
            streamVideo.videoPlayer.Play();
            streamVideo.audioSource.Play();
        }

        private void OnLoopPointReached(VideoPlayer source)
        {
            SceneManager.LoadScene(streamVideo.sceneName);
        }

    }
}
