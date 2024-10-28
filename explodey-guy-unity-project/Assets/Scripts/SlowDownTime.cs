using UnityEngine;

public class SlowDownTime : MonoBehaviour
{
    [SerializeField] private bool slowTime;
    [SerializeField] private bool _dieAfterLeaving;
    [SerializeField] private float _slowDownTimeSpeedBase;
    [SerializeField] private float _slowDownTimeSpeedLowest;
    [SerializeField] private bool _slowDownToSlowest;
    [SerializeField] private float _slowDownTimeRate;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PExplosion" || collision.gameObject.tag == "Player")
        {
            slowTime = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PExplosion" || collision.gameObject.tag == "Player")
        {
            slowTime = false;
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
            if (_slowDownToSlowest)
            {
                _slowDownTimeSpeedBase -= _slowDownTimeRate * Time.deltaTime;
                Time.timeScale = _slowDownTimeSpeedBase;
                //Time.fixedDeltaTime = Time.deltaTime;
                if (_slowDownTimeSpeedBase <= _slowDownTimeSpeedLowest)
                {
                    _slowDownTimeSpeedBase = _slowDownTimeSpeedLowest;
                }
            }
            else
            {
                Time.timeScale = _slowDownTimeSpeedLowest;
                Time.fixedDeltaTime = Time.deltaTime;
            }
        }
        else
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.deltaTime;
        }
        
    }
}
