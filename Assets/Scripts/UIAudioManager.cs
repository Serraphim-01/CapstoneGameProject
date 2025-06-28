using UnityEngine;

public class UIAudioManager : MonoBehaviour
{
    public static UIAudioManager Instance;

    public AudioSource clickAudioSource;
    public AudioSource closeAudioSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayClickSound()
    {
        if (clickAudioSource != null)
            clickAudioSource.Play();
    }

    public void PlayCloseSound()
    {
        if (closeAudioSource != null)
            closeAudioSource.Play();
    }
}
