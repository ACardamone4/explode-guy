using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    [SerializeField] private CheckpointManager _checkpointManager;
    private void Start()
    {
        _checkpointManager = FindObjectOfType<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _checkpointManager.NewLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void NewLevel()
    {
        _checkpointManager.NewLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Menu()
    {
        _checkpointManager.NewLevel();
        Time.timeScale = 1;
        //Time.fixedDeltaTime = Time.deltaTime;
        // Loads the first scene, which is the main menu
        SceneManager.LoadScene(0);
    }
}