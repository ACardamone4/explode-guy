using Cinemachine;
using UnityEngine;

public class CameraShakeDetect : MonoBehaviour
{

    [SerializeField] private GameObject _camera;
    [SerializeField] private CameraShake _cameraShake;

    // Update is called once per frame
    void Awake()
    {
        _camera = GameObject.FindGameObjectWithTag("Cam");
        if (_camera != null)
        {
            _cameraShake = _camera.GetComponent<CameraShake>();
            _cameraShake.Shake();
        } 
        else
        {
            print("No Cam?");
        }
    }

    private void OnEnable()
    {
        if (_camera != null)
        {
            _cameraShake.Shake();
        }
        else
        {
            print("No Cam?");
        }
    }
}
