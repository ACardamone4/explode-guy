using System.Collections;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private float _breakingDuration;
    [SerializeField] private bool _respawns;
    [SerializeField] private Transform _self;
    [SerializeField] private GameObject _rubble;
    [SerializeField] private GameObject _blockOutline;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Breaking());
        }
    }

    private IEnumerator Breaking()
    {
        yield return new WaitForSeconds(_breakingDuration);
        GameObject BrokenParticles = Instantiate(_rubble, _self.position, _self.rotation);
        if (_respawns)
        {
            GameObject BlockRefresh = Instantiate(_blockOutline, _self.position, _self.rotation);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion")
        {
            GameObject BrokenParticles = Instantiate(_rubble, _self.position, _self.rotation);
            if (_respawns)
            {
                GameObject BlockRefresh = Instantiate(_blockOutline, _self.position, _self.rotation);
            }
            Destroy(gameObject);
        }
    }
}
