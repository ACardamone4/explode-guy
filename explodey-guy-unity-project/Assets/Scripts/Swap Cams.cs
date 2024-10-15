using UnityEngine;

public class SwapCams : MonoBehaviour
{
    [SerializeField] private GameObject _turnOffCam;
    [SerializeField] private GameObject _turnOnCam;
    [SerializeField] private bool _noSwap;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _noSwap == false)
        {
            _turnOffCam.SetActive(false);
            _turnOnCam.SetActive(true);
        }
    }

    public void SwapCamSwap()
    {
        _noSwap = true;
        _turnOffCam.SetActive(true);
        _turnOnCam.SetActive(false);
    }
}
