using UnityEngine;

//ENSURE THERE ARE MORE SPAWNPOINTS THAN CHASERS --- VERY IMPORTANT
public class ChaserManager : GenericSingleton<ChaserManager>
{
    [SerializeField] ChaserSpawnpoint[] spawnpoints;
    [SerializeField] GameObject pfChaser;
    [SerializeField] Transform chaserContainer;

    override protected void Awake()
    {
        base.Awake();

        foreach(ChaserSpawnpoint sp in spawnpoints)
        {
            if (!sp.SpawnOnAwake) continue;
            GameObject chaserGO = Instantiate(pfChaser, sp.transform.position, Quaternion.identity);
            if (chaserContainer != null) chaserGO.transform.SetParent(chaserContainer);
            Chaser chaser = chaserGO.GetComponent<Chaser>();
            sp.SetChaser(chaser);
            chaser.CurrentSpawnpoint = sp;
        }
    }

    //randoma
    public bool TryGetAvailableResetPosition(out Vector3 position, out ChaserSpawnpoint spawnpoint)
    {
        var available = new System.Collections.Generic.List<ChaserSpawnpoint>();
        foreach (var sp in spawnpoints)
        {
            if (sp.IsAvailable())
                available.Add(sp);
        }

        if (available.Count == 0){
            position = Vector3.zero;
            spawnpoint = null;
            return false;  
        } 

        spawnpoint = available[Random.Range(0, available.Count)];
        position = spawnpoint.transform.position;

        return true;
    }

    public bool TryGetAvailableResetPosition(out Vector3 position)
    {
        return TryGetAvailableResetPosition(out position, out _);
    }

    public void ForceResetAllChasers()
    {
        foreach(ChaserSpawnpoint sp in spawnpoints)
        {
            sp.ForceReset();
        }
    }
}
