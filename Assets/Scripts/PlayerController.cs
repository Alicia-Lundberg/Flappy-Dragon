using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    public ScoreTrackerController scoreTracker;
    public TextMeshProUGUI countdownText;
    Rigidbody2D rb;
    AudioSource wingFlapSound;
    AudioSource lifePickupSound;
    AudioSource loseLifeSound;
    AudioSource backgroundMusic;
    public GameObject restartButton; 
    public GameObject quitButton; 

    bool flyUp; 
    float upForce = 600f;
    float startSpeed = 5f;
    float maxSpeed = 8f;
    private bool crashed = false;
    public GameObject heartPrefab; 
    public Transform lifeContainer; 
    public int lifeCount;
    private bool isInvincible = false;
    float randomValue;
    public GameObject[] lifeHearts;

    private bool isCountingDown = true;  
    

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.right * startSpeed, ForceMode2D.Impulse);
        
        AudioSource[] audioSources = GetComponents<AudioSource>();
        wingFlapSound = audioSources[0]; 
        lifePickupSound = audioSources[1];
        loseLifeSound = audioSources[2];
        backgroundMusic = audioSources[4];
      
        restartButton.SetActive(false); // Hide buttons initially
        quitButton.SetActive(false);
        scoreTracker.scoreText.gameObject.SetActive(false);
        Time.timeScale = 1;

        lifeCount = 3;
        UpdateLivesUI(); // Update the life hearts UI at the start

        StartCoroutine(StartCountdown());
    }


    void Update() {
        if (isCountingDown) return;  // Prevent actions during countdown
        if (Input.GetKeyDown(KeyCode.Space)) { flyUp = true;} //fly up when space is pressed
        if (Input.GetKeyDown(KeyCode.Q)) { QuitGame(); } //Quit on q
        GameOver(); 
    }

    private void FixedUpdate() {
        if (isCountingDown) return;  // Prevent actions during countdown

        if (flyUp) { 
            flyUp = false; // Reset the flag after processing
            rb.AddForce(Vector2.up * upForce);
            wingFlapSound.Play();
        }
        if (rb.linearVelocity.x < maxSpeed) { // Move player forward
            rb.AddForce(Vector2.right);
        }
    }

    private IEnumerator StartCountdown() {
        int countdownTime = 3;  // 3 seconds countdown

        while (countdownTime > 0) {
            countdownText.text = countdownTime.ToString(); 
            yield return new WaitForSeconds(1f); 
            countdownTime--;
        }

        countdownText.text = "GO!";  
        yield return new WaitForSeconds(1f);  // Wait for a moment before continuing

        countdownText.gameObject.SetActive(false);  
        isCountingDown = false;  // Allow the game to continue

        backgroundMusic.Play();  
        scoreTracker.scoreText.gameObject.SetActive(true);
    }
    
    private void GameOver() {
        if (playerCrashed()) {
            EndGame();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision) { //if player collides with an element
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Cloud")) {
            if(lifeCount > 0){ 
                if (isInvincible) return; // Skip if player is immune
                if (isCountingDown) return; // Skip if game has not started
                lifeCount--;
                loseLifeSound.Play();
                UpdateLivesUI();
                StartCoroutine(BlinkAndInvincibility());
            }
            else{
                crashed = true;
            } 
        }  
    }
    private IEnumerator BlinkAndInvincibility() {
        isInvincible = true; // Activate invincibility
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (spriteRenderer == null || playerCollider == null) yield break; // Exit if missing components

        for (int i = 0; i < 5; i++) { // Blink 5 times
            spriteRenderer.enabled = false; // Hide player
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds
            spriteRenderer.enabled = true; // Show player
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds
        }

        isInvincible = false; // Deactivate invincibility
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("LifePickup")) { //Using layers: Edit > Project Settings > Physics 2D > Collision Matrix, i made sure the layer "scoretracker" and "lifePickup" cannot interract with eachother
            collision.gameObject.SetActive(false); 
            if(lifeCount < 3){ //maximum nbr of lives is three
                lifeCount++;
                lifePickupSound.Play();
                UpdateLivesUI();
            }
            Vector3 newPosition = collision.transform.position;
            randomValue = UnityEngine.Random.Range(-4f, 4f);
            newPosition.x += 60;
            newPosition.y += randomValue;

            // Start a coroutine to move and re-enable the pickup
            StartCoroutine(RespawnPickup(collision.gameObject, newPosition));
        }
        if (collision.CompareTag("BottomTower") || collision.CompareTag("TopTower")) {
            if(lifeCount > 0){ 
                if (isInvincible) return; // Skip if player is immune
                lifeCount--;
                loseLifeSound.Play();
                UpdateLivesUI();
                StartCoroutine(BlinkAndInvincibility());
            }
            else{
                crashed = true;
            } 
        } 
    }

    private void UpdateLivesUI() {
        for (int i = 0; i < lifeHearts.Length; i++) { // Loop through the heart images and enable/disable based on the lifeCount
            if (i < lifeCount) {
                lifeHearts[i].SetActive(true); // Show heart
            } else {
                lifeHearts[i].SetActive(false); // Hide heart
            }
        }
    }

    private IEnumerator RespawnPickup(GameObject pickup, Vector3 newPosition) {
        yield return new WaitForSeconds(1f); // Wait for 1 second (or adjust the duration as needed)
        pickup.transform.position = newPosition; // Move the pickup to the new position
        pickup.SetActive(true); // Re-enable the pickup
    }

    private bool playerCrashed() {
        return crashed;
    }
    

    private void EndGame() {
        restartButton.SetActive(true); // Show restart and quit buttons
        quitButton.SetActive(true);
        Time.timeScale = 0; // Pause the game
        scoreTracker.DisplayEndScore(); // Call the method to show scores
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame() {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

}