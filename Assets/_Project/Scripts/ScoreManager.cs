using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; } = 0;

    public void ResetScore()
    {
        Score = 0;
    }

    public void AddPoint(int amount = 1)
    {
        Score += amount;
    }
}