using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMenu : MonoBehaviour
{
    public static AudioMenu instance;

    [Header("Música de Fondo")]
    public AudioClip backgroundMusic;
    private AudioSource audioSource;

    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
        PlayBackgroundMusic();

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        if (newScene.name != "Menu")
        {
            Destroy(gameObject);
        }
    }

    private void PlayBackgroundMusic()  // <--- Solo una función
    {
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
