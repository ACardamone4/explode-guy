using UnityEngine;

public class TurnOffSelf : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _self;

    private void Update()
    {
        if (_target == null)
        {
            print("TurnOff");
            _self.SetActive(false);
        }
    }
}
