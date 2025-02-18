using UnityEngine;

public class AudioSetter : MonoBehaviour
{
    public bool Menu;
    public bool Tutorial;
    public bool Level;
    public AudioManager audioManager;
    public GameObject audioManagerObject;

    public void Start()
    {
        audioManagerObject = GameObject.Find("Audio Manager");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();

            if (Menu)
            {
                audioManager.Menu();
            }
            else if (Tutorial)
            {
                audioManager.Tutorial();
            }
            else if (Level)
            {
                audioManager.Level();
            }
        }
        
    }
}
