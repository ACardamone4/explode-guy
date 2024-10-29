using UnityEngine;

public class NormalTime : MonoBehaviour
{
    public void Update()
    {
        Time.timeScale = 1;
        //Time.fixedDeltaTime = Time.deltaTime;
    }

}
