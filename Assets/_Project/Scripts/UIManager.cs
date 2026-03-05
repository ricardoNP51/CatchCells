using UnityEngine;
using TMPro; // Esto es para que reconozca el texto de TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("Referencias de UI")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;

    [Header("Referencias del Juego")]
    public RoundManager roundManager;
    public ScoreManager scoreManager;

    void Update()
    {
        // Esto actualiza el tiempo en pantalla
        if (roundManager != null && timerText != null)
        {
            timerText.text = "Tiempo: " + roundManager.TimeLeft.ToString("F1");
        }

        // Esto actualiza los puntos en pantalla
        if (scoreManager != null && scoreText != null)
        {
            scoreText.text = "Puntos: " + scoreManager.Score;
        }
    }
}