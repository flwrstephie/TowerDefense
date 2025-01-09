using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public int damage = 1; 
    public float floatHeight = 0.5f; 
    public float effectDuration = 2f; 
    public float spinSpeed = 360f; 

    void Update()
    {
        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log("Enemy hit by bullet!");
            Destroy(other.gameObject); 

            int randomEffect = Random.Range(0, 3); 
            if (randomEffect == 0)
            {
                StartCoroutine(StunEffect());
            }
            else if (randomEffect == 1)
            {
                StartCoroutine(FloatEffect());
            }
            else if (randomEffect == 2)
            {
                StartCoroutine(SpinEffect());
            }
        }
    }
    
    private IEnumerator StunEffect()
    {
        moveSpeed = 0f; 
        yield return new WaitForSeconds(effectDuration); 
        Destroy(gameObject); 
    }

    private IEnumerator FloatEffect()
    {
        Vector3 originalPosition = transform.position;
        float targetHeight = originalPosition.y + floatHeight; 
        float elapsedTime = 0f;

        while (elapsedTime < effectDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, new Vector3(originalPosition.x, targetHeight, originalPosition.z), (elapsedTime / effectDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private IEnumerator SpinEffect()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < effectDuration)
        {
            transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime); 
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
