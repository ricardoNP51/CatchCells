using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    private bool isDead = false;
    private ScoreManager scoreManager;

    public void Init(ScoreManager scoreMgr)
    {
        scoreManager = scoreMgr;
    }

    public void KillByPlayer()
    {
        if (isDead) return;
        isDead = true;

        if (scoreManager != null)
            scoreManager.AddPoint(1);

        Destroy(gameObject);
    }
}