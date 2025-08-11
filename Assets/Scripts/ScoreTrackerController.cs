using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreTrackerController : MonoBehaviour {
    int score;
    int highScore;
    public TMP_Text highscoreText;
    public TMP_Text scoreText;
    public TMP_Text endText;
    public GameObject endText_rect;
    AudioSource scoreSound;


    void Start() {
        score = 0;
        scoreSound = GetComponent<AudioSource>();
        highScore = PlayerPrefs.GetInt("HighScore", 0); // Load the saved high score
        highscoreText.text = "HIGHSCORE: " + highScore.ToString();
        UpdateScoreText();

        // Ensure endText and endImage are invisible at the start
        if (endText != null) endText.gameObject.SetActive(false);
        if (endText_rect != null) endText_rect.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "TopTower") {
            IncrementScore();
        }
    }

    private void IncrementScore() {
        score++;
        UpdateScoreText();
        scoreSound.Play();
    }

    private void UpdateScoreText() {
        scoreText.text = score.ToString(); // Update the current score UI
    }
    public void DisplayEndScore() {
    if (score > highScore) {
        Debug.Log("NEW HIGHSCOREE");
        highScore = score;
        endText.text = $"<size=100>New Highscore!</size>\n\n" + // GAME OVER is larger
        $"High Score: <color=yellow>{highScore}</color>\n";
        PlayerPrefs.SetInt("HighScore", highScore); // Save the new high score
    }
    else{
        // Display high score and the score for the round
        endText.text = $"<size=100>GAME OVER</size>\n\n" + // GAME OVER is larger
        $"High Score: <color=white>{highScore}</color>\n" +
        $"Your Score: <color=#D32A4F>{score}</color>";
    }
    endText.gameObject.SetActive(true); // Make sure the endText UI is visible

    // Make endText and endText_rect visible
    if (endText != null) endText.gameObject.SetActive(true);
    if (endText_rect != null) endText_rect.SetActive(true);
}

}
