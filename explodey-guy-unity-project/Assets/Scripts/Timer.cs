using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private GameObject _timerTextObject;
    private DataPersistenceManager _dataPersistanceManager;
    private GameObject _dataPersistanceManagerGameobject;
    public float elapsedTime;
    public bool Active;
    public bool _performedInvoke;
    public bool _recievedData;

    private static Timer instance;
    private void Awake()
    {
        _recievedData = false;
        _performedInvoke = false;

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

    public void StartTimer()
    {
        elapsedTime = _dataPersistanceManager.GameData.CurrentTimerTime;
        _recievedData = true;
    }

    public void Reset()
    {
        elapsedTime = 0;
    }

    private void FixedUpdate()
    {
        _dataPersistanceManagerGameobject = GameObject.Find("DataPersistanceManager");
        if (_dataPersistanceManagerGameobject != null)
        {
            _dataPersistanceManager = _dataPersistanceManagerGameobject.GetComponent<DataPersistenceManager>();
            //print(_dataPersistanceManager.GameData.CurrentTimerTime);
            if (!_performedInvoke)
            {
                _performedInvoke = true;
                Invoke("StartTimer", .3f);
            }
            
        }
        if (_recievedData)
        {
            //_dataPersistanceManager.GameData.CurrentTimerTime = elapsedTime;
            _timerTextObject = GameObject.Find("Timer");
            if (_timerTextObject != null)
            {
                _timerText = _timerTextObject.GetComponent<TextMeshProUGUI>();


                if (Active)
                {
                    elapsedTime += Time.deltaTime;
                }
                int minutes = Mathf.FloorToInt(elapsedTime / 60);
                int seconds = Mathf.FloorToInt(elapsedTime % 60);

                _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                _dataPersistanceManager.GameData.CurrentTimerTime = elapsedTime;
                //print(_dataPersistanceManager.GameData.CurrentTimerTime);
            }
        }
    }
}
