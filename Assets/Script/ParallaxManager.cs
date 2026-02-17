using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : GenericSingleton<ParallaxManager>
{
    [SerializeField] private int parallaxCount = 5;
    [SerializeField] private float distancex = 2f;
    [SerializeField] private float distancey = 2f;
    [SerializeField] float maxScale = 1.3f;
    [SerializeField] float maxTint = 0.7f;
    private float skewAmountx;
    private float skewAmounty;
    private float scaleAmount;

    [SerializeField] GameObject wallPrefab; // assign original wall prefab
    private List<ParallaxLayer> layers = new List<ParallaxLayer>();

    protected override void Awake()
    {
        base.Awake();
        CreateLayers(parallaxCount);
    }

    void Start()
    {
        InitLayers();
    }

    void Update()
    {
        skewAmountx = distancex / ((float)parallaxCount);
        skewAmounty = distancey / ((float)parallaxCount);
        scaleAmount = maxScale / ((float)parallaxCount);
        UpdateLayers(PlayerManager.Instance.PlayerPosition);
    }

    void CreateLayers(int parallaxCount)
    {
        for (int i = 0; i < parallaxCount; i++)
        {
            GameObject layerObj;

            if (i == 0)
            {
                // first layer uses original prefab
                layerObj = wallPrefab;
            }
            else
            {
                layerObj = Instantiate(wallPrefab, wallPrefab.transform.parent);
                
            }

            // attach ParallaxLayer if not already
            ParallaxLayer layer = layerObj.GetComponent<ParallaxLayer>();
            if (layer == null)
            {
                layer = layerObj.AddComponent<ParallaxLayer>();
            }
            layer.totalLayers = parallaxCount;


            layer.SetFactor(i);
            layer.ApplyTint(maxTint);
            layers.Add(layer);
        }
    }

    void InitLayers()
    {
        for (int i = 0; i < parallaxCount; i++)
        {
            if (i == parallaxCount - 1) {
                layers[i].Init(UnityEngine.Rendering.Universal.ShadowCaster2D.ShadowCastingOptions.CastAndSelfShadow, true);
            } else if (i == 0) {
                layers[i].Init(UnityEngine.Rendering.Universal.ShadowCaster2D.ShadowCastingOptions.NoShadow, false);
            } else {
                layers[i].Init(UnityEngine.Rendering.Universal.ShadowCaster2D.ShadowCastingOptions.NoShadow, true);
            }
        }
    }

    void UpdateLayers(Vector3 playerPosition)
    {
        foreach (ParallaxLayer layer in layers)
        {
            layer.UpdateLayerPosition(playerPosition, skewAmountx, skewAmounty, scaleAmount);
        }
    }
}
