using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [Header("References")]
    public RoundManager roundManager;
    public CellSpawner cellSpawner;
    public Transform mouseTarget;

    [Header("Training Settings")]
    public bool autoMoveMouse = true;
    public float mouseMoveRadiusX = 7f;
    public float mouseMoveRadiusY = 4f;
    public float mouseMoveSpeed = 1.5f;

    private Vector2 centerPoint = Vector2.zero;

    private void Update()
    {
        if (!autoMoveMouse || mouseTarget == null) return;

        float x = Mathf.Sin(Time.time * mouseMoveSpeed) * mouseMoveRadiusX;
        float y = Mathf.Cos(Time.time * mouseMoveSpeed * 0.8f) * mouseMoveRadiusY;

        mouseTarget.position = centerPoint + new Vector2(x, y);
    }
}