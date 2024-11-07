using UnityEngine;

public class BGBrightness : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _lightLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _animator.Play("Mines_BG_" + _lightLevel);
        }
    }
}
