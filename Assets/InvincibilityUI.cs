using UnityEngine;
using TMPro; // Required for TextMeshPro

public class InvincibilityUI : MonoBehaviour
{
    public PlayerMovement player; // Reference to PlayerMovement script
    public TextMeshProUGUI timerText; // Reference to TMP_Text component

    void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("Timer Text (TMP) is not assigned in InvincibilityUI!");
        }
        if (player == null)
        {
            Debug.LogError("Player is not assigned in InvincibilityUI!");
        }
        timerText.gameObject.SetActive(false); // Hide by default
    }

    void Update()
    {
        if (player != null && player.isInvincible && timerText != null)
        {
            timerText.gameObject.SetActive(true);
            timerText.text = "Invincibility: " + player.invincibleTimer.ToString("F1") + "s";
        }
        else
        {
            timerText.gameObject.SetActive(false);
        }
    }
}