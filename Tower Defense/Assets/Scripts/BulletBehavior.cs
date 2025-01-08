using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Rigidbody _rigidbody;

    void Start()
    {
        // Get the Rigidbody component on the bullet
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Optionally, if you want any additional behaviors (like gravity or effects), you can apply them here.
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hits an enemy
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit by bullet!");
            Destroy(gameObject); // Destroy the bullet
        }
    }
}
