using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class CellAgent : Agent
{
    [Header("Mouse Danger")]
    public float mouseDangerRadius = 0.8f;
    public float mouseKillPenalty = -1.5f;

    public BackgroundManager backgroundManager;
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
    public float outOfBoundsPenalty = -0.5f;

    public float closeMouseDistance = 1.2f;
    public float closeToMousePenalty = -0.03f;
    public float moveAwayReward = 0.02f;
    public float moveTowardPenalty = -0.02f;
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
    new Color(0.2f, 0.4f, 0.8f),
    new Color(0.15f, 0.35f, 0.7f),
    new Color(0.3f, 0.5f, 0.9f),

    new Color(0.2f, 0.7f, 0.3f),
    new Color(0.1f, 0.55f, 0.2f),
    new Color(0.35f, 0.8f, 0.4f),

    new Color(0.8f, 0.3f, 0.3f),
    new Color(0.65f, 0.2f, 0.2f),
    new Color(0.9f, 0.45f, 0.45f),

    new Color(0.5f, 0.3f, 0.8f),
    new Color(0.4f, 0.2f, 0.65f),
    new Color(0.65f, 0.4f, 0.9f)
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

    public void SetEnvironmentReferences(Transform mouse, RoundManager rm, BackgroundManager bg)
    {
        mouseTarget = mouse;
        roundManager = rm;
        backgroundManager = bg;
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
        Color bgColor = Color.black;
        if (backgroundManager != null)
            bgColor = backgroundManager.GetBackgroundColorAtPosition(transform.position);

        sensor.AddObservation(bgColor.r);
        sensor.AddObservation(bgColor.g);
        sensor.AddObservation(bgColor.b);

        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(transform.position.y);

        float mouseDistance = 10f;
        if (mouseTarget != null)
            mouseDistance = Vector2.Distance(transform.position, mouseTarget.position);

        sensor.AddObservation(mouseDistance);

        float normalizedTime = 0f;
        if (roundManager != null && roundManager.roundDuration > 0f)
            normalizedTime = roundManager.TimeLeft / roundManager.roundDuration;

        sensor.AddObservation(normalizedTime);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (isDead) return;

        int colorAction = actions.DiscreteActions[0];
        int sizeAction = actions.DiscreteActions[1];

        if (colorAction >= 0 && colorAction < colorPalette.Length)
            spriteRenderer.color = colorPalette[colorAction];

        if (sizeAction >= 0 && sizeAction < sizeLevels.Length)
            transform.localScale = Vector3.one * sizeLevels[sizeAction];

        AddReward(aliveRewardPerStep);

        if (backgroundManager != null)
        {
            Color bgColor = backgroundManager.GetBackgroundColorAtPosition(transform.position);
            Color cellColor = spriteRenderer.color;

            float contrast =
                Mathf.Abs(cellColor.r - bgColor.r) +
                Mathf.Abs(cellColor.g - bgColor.g) +
                Mathf.Abs(cellColor.b - bgColor.b);

            AddReward(-contrast * 0.004f);
        }

        if (mouseTarget != null)
        {
            float sqrDistance = ((Vector2)transform.position - (Vector2)mouseTarget.position).sqrMagnitude;

            if (sqrDistance > 2.25f)
                AddReward(0.002f);
            else
                AddReward(-0.002f);
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
        discreteActions[0] = 1;
        discreteActions[1] = 1;
        discreteActions[2] = 0;
        discreteActions[3] = 4;
    }
}