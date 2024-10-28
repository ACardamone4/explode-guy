using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject _tntOutline;
    [SerializeField] private Transform _self;
    [SerializeField] private bool _respawnTNT;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject BouncyAttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
            if (_respawnTNT)
            {
                GameObject TNTOutline = Instantiate(_tntOutline, _self.position, _self.rotation);
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion")
        {
            GameObject BouncyAttackInstance = Instantiate(_explosion, _self.position, _self.rotation);
            if (_respawnTNT)
            {
                GameObject TNTOutline = Instantiate(_tntOutline, _self.position, _self.rotation);
            }
            Destroy(gameObject);
        }
    }
}
