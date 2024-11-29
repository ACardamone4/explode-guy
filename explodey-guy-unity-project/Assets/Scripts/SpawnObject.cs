using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject spawnObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "NPC")
        {
            spawnObject.SetActive(true);
        }
    }
}
