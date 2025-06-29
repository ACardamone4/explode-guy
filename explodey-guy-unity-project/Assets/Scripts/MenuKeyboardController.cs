using UnityEngine;
using UnityEngine.EventSystems;

public class MenuKeyboardController : MonoBehaviour
{
    [SerializeField] private GameObject _creditsButton;
    [SerializeField] private GameObject _creditsBack;

    [SerializeField] private GameObject _levelSelectButton;
    [SerializeField] private GameObject _levelOne;

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

    public void LevelSelectOn()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_levelOne);
    }
    
    public void LevelSelectOff()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_levelSelectButton);
    }
}
