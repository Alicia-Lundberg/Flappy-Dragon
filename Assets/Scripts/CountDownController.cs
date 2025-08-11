using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class CountdownController : MonoBehaviour {
    public Text countdownText; 
    private bool isCountdownActive = false;

    void Start() {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown() {
        int countdownTime = 3; 

        // Loop through countdown (3, 2, 1)
        while (countdownTime > 0) {
            countdownText.text = countdownTime.ToString();  // Update the text with current countdown value
            yield return new WaitForSeconds(1);  // Wait for 1 second
            countdownTime--;  // Decrease the countdown
        }

        countdownText.text = "GO!"; 
        yield return new WaitForSeconds(1); 

        countdownText.gameObject.SetActive(false); 
    }
}
