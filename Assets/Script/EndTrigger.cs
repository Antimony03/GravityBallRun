using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameObject levelCompletePanel; // Reference to the Level Complete UI panel

    void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the end! Level completed!");
            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true); // Show UI panel
                Time.timeScale = 0f; // Pause the game
            }
            else
            {
                Debug.LogError("Level Complete Panel is not assigned in EndTrigger!");
            }
        }*/
        levelCompletePanel.SetActive(true);
    }
}