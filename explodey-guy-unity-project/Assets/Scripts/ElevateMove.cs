using UnityEngine;

public class ElevateMove : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed;
    [SerializeField] private Rigidbody2D _rigidbody;


    void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(0, _scrollSpeed * -1);

    }
}
