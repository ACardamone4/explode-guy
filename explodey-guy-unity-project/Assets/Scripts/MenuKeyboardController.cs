using UnityEngine;
using UnityEngine.EventSystems;

public class MenuKeyboardController : MonoBehaviour
{
    [SerializeField] private GameObject _creditsButton;
    [SerializeField] private GameObject _creditsBack;

    public void CreditsOn()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_creditsBack);
    }

    public void CreditsOff()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_creditsButton);
    }
}
