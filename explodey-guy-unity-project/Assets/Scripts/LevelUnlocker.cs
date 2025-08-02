using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] private GameObject _dataPersistanceManagerGameObject;
    [SerializeField] private DataPersistenceManager _dataPersistanceManager;
    [SerializeField] private int _levelsUnlocked;
    [SerializeField] private List<GameObject> _levelButtonsGameObject;
    [SerializeField] private Button _levelButtonsButton;

    // Start is called before the first frame update
    void Awake()
    {

        _dataPersistanceManagerGameObject = GameObject.Find("DataPersistanceManager");
        if (_dataPersistanceManagerGameObject != null)
        {
            _dataPersistanceManager = _dataPersistanceManagerGameObject.GetComponent<DataPersistenceManager>();
        }
        if (_dataPersistanceManager != null)
        {
            _levelsUnlocked = _dataPersistanceManager.GameData.LevelsUnlocked;
        }

        if (_levelsUnlocked > -1)
        {
            CheckActiveLevels(0);
        }
        if (_levelsUnlocked > 0)
        {
            CheckActiveLevels(1);
        }
        if (_levelsUnlocked > 1)
        {
            CheckActiveLevels(2);
        }
        if (_levelsUnlocked > 2)
        {
            CheckActiveLevels(3);
        }
        if (_levelsUnlocked > 3)
        {
            CheckActiveLevels(4);
        }
        if (_levelsUnlocked > 4)
        {
            CheckActiveLevels(5);
        }
        if (_levelsUnlocked > 5)
        {
            CheckActiveLevels(6);
        }
        if (_levelsUnlocked > 6)
        {
            CheckActiveLevels(7);
        }
        if (_levelsUnlocked > 7)
        {
            CheckActiveLevels(8);
        }
    }

    private void Update()
    {
        if (_dataPersistanceManager != null)
        {
            _levelsUnlocked = _dataPersistanceManager.GameData.LevelsUnlocked;
        }
        if (_levelsUnlocked == 0)
        {
            _levelButtonsGameObject[1].GetComponent<Button>().interactable = false;
            _levelButtonsGameObject[2].GetComponent<Button>().interactable = false;
            _levelButtonsGameObject[3].GetComponent<Button>().interactable = false;
            _levelButtonsGameObject[4].GetComponent<Button>().interactable = false;
            _levelButtonsGameObject[5].GetComponent<Button>().interactable = false;
            _levelButtonsGameObject[6].GetComponent<Button>().interactable = false;
            _levelButtonsGameObject[7].GetComponent<Button>().interactable = false;
            _levelButtonsGameObject[8].GetComponent<Button>().interactable = false;
        }
    }

    public void CheckActiveLevels(int Index)
    {
        if (Index >= 0 && Index < _levelButtonsGameObject.Count)
        {
            _levelButtonsGameObject[Index].SetActive(true);
            _levelButtonsButton = _levelButtonsGameObject[Index].GetComponent<Button>();
            _levelButtonsButton.interactable = true;
        }
    }

}
