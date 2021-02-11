using UnityEngine;
using UnityEditor;

public abstract class ARandomGenerator_Inspector : Editor
{
    public static void DrawPercents<T>(T[] weightPool, int totalWeight)
        where T : AWeightedProbability
    {
        GUILayout.Label("Percents %:");
        for (var i = 0; i < weightPool.Length; ++i)
        {
            var entry = weightPool[i];
            var weight = entry.Weight;

            GUILayout.Label(string.Format("#{0}: \t{1}/{2}\t{3}%",
                i, weight, totalWeight, (float)weight / (float)totalWeight * 100));
        }
    }
}
