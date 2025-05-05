using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    [SerializeField] private GameObject Cutscene;
    [SerializeField] private float _cutsceneTimer;
     [SerializeField] private bool gameRestarting = false;
    private DataPersistenceManager _dataPersistanceManager;
    private GameObject _dataPersistanceManagerGameobject;

    public void Awake()
    {
        _dataPersistanceManagerGameobject = GameObject.Find("DataPersistanceManager");
        if (_dataPersistanceManagerGameobject != null)
        {
            _dataPersistanceManager = _dataPersistanceManagerGameobject.GetComponent<DataPersistenceManager>();
        }
    }

    /// <summary>
    /// Loads the next scene in the build index.
    /// </summary>
    public void NextScene()
    {
        if (_dataPersistanceManager != null)
        {
            _dataPersistanceManager.Save();
        }
        print("NextScene");
        Cutscene.SetActive(true);
    }


    public void ShowNextScene()
    {
        if (_dataPersistanceManager != null)
        {
            _dataPersistanceManager.Save();
        }
        print(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /// <summary>
    /// Loads the next scene
    /// </summary>
    public void LoadScene()
    {
        if (_dataPersistanceManager != null)
        {
            _dataPersistanceManager.Save();
        }
        Debug.Log("E");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    /// <param name="context"></param>
    public void Quit()
    {
        if (_dataPersistanceManager != null)
        {
            _dataPersistanceManager.Save();
        }
        Debug.Log("Quit");
        Application.Quit();
    }

    /// <summary>
    /// Loads the main menu
    /// </summary>
    public void Menu()
    {
        if (_dataPersistanceManager != null)
        {
            _dataPersistanceManager.Save();
        }
        Time.timeScale = 1;
        //Time.fixedDeltaTime = Time.deltaTime;
        // Loads the first scene, which is the main menu
        SceneManager.LoadScene(0);
    }
}