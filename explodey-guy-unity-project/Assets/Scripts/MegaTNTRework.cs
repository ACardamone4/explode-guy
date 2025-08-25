using UnityEngine;

public class MegaTNTRework : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _fuseLight;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _selfPosition;
    [SerializeField] private Collider2D _selfCollider;
    private bool canExplode;

    private void Start()
    {
        canExplode = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && canExplode)
        {
            canExplode = false;
            _selfCollider.enabled = false;
            _fuseLight.SetActive(false);
            GameObject Explosion = Instantiate(_explosion, _selfPosition.position, _selfPosition.rotation);
            _animator.Play("Refresh");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion" && canExplode)
        {
            canExplode = false;
            _selfCollider.enabled = false;
            _fuseLight.SetActive(false);
            GameObject Explosion = Instantiate(_explosion, _selfPosition.position, _selfPosition.rotation);
            _animator.Play("Refresh");
        }
    }

    public void Respawn()
    {
        canExplode = true;
        _selfCollider.enabled = true;
        _fuseLight.SetActive(true);
        _animator.Play("MegaTNT_Lit_Fuse");
    }
}
