using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float powerUpDuration = 700f;
    public ParticleSystem collectEffect; // Optional: assign in Inspector
    public AudioSource collectSound; // Optional: assign in Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.ActivateInvincibility(powerUpDuration);
                Debug.Log("Coin collected! Invincibility activated!");

                // Optional: Play effects
                if (collectEffect != null) Instantiate(collectEffect, transform.position, Quaternion.identity);
                if (collectSound != null) collectSound.Play();

                Destroy(gameObject); // Destroy the coin
            }
        }
    }
}