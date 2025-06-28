using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    private List<EnemyHealth> aliveEnemies = new();
    private int commonCount, eliteCount, bossCount;

    [Header("UI References")]
    public UIDocument gameOverUIDocument;

    private VisualElement gameOverRoot;
    private Label gameOverLabel;
    private Label youWinLabel;
    private UnityEngine.UIElements.Button restartButton;
    private UnityEngine.UIElements.Button nextLevelButton;
    private UnityEngine.UIElements.Button returnToLobbyButton;



    [Header("Canvas Enemy Count UI")]
    public Text totalEnemiesText;
    public Text commonEnemiesText;
    public Text eliteEnemiesText;
    public Text bossEnemiesText;
    private bool hasWon;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (gameOverUIDocument != null)
        {
            gameOverRoot = gameOverUIDocument.rootVisualElement;
            gameOverLabel = gameOverRoot.Q<Label>("GameOver");
            youWinLabel = gameOverRoot.Q<Label>("YouWin");
            restartButton = gameOverRoot.Q<UnityEngine.UIElements.Button>("Restart");
            nextLevelButton = gameOverRoot.Q<UnityEngine.UIElements.Button>("NextLevel");
            returnToLobbyButton = gameOverRoot.Q<UnityEngine.UIElements.Button>("Return");

            gameOverRoot.style.display = DisplayStyle.None;
        }

        if (restartButton != null)
        {
            restartButton.clicked -= RestartLevel;
            restartButton.clicked += RestartLevel;
        }

        if (returnToLobbyButton != null)
        {
            returnToLobbyButton.clicked -= ReturnToLobby;
            returnToLobbyButton.clicked += ReturnToLobby;
        }

        // Existing monster registration
        EnemyHealth[] existingEnemies = FindObjectsOfType<EnemyHealth>(true);
        foreach (var enemy in existingEnemies)
        {
            RegisterEnemy(enemy);
        }
    }

    public void RegisterEnemy(EnemyHealth enemy)
    {
        if (enemy == null || aliveEnemies.Contains(enemy)) return;

        aliveEnemies.Add(enemy);

        switch (enemy.enemyType)
        {
            case EnemyHealth.Type.Common: commonCount++; break;
            case EnemyHealth.Type.Elite: eliteCount++; break;
            case EnemyHealth.Type.Boss: bossCount++; break;
        }

        UpdateEnemyCountUI();
    }

    public void UnregisterEnemy(EnemyHealth enemy)
    {
        if (!aliveEnemies.Contains(enemy)) return;

        aliveEnemies.Remove(enemy);

        switch (enemy.enemyType)
        {
            case EnemyHealth.Type.Common: commonCount--; break;
            case EnemyHealth.Type.Elite: eliteCount--; break;
            case EnemyHealth.Type.Boss: bossCount--; break;
        }

        UpdateEnemyCountUI();

        if (aliveEnemies.Count == 0)
        {
            TriggerWin();
        }

    }

    private void UpdateEnemyCountUI()
    {
        if (totalEnemiesText != null)
            totalEnemiesText.text = $"Total: {aliveEnemies.Count}";

        if (commonEnemiesText != null)
            commonEnemiesText.text = $"Common: {commonCount}";

        if (eliteEnemiesText != null)
            eliteEnemiesText.text = $"Elite: {eliteCount}";

        if (bossEnemiesText != null)
            bossEnemiesText.text = $"Boss: {bossCount}";

        Debug.Log($"[Enemy UI] Total={aliveEnemies.Count}, C={commonCount}, E={eliteCount}, B={bossCount}");
    }


    private void TriggerWin()
    {
        Debug.Log("YOU WIN!");
        hasWon = true;
        Time.timeScale = 0f;

        if (gameOverRoot != null)
        {
            gameOverRoot.style.display = DisplayStyle.Flex;

            if (gameOverLabel != null) gameOverLabel.style.display = DisplayStyle.None;
            if (restartButton != null) restartButton.style.display = DisplayStyle.None;
            if (youWinLabel != null) youWinLabel.style.display = DisplayStyle.Flex;
            // Determine if "Next Level" should be shown
            if (nextLevelButton != null)
            {
                string currentScene = SceneManager.GetActiveScene().name;
                if (currentScene == "Level3")
                {
                    Debug.Log("🛑 NextLevel button disabled (final level).");
                    nextLevelButton.style.display = DisplayStyle.None;
                }
                else
                {
                    nextLevelButton.style.display = DisplayStyle.Flex;
                    nextLevelButton.clicked -= LoadNextLevel;
                    nextLevelButton.clicked += LoadNextLevel;
                }
            }
        }
    }

    void LoadNextLevel()
    {
        UIAudioManager.Instance?.PlayClickSound();

        Time.timeScale = 1f;

        string current = SceneManager.GetActiveScene().name;

        if (current == "Level1")
            SceneManager.LoadScene("Level2");
        else if (current == "Level2")
            SceneManager.LoadScene("Level3");
        else if (current == "Level3")
            SceneManager.LoadScene("PlayerLobby"); // or "VictoryScreen" if you want a final screen
        else
            Debug.LogWarning("No next level defined for: " + current);
    }

    void RestartLevel()
    {
        UIAudioManager.Instance?.PlayClickSound(); 

        Debug.Log("🔁 Restarting current level...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ReturnToLobby()
    {
        UIAudioManager.Instance?.PlayClickSound();
        
        Debug.Log("🏠 Returning to Lobby...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("PlayerLobby");
    }
}
