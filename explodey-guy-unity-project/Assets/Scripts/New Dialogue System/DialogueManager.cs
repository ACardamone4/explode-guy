using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;

    public float typingSpeed = 0.2f;

    public PlayerControls playerControls;

    public GameObject Icons;

    //public Animator animator;
    [SerializeField] private GameObject _dialogueBox;

    private void Awake()
    {
        playerControls = FindObjectOfType<PlayerControls>();
        if (Instance == null)
            Instance = this;
        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;

       // animator.Play("show");
       _dialogueBox.SetActive(true);

        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            print(letter);

            if (letter.ToString().Equals("?") || letter.ToString().Equals("!") || letter.ToString().Equals("."))
            {
                yield return new WaitForSeconds(.2f);
            }
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void EndDialogue()
    {
        playerControls.StopCutscene();
        isDialogueActive = false;
        //animator.Play("hide");
        if (_dialogueBox != null)
        {
            _dialogueBox.SetActive(false);
        }
        Icons.SetActive(false);
    }
}

