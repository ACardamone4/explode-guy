using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    [SerializeField] private GameObject Cutscene;
    [SerializeField] private float _cutsceneTimer;
     [SerializeField] private bool gameRestarting = false;
    private DataPersistenceManager _dataPersistanceManager;
    private GameObject _dataPersistanceManagerGameobject;
    [SerializeField] private string _levelName;
    [SerializeField] private bool _backToStart;
    [SerializeField] private bool _hasTimer;
    [SerializeField] private TextMeshProUGUI _levelBestTimeText;
    [SerializeField] private int _levelNumber;

    public void Awake()
    {
        _dataPersistanceManagerGameobject = GameObject.Find("DataPersistanceManager");
        if (_dataPersistanceManagerGameobject != null)
        {
            _dataPersistanceManager = _dataPersistanceManagerGameobject.GetComponent<DataPersistenceManager>();
        }


        if (_hasTimer)
        {
            int minutes = Mathf.FloorToInt(_dataPersistanceManager.GameData.BestLevelTimes[_levelNumber] / 60);
            int seconds = Mathf.FloorToInt(_dataPersistanceManager.GameData.BestLevelTimes[_levelNumber] % 60);
            _levelBestTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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
                if (_dataPersistanceManager.GameData.RoomName != _levelName || _backToStart)
                {
                    _dataPersistanceManager.GameData.PlayerPosX = 0;
                    _dataPersistanceManager.GameData.PlayerPosY = 0;
                }
                _dataPersistanceManager.GameData.RoomName = _levelName;
                _dataPersistanceManager.Save();
            }
            Debug.Log("E");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReenterScene()
    {
        if (_dataPersistanceManager != null)
        {
            _dataPersistanceManager.Save();
            _levelName = _dataPersistanceManager.GameData.RoomName;
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
        //_dataPersistanceManager.GameData.PlayerPosX = 0;
        //_dataPersistanceManager.GameData.PlayerPosY = 0;
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