using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _activeObject;
    // Start is called before the first frame update
    void Awake()
    {
        if (_player.activeSelf == false)
        {
            _activeObject.SetActive(true);
        }
    }

    
}
