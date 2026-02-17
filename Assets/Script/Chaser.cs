using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour, ILitable
{
    [SerializeField] private bool isLit;
    [SerializeField] private float chaseSpeed = 1f;
    private Vector3 targetPos;
    NavMeshAgent agent;

    public void SetLit(bool lit)
    {
        if (isLit == lit) return;

        isLit = lit;
    }

    void Start()
    {
        isLit = false;
        TextDisplayManager.New3D(Vector3.zero, 0.1f).WithParent(transform).WithTrackedProvider(() => $"{isLit}").Build();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        targetPos = PlayerManager.Instance.PlayerPosition;
        agent.speed = chaseSpeed;
        agent.SetDestination(targetPos);
    }
}
