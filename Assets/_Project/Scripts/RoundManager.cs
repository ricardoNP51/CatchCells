using UnityEngine;

public class RoundManager : MonoBehaviour
{
    [Header("Round Settings")]
    public float roundDuration = 10f;

    [Header("References")]
    public CellSpawner spawner;

    public int CurrentRound { get; private set; } = 1;
    public float TimeLeft { get; private set; }

    private bool roundRunning = false;

    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        if (!roundRunning) return;

        TimeLeft -= Time.deltaTime;

        if (TimeLeft <= 0f)
        {
            EndRound();
            StartNextRound();
        }
    }

    private void StartRound()
    {
        roundRunning = true;
        TimeLeft = roundDuration;

        if (spawner != null)
        {
            spawner.ClearCells();
            spawner.SpawnCellsForRound();
        }
    }

    private void EndRound()
    {
        roundRunning = false;
        // Aquí luego metemos evaluación ML (sobrevivió o murió)
    }

    private void StartNextRound()
    {
        CurrentRound++;
        StartRound();
    }
}