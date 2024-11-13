using UnityEngine;

public class OneWayTile : MonoBehaviour
{
    [SerializeField] private Collider2D _boxCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
       if (collision.gameObject.tag == "Player")
        {
            _boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _boxCollider.isTrigger = false;
        }
    }
}
