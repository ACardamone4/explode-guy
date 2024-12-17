using System.Collections.Generic;
using UnityEngine;

public class TurnOffObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> disableObjects;
    [SerializeField] private bool _remove;


    // Update is called once per frame
    void Update()
    {
        if (_remove)
        {
            foreach (var obj in disableObjects)
            {
                obj.SetActive(false);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _remove = true;
        }
    }
}
