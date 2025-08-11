/*using System.Collections;
using UnityEngine;

public class LifeTrackerController : MonoBehaviour{
    public int lifeCount;
    AudioSource lifePickupSound;
    float randomValue;


    void Start() {
        lifeCount = 0;
        lifePickupSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("LifePickup")) {
            collision.gameObject.SetActive(false); // Temporarily deactivate the pickup
            lifeCount++;
            lifePickupSound.Play();

            // Calculate the new position
            Vector3 newPosition = collision.transform.position;
            randomValue = UnityEngine.Random.Range(-5f, 5f);
            newPosition.x += 60;
            newPosition.y += randomValue;

            // Start a coroutine to move and re-enable the pickup
            StartCoroutine(RespawnPickup(collision.gameObject, newPosition));
        }
    }

    private IEnumerator RespawnPickup(GameObject pickup, Vector3 newPosition) {
        yield return new WaitForSeconds(1f); // Wait for 1 second (or adjust the duration as needed)
        pickup.transform.position = newPosition; // Move the pickup to the new position
        pickup.SetActive(true); // Re-enable the pickup
    }

    
}*/
