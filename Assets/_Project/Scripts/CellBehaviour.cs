using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    private bool isDead = false;
    private ScoreManager scoreManager;
<<<<<<< Updated upstream

    public void Init(ScoreManager scoreMgr)
    {
=======
    private CellAgent cellAgent;

    [Header("Pulse Animation Settings")]
    public float pulseSpeed = 5f;
    public float pulseIntensity = 0.1f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
        cellAgent = GetComponent<CellAgent>();
    }

    private void Update()
    {
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
        transform.localScale = originalScale * pulse;
    }

    public void Init(ScoreManager scoreMgr)
    {
>>>>>>> Stashed changes
        scoreManager = scoreMgr;
    }

    public void KillByPlayer()
    {
        if (isDead) return;
        isDead = true;

        if (scoreManager != null)
            scoreManager.AddPoint(1);

        if (cellAgent != null)
            cellAgent.OnKilledByPlayer();

        Destroy(gameObject);
    }
}