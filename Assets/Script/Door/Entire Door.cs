using UnityEngine;
using UnityEngine.AI;

public class EntireDoor : MonoBehaviour,  ILitable
{
    [SerializeField] private Door door1;
    [SerializeField] private Door door2;
    private NavMeshObstacle obstacle;

    void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
    }

    void Update()
    {
        obstacle.carving = !(door1.State == DoorState.Open && door2.State == DoorState.Open);
    }

    public void SetLit(bool lit)
    {
        door1.SetLit(lit);
        door2.SetLit(lit);
    }
}
