using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio Clips")]
    public AudioClip lobbyTheme;
    public AudioClip gameTheme;

    private AudioSource audioSource;
    private string currentTheme = "";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start() {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "PlayerLobby") {
            PlayTheme("Lobby");
        } else {
            PlayTheme("Game");
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName == "PlayerLobby")
        {
            PlayTheme("Lobby");
        }
        else if (sceneName == "Level1" || sceneName == "Level2" || sceneName == "Level3")
        {
            PlayTheme("Game");
        }
    }

    void PlayTheme(string themeType)
    {
        if (themeType == currentTheme) return;

        AudioClip clipToPlay = null;

        switch (themeType)
        {
            case "Lobby":
                clipToPlay = lobbyTheme;
                break;
            case "Game":
                clipToPlay = gameTheme;
                break;
        }

        if (clipToPlay != null)
        {
            audioSource.clip = clipToPlay;
            audioSource.Play();
            currentTheme = themeType;
        }
    }
}