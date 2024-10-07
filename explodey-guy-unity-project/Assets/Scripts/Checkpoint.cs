
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public CheckpointManager CM; //Grabs the game manager

    /// <summary>
    /// Grabs the GameManager script at the start.
    /// </summary>
    void Start()
    {
        CM = FindObjectOfType<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Sets the GameManager's checkpoint system to this current checkpoint
            CM.LastCheckPointPos = transform.position;
            // Removes the checkpoint, making it so the player can't accidentally go back to an older checkpoint
            Destroy(gameObject);
        }
    }
}