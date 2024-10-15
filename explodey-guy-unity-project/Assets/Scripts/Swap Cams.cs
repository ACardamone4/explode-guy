using UnityEngine;

public class SwapCams : MonoBehaviour
{
    [SerializeField] private GameObject _turnOffCam;
    [SerializeField] private GameObject _turnOnCam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _turnOffCam.SetActive(false);
            _turnOnCam.SetActive(true);
        }
    }
}
