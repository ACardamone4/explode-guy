using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public AudioSource audioSource;
    public GameObject LevelMusicSFX;
    public GameObject MenuMusicSFX;
    public GameObject TutorialMusicSFX;
    public AudioClip[] Explosions;
    public int TotalExplosionTypes;
    private int targetedExplosion;
    private AudioClip chosenExplosion;
    public AudioClip LandSFX;
    public AudioClip PauseExplosionSFX;
    public AudioClip DeathSFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Menu()
    {
        LevelMusicSFX.SetActive(false);
        MenuMusicSFX.SetActive(true);
        TutorialMusicSFX.SetActive(false);
    }
    
    public void Tutorial()
    {
        LevelMusicSFX.SetActive(false);
        MenuMusicSFX.SetActive(false);
        TutorialMusicSFX.SetActive(true);
    }
    
    public void Level()
    {
        LevelMusicSFX.SetActive(true);
        MenuMusicSFX.SetActive(false);
        TutorialMusicSFX.SetActive(false);
    }

    public void Explosion()
    {
        targetedExplosion = Random.Range(0, TotalExplosionTypes);
        chosenExplosion = Explosions[targetedExplosion];
        if (chosenExplosion != null)
        { 
            audioSource.PlayOneShot(chosenExplosion);
        }
    }

    public void Land()
    {
        audioSource.PlayOneShot(LandSFX);
    }

    public void PauseExplosion()
    {
        audioSource.PlayOneShot(PauseExplosionSFX);
    }

    public void Death()
    {
        Explosion();
        audioSource.PlayOneShot(DeathSFX);
    }
}
