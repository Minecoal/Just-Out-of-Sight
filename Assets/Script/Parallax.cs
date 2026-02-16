using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float totalLayers;
    private float factor; // how much this layer moves / darkens

    [SerializeField] private WallUpdater[] childWalls;
    private Vector3 originalPosition;
    private List<Vector3> originalWallPositions;

    void Awake()
    {
        originalPosition = transform.localPosition;
        childWalls = GetComponentsInChildren<WallUpdater>();

        originalWallPositions = new List<Vector3>();

        foreach (WallUpdater wall in childWalls)
        {
            originalWallPositions.Add(wall.transform.position);
        }
    }

    public void SetFactor(int factor)
    {
        this.factor = factor;
    }

    public void ApplyTint()
    {
        if (childWalls == null) return;
        foreach (WallUpdater wall in childWalls)
        {
            wall.SetTint(1 - (factor / (totalLayers - 1f)));
        }
    }

    public void UpdateLayerPosition(Vector3 playerPosition, float skewAmount)
    {
        for (int i = 0; i < childWalls.Length; i++)
        {
            WallUpdater t = childWalls[i];

            t.transform.position = originalWallPositions[i] + new Vector3( 
                (originalWallPositions[i].x - playerPosition.x) * factor * skewAmount,
                (originalWallPositions[i].y - playerPosition.y) * factor * skewAmount,
                0f
            );
        }
    }
    
    public void Clear()
    {
        foreach (WallUpdater wall in childWalls)
        {
            wall.Clear();
        }
    }
}
