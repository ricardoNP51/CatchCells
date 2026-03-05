using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject cellPrefab;
    public Transform cellsParent;
    public BoxCollider2D spawnArea;
    public ScoreManager scoreManager;

 
    public LearningEngine learningEngine; //Referencia al motor de aprendizaje

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
            var config = learningEngine.GetBestConfig();

            Vector2 pos = GetRandomPointInBounds(spawnArea.bounds);
            GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity, cellsParent);

            // aplica color y tamaño
            cell.GetComponent<SpriteRenderer>().color = config.color;
            cell.transform.localScale = Vector3.one * config.size;
          
            var behaviour = cell.GetComponent<CellBehaviour>();
            if (behaviour != null && scoreManager != null)
                // se pasa la llave y el motor al inicializar 
                behaviour.Init(scoreManager, config.key, learningEngine);
        }  

    }

    public void ClearCells()
    {
        if (cellsParent == null) return;

        // ntes de borrar, las que siguen vivas cuentan como exito
        foreach (Transform child in cellsParent)
        {
            var cb = child.GetComponent<CellBehaviour>();
            if (cb != null) learningEngine.UpdateMemory(cb.configKey, true);
        }
        
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