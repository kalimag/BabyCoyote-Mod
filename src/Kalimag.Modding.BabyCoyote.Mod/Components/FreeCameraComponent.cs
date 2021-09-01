using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;

namespace Kalimag.Modding.BabyCoyote.Mod.Components
{
    internal class FreeCameraComponent : MonoBehaviour
    {

        private const float MoveSpeed = 20f;
        private const float OrthographicZoomMult = 1f;
        private const float PerspectiveZoomMult = 25f;

        private CinemachineVirtualCamera mainCamera;
        private CinemachineVirtualCamera freeCamera;
        private GameObject followTarget;
        private float moveSpeedMult = 1;
        private bool cameraDeathplaneDisabled;

        private void Start()
        {
            // clone existing camera
            mainCamera = FindObjectOfType<CinemachineVirtualCamera>();

            var freeCameraObj = Instantiate(mainCamera.VirtualCameraGameObject);

            freeCamera = freeCameraObj.GetComponent<CinemachineVirtualCamera>();
            freeCamera.Priority += 100;
            freeCamera.enabled = false;

            var confiner = freeCameraObj.GetComponent<CinemachineConfiner>();
            if (confiner)
                confiner.enabled = false;

            followTarget = new GameObject();
            freeCamera.m_Follow = followTarget.transform;

            ResetFreeCamera();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.KeypadPlus))
                Zoom(-1);
            if (Input.GetKey(KeyCode.KeypadMinus))
                Zoom(+1);
            if (Input.GetKey(KeyCode.Keypad8))
                MoveFreeCamera(Vector3.up);
            if (Input.GetKey(KeyCode.Keypad6))
                MoveFreeCamera(Vector3.right);
            if (Input.GetKey(KeyCode.Keypad2))
                MoveFreeCamera(Vector3.down);
            if (Input.GetKey(KeyCode.Keypad4))
                MoveFreeCamera(Vector3.left);
            if (Input.GetKeyDown(KeyCode.Keypad0))
                ResetFreeCamera();
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
                ToggleFreeCamera();
        }



        private void ToggleFreeCamera()
        {
            freeCamera.enabled ^= true;
        }

        private void EnableFreeCamera()
        {
            if (!cameraDeathplaneDisabled)
                DisableCameraDeathplane();

            freeCamera.enabled = true;
        }

        private void Zoom(float direction)
        {
            EnableFreeCamera();

            if (freeCamera.m_Lens.Orthographic)
            {
                var mult = 1 + OrthographicZoomMult * Time.unscaledDeltaTime;
                if (direction < 0)
                    mult = 1 / mult;
                freeCamera.m_Lens.OrthographicSize = Mathf.Max(freeCamera.m_Lens.OrthographicSize * mult, 1);
                moveSpeedMult = freeCamera.m_Lens.OrthographicSize / mainCamera.m_Lens.OrthographicSize;
            }
            else
            {
                freeCamera.m_Lens.FieldOfView = Mathf.Clamp(freeCamera.m_Lens.FieldOfView + PerspectiveZoomMult * Time.unscaledDeltaTime * direction, 1, 180);
                moveSpeedMult = freeCamera.m_Lens.FieldOfView / mainCamera.m_Lens.FieldOfView;
            }

            //Debug.Log($"[FreeCamera] Orthographic={freeCamera.m_Lens.Orthographic} OrthographicSize={freeCamera.m_Lens.OrthographicSize} FOV={freeCamera.m_Lens.FieldOfView}");
        }

        private void MoveFreeCamera(Vector3 direction)
        {
            EnableFreeCamera();
            followTarget.transform.SetParent(null, worldPositionStays: true);
            followTarget.transform.Translate(direction * MoveSpeed * Time.unscaledDeltaTime * moveSpeedMult, Space.Self);
        }

        private void ResetFreeCamera()
        {
            if (freeCamera.m_Lens.Orthographic)
                freeCamera.m_Lens.OrthographicSize = mainCamera.m_Lens.OrthographicSize;
            else
                freeCamera.m_Lens.FieldOfView = mainCamera.m_Lens.FieldOfView;

            moveSpeedMult = 1;

            followTarget.transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform, worldPositionStays: false);
            followTarget.transform.localPosition = Vector3.zero;
        }

        private void DisableCameraDeathplane()
        {
            var deathplanes = GameObject.FindGameObjectsWithTag("FallDector");
            bool anyFound = false;
            foreach (var deathplane in deathplanes)
            {
                if (deathplane.transform.IsChildOf(Camera.main.transform))
                {
                    deathplane.SetActive(false);
                    anyFound = true;
                }
            }

            if (anyFound)
                ModController.AddNotification("Disabled deathplane attached to camera");

            cameraDeathplaneDisabled = true;
        }


        public static void CreateFreeCamera()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var camera = FindObjectOfType<CinemachineVirtualCamera>();
            if (!player || !camera)
                return;

            var freecamObj = new GameObject("FreeCamera", typeof(FreeCameraComponent));
        }

    }
}
