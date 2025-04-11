using UnityEngine;
using UnityEngine.SceneManagement;

public class AliveInScene : MonoBehaviour
{
    private static AliveInScene instance;
    [SerializeField] private string _sceneName;


    // Start is called before the first frame update
    private void Awake()
    {
        

        // Checks if there is no instance of the GameManager in the scene
        if (instance == null)
        {
            // Makes the instance 
            instance = this;
            // Makes it so this object does not get destroyed on load
            DontDestroyOnLoad(instance);
        }
        else
        {
            // Makes it so there aren't multiple instances of the Game Manager
            Destroy(gameObject);
       
        }

       
    }

    private void Update()
    {
    }
}
