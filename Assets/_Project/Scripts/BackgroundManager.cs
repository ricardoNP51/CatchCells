using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Color topLeftColor = new Color(0.2f, 0.4f, 0.8f);
    public Color topRightColor = new Color(0.2f, 0.7f, 0.3f);
    public Color bottomLeftColor = new Color(0.8f, 0.3f, 0.3f);
    public Color bottomRightColor = new Color(0.5f, 0.3f, 0.8f);

    public Color GetBackgroundColorAtPosition(Vector2 pos)
    {
        if (pos.x < 0 && pos.y >= 0) return topLeftColor;
        if (pos.x >= 0 && pos.y >= 0) return topRightColor;
        if (pos.x < 0 && pos.y < 0) return bottomLeftColor;
        return bottomRightColor;
    }
}