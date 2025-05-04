using UnityEngine;

public class CollisionShake : MonoBehaviour
{
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private GameObject _camera;

    private void Awake()
    {
        Invoke("DoStuff", .2f);
    }

    public void DoStuff()
    {
        _camera = GameObject.Find("Cinema Cam");
        if (_camera != null)
        {
            _cameraShake = _camera.GetComponent<CameraShake>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && _cameraShake != null)
        {
            _cameraShake.LightShake();
        }
    }
}
