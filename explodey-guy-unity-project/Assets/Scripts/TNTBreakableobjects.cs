using System.Collections;
using UnityEngine;

public class TNTBreakableobjects : MonoBehaviour
{
    [SerializeField] private bool _respawns;
    [SerializeField] private Transform _self;
    [SerializeField] private GameObject _rubble;
    [SerializeField] private GameObject _blockOutline;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Explosion")
        {
            GameObject BrokenParticles = Instantiate(_rubble, _self.position, _self.rotation);
            if (_respawns)
            {
                GameObject BlockRefresh = Instantiate(_blockOutline, _self.position, _self.rotation);
            }
            Destroy(gameObject);
        }
    }
}
