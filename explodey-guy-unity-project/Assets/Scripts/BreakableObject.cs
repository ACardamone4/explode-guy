using System.Collections;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private float _breakingDuration;
    [SerializeField] private Transform _self;
    [SerializeField] private GameObject _rubble;

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
        Destroy(gameObject);
    }
}
