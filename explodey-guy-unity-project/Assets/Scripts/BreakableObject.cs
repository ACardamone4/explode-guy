using System.Collections;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private float _breakingDuration;

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
        Destroy(gameObject);
    }
}
