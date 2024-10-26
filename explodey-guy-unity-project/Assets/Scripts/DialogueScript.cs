using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameObject;
    public string speakerName;
    public string[] dialogue;
    private int index = 0;

    public float wordSpeed;
    public bool playerIsClose;
    [SerializeField] private bool _cutscene;
    [SerializeField] private bool _cameraSwapper;
    [SerializeField] private bool _sayNewText;
    [SerializeField] private bool _repeatCutscene;
    [SerializeField] private GameObject _cutsceneStop;
    [SerializeField] private GameObject _otherText;
    [SerializeField] private GameObject _thisText;
    [SerializeField] private GameObject _interactButton;
    [SerializeField] private SwapCams _swapCams;
    [SerializeField] private Sprite _speakerSprite;
    [SerializeField] private Image _speakerImageObject;

    public PlayerInput MPI;
    private InputAction interact;

    private void Awake()
    {
        
        if (_cutscene == true)
        {
            _cutsceneStop.SetActive(true);
        }
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
                if (_cutscene == false)
                {
                    
                    _interactButton.SetActive(false);
                }
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
        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy)
        {
            RemoveText();
        }
    }

    public void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        if (dialoguePanel.activeInHierarchy && dialoguePanel.gameObject != null)
        {
            dialoguePanel.SetActive(false);
        }
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
            if (_cutscene == false)
            {
                _interactButton.SetActive(true);
            }
            else if (_cutscene == true)
            {
                if (_repeatCutscene == false)
                {
                    _cutscene = false;
                }
                _cutsceneStop.SetActive(false);
            }
            if (_cameraSwapper == true)
            {
                _swapCams.SwapCamSwap();
            }
            if (_sayNewText == true)
            {
                _otherText.SetActive(true);
                _thisText.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            speakerNameObject.text = speakerName;
            _speakerImageObject.sprite = _speakerSprite;
            playerIsClose = true;
            if (_cutscene == false)
            {
                _interactButton.gameObject.SetActive(true);
            }
            else if (_cutscene == true)
            {
                if (!dialoguePanel.activeInHierarchy)
                {
                    _cutsceneStop.SetActive(true);
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
            _interactButton.gameObject.SetActive(false);
        }
    }
}