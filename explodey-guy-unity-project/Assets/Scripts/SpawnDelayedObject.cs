using System.Collections;
using UnityEngine;

public class SpawnDelayedObject : MonoBehaviour
{
    [SerializeField] private float _waitTime;
    [SerializeField] private GameObject _spawnedObject;

    public void Awake()
    {
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        yield return new WaitForSeconds(_waitTime);
        GameObject TNTOutline = Instantiate(_spawnedObject, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
