using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WallUpdater : MonoBehaviour
{
    private SpriteRenderer sr;
    private ShadowCaster2D shadowCaster;
    private Collider2D collider2D;

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        shadowCaster = GetComponentInChildren<ShadowCaster2D>();
        collider2D = GetComponentInChildren<Collider2D>();
    }

    public void SetTint(float valueFactor)
    {
        if (sr == null) return;
        
        Color c = sr.color;
        c.r = c.r * valueFactor;
        c.g = c.g * valueFactor;
        c.b = c.b * valueFactor;
        sr.color = c;
    }

    public void Clear()
    {
        Destroy(collider2D);
        // Destroy(shadowCaster);
    }

    public void SetCastType()
    {
        shadowCaster.castingOption = ShadowCaster2D.ShadowCastingOptions.NoShadow;
    }

    public void SetRenderOrder(int i)
    {
        sr.rendererPriority = i;
    }
}
