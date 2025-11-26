using UnityEngine;

public class AudioManagerBoss : MonoBehaviour
{
    public static AudioManagerBoss instance; 
    
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