using System.Collections.Generic;
using UnityEngine;

public class LightDetector : MonoBehaviour
{
    // Global counter shared across all detectors
    private static readonly Dictionary<ILitable, int> globalLitCounts = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        var lit = other.GetComponentInParent<MonoBehaviour>() as ILitable;
        if (lit == null) return;

        if (!globalLitCounts.ContainsKey(lit))
            globalLitCounts[lit] = 0;

        globalLitCounts[lit]++;

        if (globalLitCounts[lit] == 1)
            lit.SetLit(true); // first detector lighting this object
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var lit = other.GetComponentInParent<MonoBehaviour>() as ILitable;
        if (lit == null || !globalLitCounts.ContainsKey(lit)) return;

        globalLitCounts[lit]--;

        if (globalLitCounts[lit] <= 0)
        {
            globalLitCounts.Remove(lit);
            lit.SetLit(false); // no detectors left → turn off
        }
    }

    private void OnEnable()
    {
        // Handle objects already overlapping this trigger
        var trigger = GetComponent<Collider2D>();
        if (trigger == null) return;

        ContactFilter2D filter = new ContactFilter2D
        {
            useTriggers = true,
            useLayerMask = false
        };

        List<Collider2D> results = new List<Collider2D>();
        trigger.Overlap(filter, results);

        foreach (var col in results)
        {
            var lit = col.GetComponentInParent<MonoBehaviour>() as ILitable;
            if (lit == null) continue;

            if (!globalLitCounts.ContainsKey(lit))
                globalLitCounts[lit] = 0;

            globalLitCounts[lit]++;

            if (globalLitCounts[lit] == 1)
                lit.SetLit(true);
        }
    }

    private void OnDisable()
    {
        // Cleanup: remove all references for objects still lit by this detector
        var trigger = GetComponent<Collider2D>();
        if (trigger == null) return;

        ContactFilter2D filter = new ContactFilter2D
        {
            useTriggers = true,
            useLayerMask = false
        };

        List<Collider2D> results = new List<Collider2D>();
        trigger.Overlap(filter, results);

        foreach (var col in results)
        {
            var lit = col.GetComponentInParent<MonoBehaviour>() as ILitable;
            if (lit == null || !globalLitCounts.ContainsKey(lit)) continue;

            globalLitCounts[lit]--;

            if (globalLitCounts[lit] <= 0)
            {
                globalLitCounts.Remove(lit);
                lit.SetLit(false);
            }
        }
    }
}