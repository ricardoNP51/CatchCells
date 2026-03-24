using UnityEngine;
using TMPro; // Needed for UI texts

public class RoundManager : MonoBehaviour
{
    [Header("Round Settings")]
    public float roundDuration = 10f;

    [Header("References")]
    public CellSpawner spawner;
    public Transform cellsParent;

    public int CurrentRound { get; private set; } = 1;
    public float TimeLeft { get; private set; }

    private bool roundRunning = false;
    private TextMeshProUGUI timeTextUI;
    private TextMeshProUGUI enemiesTextUI;

    private int lastDisplayedRound = -1;
    private int lastDisplayedTime = -1;
    private int lastChildCount = -1;

    private void Awake()
    {
        // Automatically find and setup the UI texts (those named "New Text")
        TextMeshProUGUI[] allTexts = FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
        foreach (var t in allTexts)
        {
            if (t.text == "New Text" || t.text.StartsWith("TIME") || t.text.StartsWith("CELLS"))
            {
                if (timeTextUI == null)
                {
                    timeTextUI = t;
                    SetupUIFormat(timeTextUI, new Vector2(0, 1), new Vector2(0, 1), new Vector2(30, -30), TextAlignmentOptions.TopLeft);
                }
                else if (enemiesTextUI == null)
                {
                    enemiesTextUI = t;
                    SetupUIFormat(enemiesTextUI, new Vector2(1, 1), new Vector2(1, 1), new Vector2(-30, -30), TextAlignmentOptions.TopRight);
                }
            }
        }
    }

    private void SetupUIFormat(TextMeshProUGUI t, Vector2 anchor, Vector2 pivot, Vector2 pos, TextAlignmentOptions align)
    {
        t.alignment = align;
        t.fontSize = 36;
        t.color = Color.white;
        t.fontStyle = FontStyles.Bold;
        t.enableWordWrapping = false;
        
        RectTransform rt = t.GetComponent<RectTransform>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot = pivot;
        rt.anchoredPosition = pos;
        rt.sizeDelta = new Vector2(400, 100);
    }

    private void Start()
    {
        StartRound();
    }

    private void Update()
    {
        int totalTimeInt = Mathf.RoundToInt(Mathf.Max(0, TimeLeft) * 10f);
        if (timeTextUI != null && (totalTimeInt != lastDisplayedTime || CurrentRound != lastDisplayedRound))
        {
            lastDisplayedTime = totalTimeInt;
            lastDisplayedRound = CurrentRound;
            timeTextUI.text = $"TIME: {Mathf.Max(0, TimeLeft):F1}s\nROUND: {CurrentRound}";
        }
        
        if (enemiesTextUI != null && cellsParent != null && cellsParent.childCount != lastChildCount)
        {
            lastChildCount = cellsParent.childCount;
            enemiesTextUI.text = $"CELLS ALIVE: {cellsParent.childCount}";
        }

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

        if (cellsParent != null)
        {
            for (int i = 0; i < cellsParent.childCount; i++)
            {
                CellAgent agent = cellsParent.GetChild(i).GetComponent<CellAgent>();
                if (agent != null)
                {
                    agent.OnSurvivedRound();
                }
            }
        }
    }

    private void StartNextRound()
    {
        CurrentRound++;
        StartRound();
    }
}