using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    [SerializeField] private GameObject Cutscene;
    [SerializeField] private float _cutsceneTimer;
     [SerializeField] private bool gameRestarting = false;

    /// <summary>
    /// Loads the next scene in the build index.
    /// </summary>
    public void NextScene()
    {
        print("NextScene");
        Cutscene.SetActive(true);
    }


    public void ShowNextScene()
    {
        print(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /// <summary>
    /// Loads the next scene
    /// </summary>
    public void LoadScene()
    {
        Debug.Log("E");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    /// <param name="context"></param>
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    /// <summary>
    /// Loads the main menu
    /// </summary>
    public void Menu()
    {
        Time.timeScale = 1;
        //Time.fixedDeltaTime = Time.deltaTime;
        // Loads the first scene, which is the main menu
        SceneManager.LoadScene(0);
    }
}