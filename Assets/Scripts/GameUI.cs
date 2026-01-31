using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    [Header("Game Settings")]
    public int scorePerBox = 2;

    private int score = 0;

    void Start()
    {
        // Cacher Game Over au début
        gameOverPanel.SetActive(false);

        // Mettre à jour le score au début
        UpdateScoreDisplay();
    }

    void Update()
    {
        // Test avec la touche A pour ajouter score
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddScore();
        }

        // Test avec la touche G pour Game Over
        if (Input.GetKeyDown(KeyCode.G))
        {
            ShowGameOver();
        }
    }

    // Ajouter des points
    public void AddScore()
    {
        score += scorePerBox;
        UpdateScoreDisplay();
        Debug.Log("Score: " + score);
        StartCoroutine(ScoreEffect());
    }

    // Mettre à jour le texte du score en temps réel
    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + score;
        }
    }

    // Montrer Game Over
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (gameOverText != null)
            {
                gameOverText.text = "GAME OVER!\nSCORE: " + score;
            }

            Debug.Log("Game Over!");
        }
    }

    // Effet visuel quand le score change (agrandit + vert)
    System.Collections.IEnumerator ScoreEffect()
    {
        if (scoreText != null)
        {
            Vector3 originalScale = scoreText.transform.localScale;
            Color originalColor = scoreText.color;

            // Agrandir et mettre en vert
            scoreText.transform.localScale = originalScale * 1.2f;
            scoreText.color = Color.green;

            yield return new WaitForSeconds(0.1f);

            // Retour à la normale
            scoreText.transform.localScale = originalScale;
            scoreText.color = originalColor;
        }
    }

    // Appelé quand le player collecte un box
    public void CollectBox(int points = 2)
    {
        score += points;
        UpdateScoreDisplay();
        StartCoroutine(ScoreEffect());
    }

    // Appelé quand le player touche un obstacle
    public void PlayerTrapped()
    {
        ShowGameOver();
    }
}
