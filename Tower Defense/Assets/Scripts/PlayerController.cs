using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f; // Movement speed of the player
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform bulletSpawnPoint; // Where the bullet spawns
    public float bulletSpeed = 20f; // Speed of the bullet
    [Range(0, 90)] public float shootAngle = 45f; // Angle for shooting
    public float shootPower = 20f; // Power for the shot

    private Vector2 _initialVelocity;

    void Update()
    {
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
}
