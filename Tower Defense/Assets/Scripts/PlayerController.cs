using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f; 
    public GameObject bulletPrefab; 
    public Transform bulletSpawnPoint; 
    public float bulletSpeed = 20f; 
    [Range(0, 90)] public float shootAngle = 45f; 
    public float shootPower = 20f; 
    public float floatHeight = 3f; 
    public float floatDuration = 2f; 

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
        if (isFloating) return; 

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            float angleInRadians = shootAngle * Mathf.PI / 180;
            
            Vector3 forwardDirection = bulletSpawnPoint.forward; 
            Vector3 velocity = forwardDirection * shootPower;

            velocity += bulletSpawnPoint.up * Mathf.Sin(angleInRadians) * shootPower;

            bulletRb.velocity = velocity;
        }
        Destroy(bullet, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Collision with {other.name} detected."); 

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy detected! Floating the player."); 
            StartCoroutine(FloatEffect());
        }
    }

    private IEnumerator FloatEffect()
    {
        isFloating = true;
        
        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = new Vector3(originalPosition.x, originalPosition.y + floatHeight, originalPosition.z);

        float elapsedTime = 0f;
        
        while (elapsedTime < floatDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsedTime / floatDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(1f); 
        
        elapsedTime = 0f;
        while (elapsedTime < floatDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsedTime / floatDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = originalPosition;
        isFloating = false;
    }
}
