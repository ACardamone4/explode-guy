using UnityEngine;

public class CheckAnimController : MonoBehaviour
{
    [SerializeField] private Animator _checkpointAnimator;
    public AudioManager audioManager;
    public GameObject audioManagerObject;
    private bool activated;

    private void Start()
    {
        audioManagerObject = GameObject.Find("Audio Manager");
        if (audioManagerObject != null)
        {
            audioManager = audioManagerObject.GetComponent<AudioManager>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !activated)
        {
            activated = true;
            _checkpointAnimator.SetBool("Activate", true);
            audioManager.CheckpointAudio();
        }
    }
}
