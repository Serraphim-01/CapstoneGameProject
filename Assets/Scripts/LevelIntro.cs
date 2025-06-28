using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelIntro : MonoBehaviour
{
    public GameObject levelIntroUI; // Assign the Canvas
    public float freezeDuration = 2f;
    private GameObject player;
    private Camera playerCamera;
    private MonoBehaviour[] playerScripts;

    void Start()
    {
        // Find player and camera
        player = GameObject.FindGameObjectWithTag("Player");
        playerCamera = Camera.main;

        if (player != null)
        {
            // Disable all movement scripts (assumes movement scripts are MonoBehaviours)
            playerScripts = player.GetComponents<MonoBehaviour>();
            foreach (var script in playerScripts)
            {
                script.enabled = false;
            }
        }

        // Disable camera look if it uses a script
        if (playerCamera != null)
        {
            MonoBehaviour[] camScripts = playerCamera.GetComponents<MonoBehaviour>();
            foreach (var script in camScripts)
            {
                script.enabled = false;
            }
        }

        // Lock the cursor completely
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(HandleIntro());
    }

    IEnumerator HandleIntro()
    {
        if (levelIntroUI != null)
            levelIntroUI.SetActive(true);

        yield return new WaitForSecondsRealtime(freezeDuration);

        // Hide UI
        if (levelIntroUI != null)
            levelIntroUI.SetActive(false);

        // Enable player movement
        if (playerScripts != null)
        {
            foreach (var script in playerScripts)
            {
                script.enabled = true;
            }
        }

        // Enable camera scripts
        if (playerCamera != null)
        {
            MonoBehaviour[] camScripts = playerCamera.GetComponents<MonoBehaviour>();
            foreach (var script in camScripts)
            {
                script.enabled = true;
            }
        }

        // Lock cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
