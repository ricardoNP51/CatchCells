using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CellAgent : Agent
{
    [Header("Cell Settings")]
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rb;

    [Header("Environment References")]
    public Transform mouseTarget;
    public Camera mainCamera;
    public RoundManager roundManager;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Rewards")]
    public float aliveRewardPerStep = 0.001f;
    public float surviveRoundReward = 1.0f;
    public float deathPenalty = -1.0f;
    public float closeToMousePenalty = -0.002f;
    public float closeMouseDistance = 1.5f;
    public float outOfBoundsPenalty = -0.5f;
    public float moveAwayReward = 0.002f;
    public float moveTowardPenalty = -0.002f;

    [Header("Bounds")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4.5f;
    public float maxY = 4.5f;

    private Vector2 startPosition;
    private bool isDead = false;
    private float previousMouseDistance = 10f;

    private readonly float[] sizeLevels = { 0.4f, 0.55f, 0.7f, 0.85f, 1.0f, 1.2f };

    private readonly Color[] colorPalette =
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.cyan,
        Color.magenta,
        new Color(1f, 0.5f, 0f),   // naranja
        new Color(0.5f, 0f, 1f),   // morado
        new Color(1f, 0.4f, 0.7f), // rosa
        new Color(0f, 0.4f, 0f),   // verde oscuro
        new Color(0f, 0f, 0.4f),   // azul oscuro
        Color.gray
    };

    public override void Initialize()
    {
        startPosition = transform.position;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void SetEnvironmentReferences(Transform mouse, RoundManager rm)
    {
        mouseTarget = mouse;
        roundManager = rm;
    }

    public override void OnEpisodeBegin()
    {
        isDead = false;
        transform.position = startPosition;
        rb.linearVelocity = Vector2.zero;
        previousMouseDistance = 10f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 1-2: posición
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);

        // 3-4: velocidad
        sensor.AddObservation(rb.linearVelocity.x);
        sensor.AddObservation(rb.linearVelocity.y);

        // 5: tamaño actual
        sensor.AddObservation(transform.localScale.x);

        // 6-7-8: mouse relativo
        Vector2 mouseDelta = Vector2.zero;
        float mouseDistance = 10f; // lejos por defecto

        if (mouseTarget != null)
        {
            mouseDelta = (Vector2)(mouseTarget.position - transform.position);
            mouseDistance = mouseDelta.magnitude;
        }

        sensor.AddObservation(mouseDelta.x);
        sensor.AddObservation(mouseDelta.y);
        sensor.AddObservation(mouseDistance);

        // 9: tiempo restante normalizado
        float normalizedTime = 0f;
        if (roundManager != null && roundManager.roundDuration > 0f)
            normalizedTime = roundManager.TimeLeft / roundManager.roundDuration;

        sensor.AddObservation(normalizedTime);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isDead) return;

        int moveXAction = actions.DiscreteActions[0];
        int moveYAction = actions.DiscreteActions[1];
        int colorAction = actions.DiscreteActions[2];
        int sizeAction = actions.DiscreteActions[3];

        float moveX = 0f;
        float moveY = 0f;

        switch (moveXAction)
        {
            case 0: moveX = -1f; break;
            case 1: moveX = 0f; break;
            case 2: moveX = 1f; break;
        }

        switch (moveYAction)
        {
            case 0: moveY = -1f; break;
            case 1: moveY = 0f; break;
            case 2: moveY = 1f; break;
        }

        Vector2 movement = new Vector2(moveX, moveY).normalized;
        rb.linearVelocity = movement * moveSpeed;

        if (colorAction >= 0 && colorAction < colorPalette.Length)
            spriteRenderer.color = colorPalette[colorAction];

        if (sizeAction >= 0 && sizeAction < sizeLevels.Length)
            transform.localScale = Vector3.one * sizeLevels[sizeAction];

        AddReward(aliveRewardPerStep);

        if (mouseTarget != null)
        {
            float distance = Vector2.Distance(transform.position, mouseTarget.position);

            if (distance < closeMouseDistance)
                AddReward(closeToMousePenalty);

            if (distance > previousMouseDistance)
                AddReward(moveAwayReward);
            else if (distance < previousMouseDistance)
                AddReward(moveTowardPenalty);

            previousMouseDistance = distance;
        }

        if (transform.position.x < minX || transform.position.x > maxX ||
            transform.position.y < minY || transform.position.y > maxY)
        {
            AddReward(outOfBoundsPenalty);
            EndEpisode();
        }
    }

    public void OnKilledByPlayer()
    {
        if (isDead) return;

        isDead = true;
        AddReward(deathPenalty);
        EndEpisode();
    }

    public void OnSurvivedRound()
    {
        if (isDead) return;

        AddReward(surviveRoundReward);
        EndEpisode();
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = 1; // quieto horizontal
        discreteActions[1] = 1; // quieto vertical
        discreteActions[2] = 0; // color por defecto
        discreteActions[3] = 4; // tamaño medio
    }
}