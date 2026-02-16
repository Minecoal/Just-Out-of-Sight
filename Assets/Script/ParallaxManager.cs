using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : GenericSingleton<ParallaxManager>
{
    [SerializeField] private int parallaxCount = 5;
    [SerializeField] private float distance = 2f;
    [SerializeField] float maxScale = 1.3f;
    private float skewAmount;
    private float scaleAmount;

    [SerializeField] GameObject wallPrefab; // assign original wall prefab
    private List<ParallaxLayer> layers = new List<ParallaxLayer>();

    protected override void Awake()
    {
        base.Awake();
        CreateLayers(parallaxCount);
    }

    void Update()
    {
        skewAmount = distance / ((float)parallaxCount);
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
            if (i != 0) layer.Clear();

            layer.SetFactor(i);
            layer.ApplyTint();
            layers.Add(layer);
        }
    }

    void UpdateLayers(Vector3 playerPosition)
    {
        foreach (ParallaxLayer layer in layers)
        {
            layer.UpdateLayerPosition(playerPosition, skewAmount, scaleAmount);
        }
    }
}
