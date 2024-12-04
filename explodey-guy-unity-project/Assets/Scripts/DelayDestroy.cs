using System.Collections;
using UnityEngine;

public class DelayDestroy : MonoBehaviour
{
    [SerializeField] private float _waitToDie;

    public void Awake()
    {
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        yield return new WaitForSeconds(_waitToDie);
        Destroy(gameObject);
    }
}
