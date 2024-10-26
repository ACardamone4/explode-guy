using UnityEngine;

public class SlowDownTime : MonoBehaviour
{
    private bool slowTime;
    [SerializeField] private bool _dieAfterLeaving;
    [SerializeField] private float _slowDownTimeSpeed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PExplosion")
        {
            slowTime = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PExplosion")
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
            Time.timeScale = _slowDownTimeSpeed;
            Time.fixedDeltaTime = Time.deltaTime;
        }
        else
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.deltaTime;
        }
        
    }
}
