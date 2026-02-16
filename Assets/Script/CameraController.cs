using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private float smoothing;
    [SerializeField] private Vector3 offset;

    [SerializeField] private float xMargin;
    [SerializeField] private float yMargin;
    [SerializeField] private float zMargin;

    [Header("Camera Shake")]
    private float shakeTime;
    private float currentShakeMagnitude;
    [SerializeField] private float intensity = 10f;
    private float shakeSeed;

    void Awake()
    {
        if (target == null) Debug.LogError("camera missing target");
        shakeTime = 0f;
        shakeSeed = Random.value * 100;
    }

    void FixedUpdate()
    {
        if (transform.position != target.position)
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x + offset.x, target.transform.position.y + offset.y, target.transform.position.z + offset.z);
            
            if (shakeTime > 0){
                targetPosition += ApplyShake();
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing);
        }
    }

    private Vector3 ApplyShake(){
        shakeTime -= Time.deltaTime;
        float shakeX = (Mathf.PerlinNoise(shakeSeed + Time.time * intensity, 0) - 0.5f) * 2 * currentShakeMagnitude;
        float shakeZ = (Mathf.PerlinNoise(shakeSeed + Time.time * intensity, 1) - 0.5f) * 2 * currentShakeMagnitude;
        Vector3 shakeOffset = new Vector3(shakeX, 0, shakeZ);
        return shakeOffset;
    }

    public void ScreenShake(float magnitude, float duration)
    {
        currentShakeMagnitude = magnitude;
        shakeTime = duration;
    }
}
