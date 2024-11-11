using UnityEngine;

public class PlayAnimPlayer : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _animator.SetBool("Interact", true);
        }
    }
}
