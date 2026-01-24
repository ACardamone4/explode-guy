using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private GameObject _warningRight;
    [SerializeField] private GameObject _warningLeft;
    [SerializeField] private float _bossAttackCooldown;
    private bool canAttack;
    private bool shouldBeActive;
    [SerializeField] private Transform[] _bossSpawnPointsLeft;
    [SerializeField] private Transform[] _bossSpawnPointsRight;
    private int randomSpawnTarget;
    private Transform targetSpawnPoint;
    private int sideDecider;

    private void Start()
    {
        canAttack = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("ACTIVATE THE WORM");
            canAttack = true;
            shouldBeActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("DEACTIVATE THE WORM");
            canAttack = true;
            shouldBeActive = false;
        }
    }

    private void Update()
    {
        if (canAttack && shouldBeActive)
        {
            canAttack = false;
            StartCoroutine(SpawnCooldown());
            sideDecider = Random.Range(0, 2);
            randomSpawnTarget = Random.Range(0, 3);
            if (sideDecider == 1)
            {
                targetSpawnPoint = _bossSpawnPointsLeft[randomSpawnTarget];
                GameObject enemySpawning = Instantiate(_warningLeft, targetSpawnPoint.position, _warningLeft.transform.rotation);
            } 
            else
            {
                targetSpawnPoint = _bossSpawnPointsRight[randomSpawnTarget];
                GameObject enemySpawning = Instantiate(_warningRight, targetSpawnPoint.position, _warningRight.transform.rotation);
            }
        }
    }

    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(_bossAttackCooldown);
        if (shouldBeActive) {
        canAttack = true;
            }
    }
}
