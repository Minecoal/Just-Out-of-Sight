using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FieldOfView : MonoBehaviour {

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Light2D spotLight;
    private Mesh mesh;
    private float fov;
    private float viewDistance;
    private Vector3 origin;
    private float startingAngle;

    private bool isEnabled = true;

    private void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;

        fov = spotLight.pointLightOuterAngle;
        viewDistance = spotLight.pointLightOuterRadius;
    }

    private void LateUpdate() {
        if (!isEnabled) return;

        SetOrigin(PlayerManager.Instance.FlashLightPosition);
        SetAimDirection(PlayerManager.Instance.FlashLightRotation);  

        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++) {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
            if (raycastHit2D.collider == null) {
                // No hit
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;                
            } else {
                // Hit object
                Vector3 dir = GetVectorFromAngle(angle);
                vertex = new Vector3(raycastHit2D.point.x, raycastHit2D.point.y, 0f) + dir * 1f;
            }

           
            vertices[vertexIndex] = vertex;

            if (i > 0) {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    }

    public void SetEnabled(bool value) {
        isEnabled = value;

        if (!value) {
            mesh.Clear();
        }

        GetComponent<MeshRenderer>().enabled = value;
    }

    public void Toggle() {
        SetEnabled(!isEnabled);
    }

    public void SetOrigin(Vector3 origin) {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection) {
        startingAngle = aimDirection.z + 90f + fov / 2f;
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        return angle;
    }

    public static Vector3 GetVectorFromAngle(float angleDeg)
    {
        float angleRad = angleDeg * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0f);
    }
}
