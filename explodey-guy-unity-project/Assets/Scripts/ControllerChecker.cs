using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerChecker : MonoBehaviour
{
    private Gamepad Controller;
    [SerializeField] private List<GameObject> controllerObjects;
    [SerializeField] private List<GameObject> keyboardObjects;

    public void ActivateControllerObjects()
    {
        foreach (var obj in controllerObjects)
            obj.SetActive(true);
        foreach (var obj in keyboardObjects)
            obj.SetActive(false);
    }
    
    public void ActivateKeyboardObjects()
    {
        foreach (var obj in keyboardObjects)
            if (obj.gameObject != null)
            {
                obj.SetActive(true);
            }
        foreach (var obj in controllerObjects)
            if (obj.gameObject != null)
            {
                obj.SetActive(false);
            }
    }


    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count > 0)
        {
            print("Controller Found");
            ActivateControllerObjects();
        }
        else
        {
            print("No Controller");
            ActivateKeyboardObjects();
        }
    }
}
