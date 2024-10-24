using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ElevatorEndScript : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _cutsceneStop;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Elevator")
        {
            print("collided");
            Destroy(collision.gameObject);
            _animator.Play("Elevator_Open");
        }
    }
    public void CloseElevator()
    {
        _spriteRenderer.sortingOrder = 3;
        _animator.Play("Elevator_Close");
        _cutsceneStop.SetActive(false);
    }

    public void SpawnLevelTransition()
    {
        print("Forced to be here");
    }
}
