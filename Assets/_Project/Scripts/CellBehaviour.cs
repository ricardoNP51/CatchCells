using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    private bool isDead = false;
    private ScoreManager scoreManager;
    
    public string configKey;
    private LearningEngine learningEngine;

    // Variables para la animación de pulso ---
    [Header("Pulse Animation Settings")]
    public float pulseSpeed = 5f;       // Velocidad del latido
    public float pulseIntensity = 0.1f;  // Qué tanto se agranda
    private Vector3 originalScale;       // Guarda el tamaño inicial
    private float pulseTimer;            // Contador interno para el movimiento
    

    public void Init(ScoreManager scoreMgr, string key, LearningEngine engine)
    {
        scoreManager = scoreMgr;
        configKey = key;
        learningEngine = engine;

        originalScale = transform.localScale;
        pulseTimer = Random.Range(0f, 5f); // Para que no todas pulsen al mismo tiempo
        
    }
    private void Update()
    {
        if (isDead) return;

        // Lógica de la tarea "Cell Pulse Animation"
        pulseTimer += Time.deltaTime * pulseSpeed;
        float scaleOffset = Mathf.Sin(pulseTimer) * pulseIntensity;
        transform.localScale = originalScale + new Vector3(scaleOffset, scaleOffset, 0);
    }

    public void KillByPlayer()
    {
        if (isDead) return;
        isDead = true;

         if (learningEngine != null) learningEngine.UpdateMemory(configKey, false);    // avisamos que esta config NO sobrevivió


        if (scoreManager != null)
            scoreManager.AddPoint(1);

        Destroy(gameObject);
    }

}