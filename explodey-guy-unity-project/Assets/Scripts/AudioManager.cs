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
    public AudioClip chosenExplosion;

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


}
