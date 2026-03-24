using UnityEngine;

public class ClickKiller : MonoBehaviour
{
    private Camera mainCam;

    [Header("Target Layer")]
    public LayerMask cellLayerMask; // selecciona solo "Cell"

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector2 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        // SOLO mata si el click cae dentro de un collider de la layer Cell
        Collider2D col = Physics2D.OverlapPoint(worldPos, cellLayerMask);

        if (col == null) return;

        var cell = col.GetComponent<CellBehaviour>();
        if (cell != null)
        {
            cell.KillByPlayer();
        }
    }
}