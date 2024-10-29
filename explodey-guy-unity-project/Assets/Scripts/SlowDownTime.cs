using UnityEngine;

public class SlowDownTime : MonoBehaviour
{
    [SerializeField] private bool slowTime;
    [SerializeField] private bool _playerDetected;
    [SerializeField] private bool _dieAfterLeaving;
    [SerializeField] private float _slowDownTimeSpeedBase;
    [SerializeField] private float _slowDownTimeSpeedLowest;
    [SerializeField] private bool _slowDownToSlowest;
    [SerializeField] private float _slowDownTimeRate;

    private void Awake()
    {
        _playerDetected = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PExplosion" || collision.gameObject.tag == "Player")
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.deltaTime;
            slowTime = true;
            _playerDetected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PExplosion" || collision.gameObject.tag == "Player")
        {
            slowTime = false;
            _playerDetected = false;
            Time.timeScale = 1;
            if (_dieAfterLeaving)
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = Time.deltaTime;
                Destroy(gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (slowTime)
        {

            Time.timeScale = 0;
            Time.fixedDeltaTime = Time.deltaTime;
            //if (_slowDownToSlowest)
            //{
            //    _slowDownTimeSpeedBase -= _slowDownTimeRate * Time.deltaTime;
            //    Time.timeScale = _slowDownTimeSpeedBase;
            //    Time.fixedDeltaTime = Time.deltaTime;
            //    if (_slowDownTimeSpeedBase <= _slowDownTimeSpeedLowest)
            //    {
            //        _slowDownTimeSpeedBase = _slowDownTimeSpeedLowest;
            //    }
            //}
            //else
            //{
            //    Time.timeScale = _slowDownTimeSpeedLowest;
            //    Time.fixedDeltaTime = Time.deltaTime;
            //}
        }
        else if (_playerDetected)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.deltaTime;
        }
        
    }
}
