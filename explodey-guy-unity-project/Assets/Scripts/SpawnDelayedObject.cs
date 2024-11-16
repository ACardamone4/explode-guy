using System.Collections;
using UnityEngine;

public class SpawnDelayedObject : MonoBehaviour
{
    [SerializeField] private float _waitToDie;
    [SerializeField] private float _waitToSpawn;
    [SerializeField] private GameObject _spawnedObject;

    public void Awake()
    {
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        yield return new WaitForSeconds(_waitToSpawn);
        GameObject TNTOutline = Instantiate(_spawnedObject, gameObject.transform.position, gameObject.transform.rotation);
        yield return new WaitForSeconds(_waitToDie);
        Destroy(gameObject);
    }
}
