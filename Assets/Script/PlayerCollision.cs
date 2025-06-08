using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement playerMovement;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (playerMovement != null && playerMovement.isInvincible)
            {
                Debug.Log("Hit obstacle while invincible. No effect (collision ignored).");
                return; // Collisions are ignored by Physics.IgnoreCollision
            }

            Debug.Log("Hit obstacle! Restarting level...");
            if (playerMovement != null)
            {
                playerMovement.ResetAfterDeath();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}