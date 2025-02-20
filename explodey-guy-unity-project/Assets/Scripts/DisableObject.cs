using UnityEngine;

public class DisableObject : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    public void Disable()
    {
        obj.SetActive(false);
    }
}
