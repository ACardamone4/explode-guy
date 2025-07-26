using UnityEngine;

public class ActivateEndScreenEffects : MonoBehaviour
{
    [SerializeField] private GameObject _endScreenControls;
    [SerializeField] private EndscreenMenuSpawn _endScreenMenuSpawn;
    // Update is called once per frame
    void Update()
    {
        _endScreenControls = GameObject.Find("EndCheckpoint");
        if (_endScreenControls != null)
        {
            _endScreenMenuSpawn = _endScreenControls.GetComponent<EndscreenMenuSpawn>();
        }
    }

    public void Continue()
    {
        _endScreenMenuSpawn.Continue();
    }
}
