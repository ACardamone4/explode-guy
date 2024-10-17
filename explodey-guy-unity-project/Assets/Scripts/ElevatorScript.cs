using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ElevatorScript : MonoBehaviour
{
    public GameObject _stopPlayerObject;

    public bool playerIsClose;
    [SerializeField] private GameObject _interactButton;
    [SerializeField] private GameObject _levelTransition;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public PlayerInput MPI;
    private InputAction interact;

    private void Awake()
    {
        MPI = GetComponent<PlayerInput>();
        interact = MPI.currentActionMap.FindAction("Interact");
        interact.started += Handle_Interact;
    }

    public void OnDisable()
    {
        MPI.currentActionMap.Disable();
        interact.started -= Handle_Interact;
    }

    private void Handle_Interact(InputAction.CallbackContext obj)
    {
        if (playerIsClose)
        {
            _stopPlayerObject.SetActive(true);
            _animator.Play("Elevator_Open");
        }
    }

    public void CloseElevator()
    {
        _spriteRenderer.sortingOrder = 5;
        _animator.Play("Elevator_Close");
    }

    public void SpawnLevelTransition()
    {
        _levelTransition.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            _interactButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            _interactButton.gameObject.SetActive(false);
        }
    }
}