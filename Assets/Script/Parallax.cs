using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float totalLayers;
    private float factor; // how much this layer moves / darkens

    [SerializeField] private WallUpdater[] childWalls;
    private Vector3 originalPosition;
    private List<Vector3> originalLocalPositions;
    private List<Vector3> originalLocalScales;

    void Awake()
{
    childWalls = GetComponentsInChildren<WallUpdater>();

    originalLocalPositions = new List<Vector3>();
    originalLocalScales = new List<Vector3>();

    foreach (WallUpdater wall in childWalls)
    {
        originalLocalPositions.Add(wall.transform.localPosition);
        originalLocalScales.Add(wall.transform.localScale);
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

    public void UpdateLayerPosition(Vector3 playerPosition, float skewAmount, float scaleAmount)
    {
        for (int i = 0; i < childWalls.Length; i++)
        {
            WallUpdater t = childWalls[i];

            t.transform.position = originalLocalPositions[i] + new Vector3( 
                (originalLocalPositions[i].x - playerPosition.x) * factor * skewAmount,
                (originalLocalPositions[i].y - playerPosition.y) * factor * skewAmount,
                0f
            );

            t.transform.localScale = originalLocalScales[i] * ((factor * scaleAmount) + 1);
        }
    }
    
    public void Clear()
    {
        int i = 0;
        foreach (WallUpdater wall in childWalls)
        {
            wall.SetRenderOrder(-i);
            wall.Clear();
            wall.SetCastType();
            i++;
        }
    }
}
