using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("References")]
    public RoundManager roundManager;
    public CellSpawner cellSpawner;
    public Transform mouseTarget;

    [Header("Training Settings")]
    public bool autoMoveMouse = true;
    public float moveInterval = 0.12f;
    public float minX = -7f;
    public float maxX = 7f;
    public float minY = -4f;
    public float maxY = 4f;
    public float moveSpeed = 20f;

    private Vector2 targetPosition;
    private float nextMoveTime;

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
        targetPosition = new Vector2(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY)
        );

        nextMoveTime = Time.time + moveInterval;
    }
}