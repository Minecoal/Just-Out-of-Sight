using UnityEngine;

public class ChaserSpawnpoint : MonoBehaviour, ILitable
{
    [SerializeField] private bool spawnOnAwake = false;
    public bool SpawnOnAwake => spawnOnAwake;

    private bool isLit = false;
    private Chaser chaser;

    public void SetChaser(Chaser c)
    {
        chaser = c;
    }

    public void ClearChaser()
    {
        chaser = null;
    }

    public void SetLit(bool lit)
    {
        isLit = lit;
    }

    public bool IsAvailable()
    {
        return !isLit && chaser == null && Vector2.Distance(PlayerManager.Instance.PlayerPosition, transform.position) > 6f;
    }

    public void ForceReset()
    {
        if (chaser == null) return;
        chaser.transform.position = transform.position;
    }
}
