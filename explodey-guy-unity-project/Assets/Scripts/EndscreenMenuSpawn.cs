using UnityEngine;
using UnityEngine.EventSystems;

public class EndscreenMenuSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _timer;
    //[SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Renamer _renamer;
    [SerializeField] private GameObject _renamerGameObject;
    [SerializeField] private string _nextLevelName;
    [SerializeField] private GameObject _endscreen;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _gameManagerGameObject;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Animator _timerAnimator;


    void Start()
    {
        _timer = GameObject.Find("Timer");
        _endscreen = GameObject.Find("Endscreen");
        _continueButton = GameObject.Find("Continue");
        _player = GameObject.Find("Player");
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _timerAnimator = _timer.GetComponent<Animator>();
        _timerAnimator.Play("Corner");
        _gameManagerGameObject = GameObject.Find("GameManager");
        _gameManager = _gameManagerGameObject.GetComponent<GameManager>();
        _endscreen.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _renamer.Name = (_nextLevelName);
            _endscreen.SetActive(true);
            _timerAnimator.Play("WinScreen");
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_continueButton);
            _playerMovement.Cutscene = true;
            Time.timeScale = 0;
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
}
