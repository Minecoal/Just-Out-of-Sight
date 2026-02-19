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
        return !isLit && chaser == null;
    }
}
