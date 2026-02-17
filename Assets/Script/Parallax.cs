using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public int totalLayers;
    private int factor; // how much this layer moves / darkens

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

    void Start()
    {
        foreach (WallUpdater wall in childWalls)
        {
            wall.SetRenderOrder(factor);
        }
    }

    public void SetFactor(int factor)
    {
        this.factor = factor;
    }

    public void ApplyTint(float maxTint)
    {
        if (childWalls == null) return;
        foreach (WallUpdater wall in childWalls)
        {
            wall.SetTint(factor / (totalLayers - 1f), maxTint);
        }
    }

    public void UpdateLayerPosition(Vector3 playerPosition, float skewAmountx, float skewAmounty, float scaleAmount)
    {
        for (int i = 0; i < childWalls.Length; i++)
        {
            WallUpdater t = childWalls[i];

            t.transform.position = originalLocalPositions[i] + new Vector3( 
                (originalLocalPositions[i].x - playerPosition.x) * factor * skewAmountx,
                (originalLocalPositions[i].y - playerPosition.y) * factor * skewAmounty,
                0f
            );

            t.transform.localScale = originalLocalScales[i] * ((factor * scaleAmount) + 1);
        }
    }
    
    public void Init(UnityEngine.Rendering.Universal.ShadowCaster2D.ShadowCastingOptions type, bool clear)
    {
        foreach (WallUpdater wall in childWalls)
        {
            if (clear) wall.Clear();
            wall.SetCastType(type);
        }
    }
}
