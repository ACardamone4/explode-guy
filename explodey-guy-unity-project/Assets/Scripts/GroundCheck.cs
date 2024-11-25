using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private PlayerControls _playerControls;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy" || collision.gameObject.tag == "Ground2")//Checks if the player is touching the ground
        {
            _playerControls.Grounded();

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy" || collision.gameObject.tag == "Ground2")//Checks if the player is touching the ground
        {
            _playerControls.Grounded();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Bouncy" || collision.gameObject.tag == "Ground2")//Checks if the player is touching the ground
        {
            _playerControls.NotGrounded();

        }
    }
}
