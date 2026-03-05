using UnityEngine;
using System.Collections.Generic;

public class LearningEngine : MonoBehaviour
{
    
    private Dictionary<string, Vector2> memory = new Dictionary<string, Vector2>();

    [Header("Configuraciones de ML")]
    public float epsilon = 0.2f; // 20% de probabilidad de probar algo nuevo 

    
    public Color[] palette = { Color.red, Color.blue, Color.green, Color.yellow, Color.black, Color.white };
    public float[] sizes = { 0.5f, 1.0f, 1.5f, 2.0f };

    public struct CellConfig
    {
        public Color color;
        public float size;
        public string key;
    }

    public CellConfig GetBestConfig()
    {
        
        if (Random.value < epsilon || memory.Count == 0)
        {
            return GetRandomConfig();
        }

        //Buscar la configuración con mejor tasa de éxito
        string bestKey = "";
        float bestRate = -1f;

        foreach (var entry in memory)
        {
            float rate = entry.Value.y / entry.Value.x; 
            if (rate > bestRate)
            {
                bestRate = rate;
                bestKey = entry.Key;
            }
        }

        return KeyToConfig(bestKey);
    }

    private CellConfig GetRandomConfig()
    {
        int cIndex = Random.Range(0, palette.Length);
        int sIndex = Random.Range(0, sizes.Length);
        return GenerateConfig(cIndex, sIndex);
    }

    private CellConfig GenerateConfig(int c, int s)
    {
        return new CellConfig
        {
            color = palette[c],
            size = sizes[s],
            key = c + "_" + s
        };
    }

    private CellConfig KeyToConfig(string key)
    {
        string[] split = key.Split('_');
        return GenerateConfig(int.Parse(split[0]), int.Parse(split[1]));
    }

    public void UpdateMemory(string key, bool survived)
    {
        if (!memory.ContainsKey(key)) memory[key] = Vector2.zero;

        Vector2 stats = memory[key];
        stats.x += 1; // Intento +1
        if (survived) stats.y += 1; // Supervivencia +1

        memory[key] = stats;
    }
}
