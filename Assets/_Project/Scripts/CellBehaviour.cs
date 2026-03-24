using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    private bool isDead = false;
    private ScoreManager scoreManager;
    private CellAgent cellAgent;
    private Transform mouseTarget;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Visual Difficulty Settings")]
    public float pulseSpeed = 5f;
    public float pulseIntensity = 0.1f;
    public bool showOutline = true; // Opcion visible en Inspector
    public float outlineThickness = 1.15f; // Grosor del contorno negro
    private Vector3 baseScale;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10; // Asegurarse de que se rendericen por delante del fondo

        // --- MEJORA DE DIFICULTAD VISUAL (ALISON) ---

        // 1. Color y Transparencia aleatoria (Camuflaje)
        // El cuarto número (Random.Range(0.3f, 0.8f)) las hace semi-transparentes
        spriteRenderer.color = new Color(Random.value, Random.value, Random.value, Random.Range(0.4f, 0.8f));

        // 2. Tamaño aleatorio (Unas mini y otras grandes)
        float randomSize = Random.Range(0.5f, 1.5f);
        transform.localScale = new Vector3(randomSize, randomSize, 1f);
        baseScale = transform.localScale;

        // --- SOLUCION AL CAMUFLAJE PERFECTO ---
        if (showOutline)
        {
            GameObject outlineObj = new GameObject("Outline");
            outlineObj.transform.SetParent(transform);
            outlineObj.transform.localPosition = Vector3.zero;
            outlineObj.transform.localScale = Vector3.one * outlineThickness;
            
            SpriteRenderer outlineSR = outlineObj.AddComponent<SpriteRenderer>();
            outlineSR.sprite = spriteRenderer.sprite;
            outlineSR.color = new Color(0f, 0f, 0f, 0.45f); // Contorno negro semi-transparente
            outlineSR.sortingOrder = 9; // Se dibuja detrás de la célula (10), pero delante del fondo (0)
        }

        cellAgent = GetComponent<CellAgent>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Animación de pulso
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
        transform.localScale = baseScale * pulse;

        // 3. EFECTO EXTRA: Parpadeo suave (Dificulta el enfoque)
        float alpha = 0.5f + Mathf.PingPong(Time.time, 0.5f);
        Color c = spriteRenderer.color;
        spriteRenderer.color = new Color(c.r, c.g, c.b, alpha);

        EscapeFromMouse();
    }

    private void EscapeFromMouse()
    {
        if (mouseTarget == null || rb == null) return;
        float sqrDistance = ((Vector2)transform.position - (Vector2)mouseTarget.position).sqrMagnitude;
        if (sqrDistance < 3.24f)
        {
            Vector2 dir = ((Vector2)transform.position - (Vector2)mouseTarget.position).normalized;
            rb.linearVelocity = dir * 4.5f;
        }
        else rb.linearVelocity = Vector2.zero;
    }

    public void Init(ScoreManager scoreMgr, Transform mouse) 
    { 
        scoreManager = scoreMgr; 
        mouseTarget = mouse;
    }

    public void KillByPlayer()
    {
        if (isDead) return;
        isDead = true;
        if (scoreManager != null) scoreManager.AddPoint(1);
        if (cellAgent != null) cellAgent.OnKilledByPlayer();
        Destroy(gameObject);
    }
}