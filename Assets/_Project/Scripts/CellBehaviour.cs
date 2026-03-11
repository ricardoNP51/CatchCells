using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    private bool isDead = false;
    private ScoreManager scoreManager;
    private CellAgent cellAgent;
    private Transform mouseTarget;
    private Rigidbody2D rb;

    [Header("Pulse Animation Settings")]
    public float pulseSpeed = 5f;
    public float pulseIntensity = 0.1f;

    [Header("Escape Settings")]
    public float escapeRadius = 1.8f;
    public float escapeSpeed = 4.5f;

    private Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;
        cellAgent = GetComponent<CellAgent>();
        rb = GetComponent<Rigidbody2D>();

        GameObject mouseObj = GameObject.Find("MouseTarget");
        if (mouseObj != null)
            mouseTarget = mouseObj.transform;
    }

    private void Update()
    {
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
        transform.localScale = baseScale * pulse;

        EscapeFromMouse();
    }

    private void EscapeFromMouse()
    {
        if (mouseTarget == null || rb == null) return;

        float distance = Vector2.Distance(transform.position, mouseTarget.position);

        if (distance < escapeRadius)
        {
            Vector2 dir = ((Vector2)transform.position - (Vector2)mouseTarget.position).normalized;
            rb.linearVelocity = dir * escapeSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

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

        if (cellAgent != null)
            cellAgent.OnKilledByPlayer();

        Destroy(gameObject);
    }
}