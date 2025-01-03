using System.Collections;
using UnityEngine;

public class ActivateObject : MonoBehaviour
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
        _spawnedObject.SetActive(true);
        yield return new WaitForSeconds(_waitToDie);
        Destroy(gameObject);
    }
}
