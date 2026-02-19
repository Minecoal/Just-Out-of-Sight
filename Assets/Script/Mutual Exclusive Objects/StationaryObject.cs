using UnityEngine;

public class StationaryObject : MonoBehaviour, ILitable
{
    [SerializeField] private GameObject sprite;
    private bool isLit;
    private bool isVisible;
    public bool IsLit => isLit;
    public bool IsVisible => isVisible;

    void Awake()
    {
        isLit = false;
        isVisible = true;
        // TextDisplayManager.New3D(Vector3.zero, 0.1f).WithParent(transform).WithTrackedProvider(() => $"{isLit}").Build();
    }

    public void SetLit(bool lit)
    {
        isLit = lit;
    }

    public void SetVisibility(bool vis)
    {
        isVisible = vis;
        sprite.SetActive(vis);
    }
}
