/// Used on Warning Signs Prefabs, uses animation event for timer.

using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField] private GameObject _boss;
    public void Spawn()
    {
        print("Called");
        GameObject enemySpawning = Instantiate(_boss, this.transform.position, this.transform.rotation);
    }
}
