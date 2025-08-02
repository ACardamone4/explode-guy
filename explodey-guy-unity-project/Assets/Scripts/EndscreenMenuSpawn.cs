using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndscreenMenuSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _timer;
    [SerializeField] private GameObject _levelBestTime;
    [SerializeField] private TextMeshProUGUI _levelBestTimeText;
    [SerializeField] private int _levelNumber;
    //[SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Renamer _renamer;
    [SerializeField] private GameObject _renamerGameObject;
    [SerializeField] private string _nextLevelName;
    [SerializeField] private int _levelUnlockNumber;
    [SerializeField] private GameObject _endscreen;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _gameManagerGameObject;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _dataPersistanceManagerGameObject;
    [SerializeField] private DataPersistenceManager _dataPersistanceManager;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Animator _timerAnimator;


    void Start()
    {
        _timer = GameObject.Find("Timer");
        _levelBestTime = GameObject.Find("LevelBestTime");
        _levelBestTimeText = _levelBestTime.GetComponent<TextMeshProUGUI>();
        _endscreen = GameObject.Find("Endscreen");
        _continueButton = GameObject.Find("Continue");
        _player = GameObject.Find("Player");
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _timerAnimator = _timer.GetComponent<Animator>();
        _timerAnimator.Play("Corner");
        _gameManagerGameObject = GameObject.Find("GameManager");
        _gameManager = _gameManagerGameObject.GetComponent<GameManager>();
        _endscreen.SetActive(false);
        _dataPersistanceManagerGameObject = GameObject.Find("DataPersistanceManager");
        _dataPersistanceManager = _dataPersistanceManagerGameObject.GetComponent<DataPersistenceManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_dataPersistanceManager.GameData.LevelsUnlocked < _levelUnlockNumber)
            {
                _dataPersistanceManager.GameData.LevelsUnlocked = _levelUnlockNumber;
            }
            _renamer.Name = (_nextLevelName);
            _playerMovement.ResetPosition();
            _endscreen.SetActive(true);
            _timerAnimator.Play("WinScreen");
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_continueButton);
            _playerMovement.Cutscene = true;
            Time.timeScale = 0;
            if (_dataPersistanceManager.GameData.BestLevelTimes[_levelNumber] > _dataPersistanceManager.GameData.CurrentTimerTime || _dataPersistanceManager.GameData.BestLevelTimes[_levelNumber] == 0)
            {
                _dataPersistanceManager.GameData.BestLevelTimes[_levelNumber] = _dataPersistanceManager.GameData.CurrentTimerTime;
                int minutes = Mathf.FloorToInt(_dataPersistanceManager.GameData.BestLevelTimes[_levelNumber] / 60);
                int seconds = Mathf.FloorToInt(_dataPersistanceManager.GameData.BestLevelTimes[_levelNumber] % 60);
                _levelBestTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                _dataPersistanceManager.Save();
            }
        }
    }

    public void Continue()
    {
        _timerAnimator.Play("Corner");
        _playerMovement.ResetPosition();
        _playerMovement.Die();
        //_playerMovement.Pause();
        Time.timeScale = 1;
        _renamer.Name = (_nextLevelName);
        _playerMovement.Save();
        print("SpawnedNewRoom");
        _gameManager.NewLevel();
        _playerMovement.Cutscene = false;
        Destroy(_renamerGameObject);
    }



    public void Restart()
    {
        _playerMovement.RestartLevel();
        _timerAnimator.Play("Corner");
        _playerMovement.Cutscene = false;
        _endscreen.SetActive(false);
    }
}
