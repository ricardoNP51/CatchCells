using UnityEngine;
using TMPro; // Necesario para cambiar el texto del botón

public class TrainingManager : MonoBehaviour
{
    [Header("References")]
    public RoundManager roundManager;
    public CellSpawner cellSpawner;
    public Transform mouseTarget;
    public TextMeshProUGUI buttonText; // Arrastra el texto de tu botón aquí

    [Header("Training Settings")]
    public bool autoMoveMouse = false;
    public float moveInterval = 0.12f;
    public float minX = -7f;
    public float maxX = 7f;
    public float minY = -4f;
    public float maxY = 4f;
    public float moveSpeed = 20f;

    private Vector2 targetPosition;
    private float nextMoveTime;

    // ESTA FUNCIÓN VA EN EL ON CLICK DEL BOTÓN
    public void ToggleTrainingMode()
    {
        autoMoveMouse = !autoMoveMouse;

        // --- TAREA: DIFICULTAD VISUAL (INDICADOR) ---
        GameObject bg = GameObject.Find("Background");
        if (bg != null)
        {
            // Se oscurece en modo entrenamiento para diferenciarlo
            bg.GetComponent<SpriteRenderer>().color = autoMoveMouse ? new Color(0.2f, 0.2f, 0.2f) : Color.white;
        }

        // --- TAREA: BOTÓN TRAINING/PLAY ---
        if (buttonText != null)
        {
            buttonText.text = autoMoveMouse ? "MODE: TRAINING" : "MODE: PLAY";
        }

        Debug.Log("Alison - Cambio de modo: " + (autoMoveMouse ? "Entrenamiento" : "Juego"));
    }

    private void Start()
    {
        PickNewTarget();
    }

    private void Update()
    {
        if (!autoMoveMouse || mouseTarget == null) return;

        if (Time.time >= nextMoveTime)
        {
            PickNewTarget();
        }

        mouseTarget.position = Vector2.MoveTowards(
            mouseTarget.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    private void PickNewTarget()
    {
        targetPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        nextMoveTime = Time.time + moveInterval;
    }
}