using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float frntbckforce = 5000f;
    public float sideforce = 1000f;
    public Transform cameraRig;

    private bool isUpsideDown = false;
    [HideInInspector] public bool isInvincible = false;
    [HideInInspector] public float invincibleTimer = 0f;
    private Collider playerCollider;
    private Collider[] obstacleColliders;

    void Start()
    {
        // Get player's collider
        playerCollider = GetComponent<Collider>();
        if (playerCollider == null)
        {
            Debug.LogError("Player is missing a Collider!");
        }

        // Find all obstacles with tag "Obstacle"
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacleColliders = new Collider[obstacles.Length];
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacleColliders[i] = obstacles[i].GetComponent<Collider>();
            if (obstacleColliders[i] == null)
            {
                Debug.LogWarning("Obstacle " + obstacles[i].name + " is missing a Collider!");
            }
        }
    }

    void FixedUpdate()
    {
        // Horizontal movement (A/D) with upside-down correction
        float horizontalInput = 0f;
        if (Input.GetKey("d")) horizontalInput = 1f;
        if (Input.GetKey("a")) horizontalInput = -1f;
        if (isUpsideDown) horizontalInput *= -1f;
        rb.AddForce(horizontalInput * sideforce * Time.fixedDeltaTime, 0, 0, ForceMode.VelocityChange);

        // Forward/backward movement (W/S)
        if (Input.GetKey("w")) rb.AddForce(0, 0, frntbckforce * Time.fixedDeltaTime);
        if (Input.GetKey("s")) rb.AddForce(0, 0, -frntbckforce * Time.fixedDeltaTime);
    }

    void Update()
    {
        // Gravity flip
        if (Input.GetKeyDown(KeyCode.Space)) FlipGravity();

        // Fall detection
        if (transform.position.y < -10f || transform.position.y > 50f)
        {
            Debug.Log("Player fell out of bounds! Restarting...");
            ResetAfterDeath();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Invincibility timer
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            Debug.Log("Invincibility remaining: " + invincibleTimer.ToString("F2") + " seconds"); // Debug timer
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
                RestoreCollisions();
                Debug.Log("Invincibility ended");
            }
        }
    }

    void FlipGravity()
    {
        rb.useGravity = false;

        // Preserve X/Z velocity, reset Y
        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.y = 0f;
        rb.linearVelocity = currentVelocity;

        // Flip gravity
        Physics.gravity = -Physics.gravity;
        isUpsideDown = !isUpsideDown;

        // Small nudge forward
        float nudgeForce = isUpsideDown ? -frntbckforce : frntbckforce;
        rb.AddForce(0, 0, nudgeForce * 0.2f * Time.fixedDeltaTime, ForceMode.VelocityChange);

        rb.useGravity = true;
        StartCoroutine(RotateCamera());
    }

    public void ResetAfterDeath()
    {
        Physics.gravity = new Vector3(0, -9.81f, 0);
        isUpsideDown = false;
        if (cameraRig != null)
        {
            cameraRig.rotation = Quaternion.identity;
        }
        // Only restore collisions if not invincible
        if (!isInvincible)
        {
            RestoreCollisions();
        }
    }

    public void ActivateInvincibility(float duration)
    {
        isInvincible = true;
        invincibleTimer = Mathf.Max(invincibleTimer, duration); // Prevent resetting to shorter duration
        IgnoreCollisions();
        Debug.Log("Invincibility activated for " + duration + " seconds!");
    }

    void IgnoreCollisions()
    {
        if (playerCollider != null && obstacleColliders != null)
        {
            foreach (Collider obstacle in obstacleColliders)
            {
                if (obstacle != null)
                {
                    Physics.IgnoreCollision(playerCollider, obstacle, true);
                    Debug.Log("Ignoring collision with " + obstacle.gameObject.name);
                }
            }
        }
    }

    void RestoreCollisions()
    {
        if (playerCollider != null && obstacleColliders != null)
        {
            foreach (Collider obstacle in obstacleColliders)
            {
                if (obstacle != null)
                {
                    Physics.IgnoreCollision(playerCollider, obstacle, false);
                   // Debug.Log("Restored collision with " + obstacle.gameObject.name);
                }
            }
        }
    }

    IEnumerator RotateCamera()
    {
        float duration = 0.5f;
        float time = 0f;
        Quaternion startRotation = cameraRig.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, 180f);

        while (time < duration)
        {
            cameraRig.rotation = Quaternion.Slerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        cameraRig.rotation = endRotation;
    }
}