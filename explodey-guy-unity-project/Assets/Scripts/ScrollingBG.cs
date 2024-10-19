using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollingBG : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private GameObject _spawnLocation;
    [SerializeField] private Rigidbody2D _rigidbody;


    void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_scrollSpeed * -1, 0);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Reset")
        {
            transform.position = _spawnLocation.transform.position;
        }
    }

}