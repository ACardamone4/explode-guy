using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAnimController : MonoBehaviour
{
    [SerializeField] private Animator _checkpointAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _checkpointAnimator.SetBool("Activate", true);
        }
    }
}
