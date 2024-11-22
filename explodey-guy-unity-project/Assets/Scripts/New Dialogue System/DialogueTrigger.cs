using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private PlayerInput MPI;
    private InputAction interact;
    private bool playerIsClose;
    private bool initiatedDialogue;
    public bool forceDialogue;
    public bool repeat;
    public float ButtonPresses;
    public float DialogueLines;
    private PlayerControls playerControls;

    public void Awake()
    {
        ButtonPresses = 0;
        playerControls = FindObjectOfType<PlayerControls>();
        MPI = GetComponent<PlayerInput>();
        interact = MPI.currentActionMap.FindAction("Interact");
        interact.started += Handle_Interact;
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    private void Handle_Interact(InputAction.CallbackContext obj)
    {
        if (playerIsClose == true && initiatedDialogue == false)
        {
            initiatedDialogue = true;
            TriggerDialogue();
        } else if (playerIsClose == true && initiatedDialogue == true)
        {
            DialogueManager.Instance.DisplayNextDialogueLine();
        }
        if (playerIsClose == true)
        {
            ButtonPresses += 1;
        }
        if (ButtonPresses == DialogueLines && repeat == false)
        {
            Destroy(gameObject);
        }
    }

    public void IsClose()
    {
        playerIsClose = true;
    }

    public void NotClose()
    {
        playerIsClose = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (forceDialogue == true)
            {
                playerControls.StartCutscene();
                TriggerDialogue();
                initiatedDialogue = true;
            }
            //TriggerDialogue();
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerIsClose = false;
            initiatedDialogue = false;
            DialogueManager.Instance.EndDialogue();
            if (repeat == false)
            {
                Destroy(gameObject);
            }
        }
    }
}