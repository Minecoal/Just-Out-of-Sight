using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DroppedItemLightDissipation : MonoBehaviour
{
    private Light2D light2D;
    private float originalIntensity;

    [SerializeField] float minIntensity = 0f;
    [SerializeField] float maxDistance = 5f;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
        originalIntensity = light2D.intensity;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, PlayerManager.Instance.PlayerPosition);

        float t = Mathf.Clamp01(distance / maxDistance); // 0 = close, 1 = far
        light2D.intensity = Mathf.Lerp(originalIntensity, minIntensity, t);
    }
}
