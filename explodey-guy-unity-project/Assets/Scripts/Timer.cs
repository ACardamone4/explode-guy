using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerText;
    private GameObject _timerTextObject;
    private float elapsedTime;
    public bool Active;

    private static Timer instance;
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

    public void Reset()
    {
        elapsedTime = 0;
    }

    private void FixedUpdate()
    {
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

        }
    }
}
