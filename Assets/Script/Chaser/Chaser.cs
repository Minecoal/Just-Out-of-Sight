using UnityEngine;
using UnityEngine.AI;

public class Chaser : MonoBehaviour, ILitable
{
    [SerializeField] private bool isLit;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float idleTeleportTime = 5f; // time before teleport if idle and unlit
    [SerializeField] private float idleTimer = 0f;

    private enum State { Idle, Moving }
    private State currentState = State.Moving;

    public ChaserSpawnpoint CurrentSpawnpoint { get; set; }

    private Vector3 targetPos;
    private NavMeshAgent agent;

    public void SetLit(bool lit)
    {
        if (isLit == lit) return;

        isLit = lit;

        if (isLit)
        {
            idleTimer = 0f; // reset idle timer when lit
            currentState = State.Moving;
        }
    }

    void Start()
    {
        isLit = false;

        TextDisplayManager.New3D(Vector3.zero, 0.1f)
            .WithParent(transform)
            .WithTrackedProvider(() => $"{isLit}")
            .Build();
        TextDisplayManager.New3D(new Vector3(0,-0.2f,0), 0.1f)
            .WithParent(transform)
            .WithTrackedProvider(() => $"{currentState.ToString()}")
            .Build();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = chaseSpeed;
    }

    void Update()
    {
        if (isLit)
        {
            // stay in place if lit
            agent.SetDestination(transform.position);
            idleTimer = 0f;
            currentState = State.Idle;
            return;
        }

        targetPos = PlayerManager.Instance.PlayerPosition;

        // check if a path exists to the player
        NavMeshPath path = new NavMeshPath();
        bool canPathfind = agent.CalculatePath(targetPos, path) && path.status == NavMeshPathStatus.PathComplete;

        if (canPathfind)
        {
            currentState = State.Moving;
            agent.speed = chaseSpeed;
            agent.SetDestination(targetPos);
            idleTimer = 0f; // reset idle timer while moving
        }
        else
        {
            currentState = State.Idle;
            agent.SetDestination(transform.position); // stop movement
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleTeleportTime)
            {
                if (ChaserManager.Instance.TryGetAvailableResetPosition(out Vector3 newPos, out ChaserSpawnpoint newSpawn))
                {
                    CurrentSpawnpoint?.ClearChaser();

                    transform.position = newPos;
                    agent.Warp(newPos);
                    newSpawn.SetChaser(this);

                    CurrentSpawnpoint = newSpawn;

                    idleTimer = 0f;
                    currentState = State.Moving;
                }
            }
        }
    }
}
