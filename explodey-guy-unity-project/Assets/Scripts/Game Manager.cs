/*****************************************************************************
// File Name :         Game Manager.cs
// Author :            Amber C. Cardamone
// Creation Date :     August 22nd, 2024
//
// Brief Description : Keeps track of the player's score, and loads the lose screen.
*****************************************************************************/
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour//, IDataPersistence
{
    [SerializeField] private DataPersistenceManager _dataPersistanceManager;
    [SerializeField] private GameObject _dataPersistanceManagerGameObject;

    [SerializeField] private GameObject _currentRoom;

    [SerializeField] private bool hasStartRoom;

    public GameObject CurrentRoom { get => _currentRoom; set => _currentRoom = value; }


    /// <summary>
    /// Sets the player's health and score back to their base stats, and changed the text back to their bases.
    /// </summary>
    void Start()
    {
        hasStartRoom = false;
        _dataPersistanceManagerGameObject = GameObject.Find("DataPersistanceManager");
        if (_dataPersistanceManagerGameObject != null)
        {
            _dataPersistanceManager = _dataPersistanceManagerGameObject.GetComponent<DataPersistenceManager>();
        }


        StartCoroutine(SpawnLevel());

    }

    public void NewLevel()
    {
        StartCoroutine(SpawnLevel());
    }

    private IEnumerator SpawnLevel()
    {
        yield return new WaitForSeconds(.1f);
        print(_dataPersistanceManager.GameData.RoomName);
            _currentRoom = Resources.Load<GameObject>("Prefabs/Levels/" + _dataPersistanceManager.GameData.RoomName);
            print(_currentRoom);
        if (_currentRoom != null)
        {
            Instantiate(_currentRoom, new Vector2(0, 0), Quaternion.identity);
        } else
        {
            _dataPersistanceManager.GameData.RoomName = "Tutorial";
            _currentRoom = Resources.Load<GameObject>("Prefabs/Levels/" + _dataPersistanceManager.GameData.RoomName);
            Instantiate(_currentRoom, new Vector2(0, 0), Quaternion.identity);
        }
            yield return new WaitForSeconds(.1f);
        hasStartRoom = true;
    }

    private void Update()
    {


        if (hasStartRoom == true)
        {
            _currentRoom = GameObject.FindGameObjectWithTag("Room");
            if (_currentRoom != null)
            {
                _dataPersistanceManager.GameData.RoomName = _currentRoom.name;
            }
        }


    }

    public void SpawnInitialRoom()
    {
        Resources.Load<GameObject>(_dataPersistanceManager.GameData.RoomName);
    }
}