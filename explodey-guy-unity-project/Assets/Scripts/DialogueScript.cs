using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index = 0;

    public float wordSpeed;
    public bool playerIsClose;
    [SerializeField] private bool _cutscene;

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
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index])
            {
                NextLine();
            }
        }
    }

    void Start()
    {
        dialogueText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        //{
        //    if (!dialoguePanel.activeInHierarchy)
        //    {
        //        dialoguePanel.SetActive(true);
        //        StartCoroutine(Typing());
        //    }
        //    else if (dialogueText.text == dialogue[index])
        //    {
        //        NextLine();
        //    }

        //}
        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy)
        {
            RemoveText();
        }
    }

    public void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            RemoveText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            if (_cutscene == true)
            {
                if (!dialoguePanel.activeInHierarchy)
                {
                    dialoguePanel.SetActive(true);
                    StartCoroutine(Typing());
                }
                else if (dialogueText.text == dialogue[index])
                {
                    NextLine();
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            RemoveText();
        }
    }
}