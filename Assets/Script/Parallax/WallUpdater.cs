using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class WallUpdater : MonoBehaviour
{
    private SpriteRenderer sr;
    private ShadowCaster2D shadowCaster;
    private Collider2D[] collider2d;
    private NavMeshObstacle[] obstacle;
    Color baseColor;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        shadowCaster = GetComponentInChildren<ShadowCaster2D>();
        collider2d = GetComponentsInChildren<Collider2D>();
        obstacle = GetComponentsInChildren<NavMeshObstacle>();
        baseColor = sr.color;
    }

    public void SetTint(float valueFactor, float maxTint)
    {
        if (sr == null) return;
        
        Color c = sr.color;
        c.r = Mathf.Lerp(baseColor.r, 1 - maxTint, valueFactor);
        c.g = Mathf.Lerp(baseColor.g, 1 - maxTint, valueFactor);
        c.b = Mathf.Lerp(baseColor.b, 1 - maxTint, valueFactor);
        sr.color = c;
    }

    public void Clear()
    {
        for (int i = 0; i < collider2d.Length; i++)
        {
            Destroy(collider2d[i]);
        }
        for (int i = 0; i < obstacle.Length; i++)
        {
            Destroy(obstacle[i]);
        }
        
    }

    public void SetCastType(ShadowCaster2D.ShadowCastingOptions type)
    {
        shadowCaster.castingOption = type;
    }

    public void SetRenderOrder(int i)
    {
        sr.sortingOrder = i;
    }
}
