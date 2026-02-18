using System.Collections.Generic;
using UnityEngine;

public class LightDetector : MonoBehaviour
{
    private readonly Dictionary<ILitable, int> litCounts = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ILitable>(out var lit))
        {
            if (!litCounts.ContainsKey(lit))
                litCounts[lit] = 0;

            litCounts[lit]++;

            if (litCounts[lit] == 1)
            {
                // First collider entered → object becomes lit
                lit.SetLit(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<ILitable>(out var lit) && litCounts.ContainsKey(lit))
        {
            litCounts[lit]--;

            if (litCounts[lit] <= 0)
            {
                lit.SetLit(false);
                litCounts.Remove(lit);
            }
        }
    }
}
