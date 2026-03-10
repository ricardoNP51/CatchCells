using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject cellPrefab;
    public Transform cellsParent;
    public BoxCollider2D spawnArea;
    public ScoreManager scoreManager;
<<<<<<< Updated upstream
=======
    public RoundManager roundManager;
    public Transform mouseTarget;
>>>>>>> Stashed changes

    [Header("Spawn Settings")]
    public int cellsPerRound = 12;

    public void SpawnCellsForRound()
    {
        if (cellPrefab == null || cellsParent == null || spawnArea == null)
        {
            Debug.LogError("CellSpawner: Faltan referencias (prefab/parent/spawnArea).");
            return;
        }

        for (int i = 0; i < cellsPerRound; i++)
        {
            Vector2 pos = GetRandomPointInBounds(spawnArea.bounds);
            GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, cellsParent);

<<<<<<< Updated upstream
            // Inicializar para que al morir sume score
            var behaviour = cell.GetComponent<CellBehaviour>();
            if (behaviour != null && scoreManager != null)
                behaviour.Init(scoreManager);
=======
            var behaviour = cell.GetComponent<CellBehaviour>();
            if (behaviour != null && scoreManager != null)
                behaviour.Init(scoreManager);

            var agent = cell.GetComponent<CellAgent>();
            if (agent != null)
                agent.SetEnvironmentReferences(mouseTarget, roundManager);
>>>>>>> Stashed changes
        }
    }

    public void ClearCells()
    {
        if (cellsParent == null) return;

        for (int i = cellsParent.childCount - 1; i >= 0; i--)
        {
            Destroy(cellsParent.GetChild(i).gameObject);
        }
    }

    private Vector2 GetRandomPointInBounds(Bounds b)
    {
        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);
        return new Vector2(x, y);
    }
}