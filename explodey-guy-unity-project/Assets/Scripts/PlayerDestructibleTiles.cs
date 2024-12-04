using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerDestructibleTiles : MonoBehaviour
{
    public Tilemap destructibleTileMap;
    private Vector3 tilePos;
    [SerializeField] private GameObject TestObject;
    [SerializeField] private Transform TestSpawn;

    private void Start()
    {
        destructibleTileMap = GetComponent<Tilemap>();
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Vector3 hitPosition = Vector3.zero;
    //        foreach (ContactPoint2D hit in collision.contacts)
    //        {
    //            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
    //            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
    //            destructibleTileMap.SetTile(destructibleTileMap.WorldToCell(hitPosition), null);
    //            print("HitPosition =" + hitPosition);

    //            TestSpawn.position = new Vector3(hitPosition.x, hitPosition.y);
    //            GameObject BlockRefresh = Instantiate(TestObject, TestSpawn.position, TestSpawn.rotation);
    //        }
    //    }
    //}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 hitPosition = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                destructibleTileMap.SetTile(destructibleTileMap.WorldToCell(hitPosition), null);
                print("HitPosition =" + hitPosition);

                TestSpawn.position = new Vector3(hitPosition.x, hitPosition.y);
                GameObject BlockRefresh = Instantiate(TestObject, TestSpawn.position, TestSpawn.rotation);
            }
        }
    }


}
