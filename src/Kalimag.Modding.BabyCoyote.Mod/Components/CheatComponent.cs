extern alias GameScripts;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kalimag.Modding.BabyCoyote.Mod.Components
{
    internal class CheatComponent : MonoBehaviour
    {

        private readonly Dictionary<string, (Vector3 position, Vector2 velocity)> savedTeleports = new Dictionary<string, (Vector3, Vector2)>();

        public static bool Invincibility { get; set; }



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H))
                RestoreHealth();
            if (Input.GetKey(KeyCode.LeftShift))
                FlyUp();

            if (Input.GetKeyDown(KeyCode.F5))
                SaveTeleport();
            if (Input.GetKeyDown(KeyCode.F9))
                UseTeleport();

            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                if (Input.GetKeyDown(KeyCode.I))
                    ToggleInvincibility();

                if (Input.GetKeyDown(KeyCode.R))
                    RestartLevel();
                if (Input.GetKeyDown(KeyCode.F))
                    FinishLevel();
                if (Input.GetKeyDown(KeyCode.L))
                    LevelSelect();
                if (Input.GetKeyDown(KeyCode.U))
                    UnlockLevels();
            }
        }

        private void RestoreHealth()
        {
            var playerHealth = FindObjectOfType<GameScripts.PlayerHealth>();
            if (playerHealth && playerHealth.health < playerHealth.maxHealth)
            {
                ModController.AddNotification("Restored health");
                playerHealth.ChangeHealth(playerHealth.maxHealth);
            }
        }

        private void ToggleInvincibility()
        {
            Invincibility ^= true;
            ModController.AddNotification(Invincibility ? "Invincibility on" : "Invincibility off");
        }



        private void FlyUp()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                var rigidbody = player.GetComponent<Rigidbody2D>();
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 5f);
            }
        }



        private void SaveTeleport()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (!playerObj)
                return;

            var body = playerObj.GetComponent<Rigidbody2D>();
            savedTeleports[SceneManager.GetActiveScene().name] = (playerObj.transform.position, body.velocity);
            ModController.AddNotification("Teleport location saved");
        }

        private void UseTeleport()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (!playerObj)
                return;

            if (!savedTeleports.TryGetValue(SceneManager.GetActiveScene().name, out var teleport))
            {
                ModController.AddNotification("No teleport saved for this level");
                return;
            }

            playerObj.transform.position = teleport.position;
            playerObj.GetComponent<Rigidbody2D>().velocity = teleport.velocity;
        }



        private void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void LevelSelect()
        {
            SceneManager.LoadScene("LevelSelection");
        }

        private void UnlockLevels()
        {
            PlayerPrefs.SetInt("levelReached", 22);
            ModController.AddNotification("All levels unlocked");
            if (SceneManager.GetActiveScene().name == "LevelSelection")
                LevelSelect();
        }

        private void FinishLevel()
        {
            var boss = FindObjectOfType<GameScripts.BossOne>();
            if (boss)
            {
                boss.ChangeHealth(-boss.currentbossHealth);
                return;
            }

            var player = FindObjectOfType<GameScripts.PlayerHealth>();
            var goal = FindObjectOfType<GameScripts.GoalFlag>();
            if (player && goal)
            {
                var collider = player.GetComponent<Collider2D>();
                var method = AccessTools.Method(typeof(GameScripts.GoalFlag), "OnTriggerEnter2D", new[] { typeof(Collider2D) });
                method?.Invoke(goal, new[] { collider });
            }
        }


    }
}
