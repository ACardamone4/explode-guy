using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAndDestroy : MonoBehaviour
{
    private GameObject[] bombs;
    private GameObject[] breakables;

    // Start is called before the first frame update
    //void Awake()
    //{
    //    bombs = GameObject.FindGameObjectsWithTag("Bomb");
    //    breakables = GameObject.FindGameObjectsWithTag("Ground2");
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bombs = GameObject.FindGameObjectsWithTag("Bomb");
        breakables = GameObject.FindGameObjectsWithTag("Ground2");
        if (collision.gameObject.tag == "Player")
        {
            if (bombs.Length > 0)
            {
                foreach (GameObject go in bombs)
                {
                    Destroy(go);
                }
            }
            if (breakables.Length > 0)
            {
                foreach (GameObject go in breakables)
                {
                    Destroy(go);
                }
            }
        }
    }
}
