using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject _teleportLocation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.position = _teleportLocation.transform.position;
        }
    }
}
