using UnityEngine;

public class CollisionShake : MonoBehaviour
{
    [SerializeField] private CameraShake _cameraShake;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            _cameraShake.LightShake();
        }
    }
}
