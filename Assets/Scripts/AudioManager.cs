using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; 
    
    [Header("Música de Fondo")]
    public AudioClip backgroundMusic;
    private AudioSource audioSource;
    
    void Awake()
    {
        if (instance == null)
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
    }
    
    void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && audioSource != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true; 
            audioSource.Play();
        }
    }
}