using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f; // Movement speed of the player
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform bulletSpawnPoint; // Where the bullet spawns
    public float bulletSpeed = 20f; // Speed of the bullet
    [Range(0, 90)] public float shootAngle = 45f; // Angle for shooting
    public float shootPower = 20f; // Power for the shot
    public float floatHeight = 3f; // How high the player floats
    public float floatDuration = 2f; // Duration of the floating effect

    private Rigidbody _rb;
    private bool isFloating = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            Debug.LogError("No Rigidbody component found on the Player GameObject.");
        }
    }

    void Update()
    {
        if (isFloating) return; // Disable player control during floating

        // Handle player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        // Handle shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Get the rigidbody component of the bullet
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            // Calculate the initial velocity based on the angle and power
            float angleInRadians = shootAngle * Mathf.PI / 180;

            // Adjust the bullet's velocity to be in the forward direction of the player
            Vector3 forwardDirection = bulletSpawnPoint.forward; // Get the forward direction of the spawn point
            Vector3 velocity = forwardDirection * shootPower;

            // Optionally, you can add an upward trajectory based on the angle
            velocity += bulletSpawnPoint.up * Mathf.Sin(angleInRadians) * shootPower;

            // Apply the calculated velocity to the bullet
            bulletRb.velocity = velocity;
        }

        // Optionally destroy the bullet after some time
        Destroy(bullet, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision with {other.name} detected."); // Debug log for testing

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy detected! Floating the player."); // Debug log for enemy collision
            StartCoroutine(FloatEffect());
        }
    }

    private IEnumerator FloatEffect()
    {
        isFloating = true;

        // Save the original position
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = new Vector3(originalPosition.x, originalPosition.y + floatHeight, originalPosition.z);

        float elapsedTime = 0f;

        // Gradually move the player upward
        while (elapsedTime < floatDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / floatDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player reaches the target height
        transform.position = targetPosition;

        yield return new WaitForSeconds(1f); // Optional pause at the top

        // Return to the ground
        elapsedTime = 0f;
        while (elapsedTime < floatDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / floatDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player returns to the ground
        transform.position = originalPosition;

        isFloating = false;
    }
}
