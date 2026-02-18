using UnityEngine;
using System.Collections.Generic;

public class Door : MonoBehaviour, IInteractable
{
    [Header("OneWaySetting")]
    [SerializeField] private bool isOneWay;
    [SerializeField] private Door connectedDoor;

    [Header("Visuals")]
    [SerializeField] private GameObject closeGO;
    [SerializeField] private GameObject openGO;
    [SerializeField] private GameObject invisibleWall;
    [SerializeField] private GameObject topFrame;

    [Header("Visual Fixes")]
    [SerializeField] private Transform anchor;
    [SerializeField] private List<SpriteRenderer> allVisuals;

    [Header("Interaction")]
    [SerializeField] private Collider2D interactionCollider;

    private bool isLit;
    private DoorState state;

    void Awake()
    {
        SetState(DoorState.Closed);
        TextDisplayManager.New3D(Vector3.zero, 0.1f).WithParent(transform).WithTrackedProvider(() => $"{isLit}").Build();
    }

    void Update()
    {
        // visual hiding
        bool hideVisuals = anchor.localScale.x <= 0f || anchor.localScale.y <= 0f;
        foreach (SpriteRenderer sr in allVisuals)
        {
            sr.enabled = !hideVisuals;
        }

        // only for one way door
        if (isOneWay)
        {
            if (state != DoorState.InvisibleWall && !isLit)
            {
                SetState(DoorState.InvisibleWall);
                connectedDoor.SetState(DoorState.Closed);
            }
        }
    }   


    public void OnInteract()
    {
        if (state == DoorState.InvisibleWall)
            return;

        // sync the 2 doors
        if (state == DoorState.Closed)
        {
            SetState(DoorState.Open);
            connectedDoor.SetState(DoorState.Open);
        } else{
            SetState(DoorState.Closed);
            if (connectedDoor.isOneWay == true) return;
            connectedDoor.SetState(DoorState.Closed); 
        }
    }

    public void SetState(DoorState newState)
    {
        state = newState;

        closeGO.SetActive(state == DoorState.Closed);
        openGO.SetActive(state == DoorState.Open);
        invisibleWall.SetActive(state == DoorState.InvisibleWall);
        topFrame.SetActive(state != DoorState.InvisibleWall);
        interactionCollider.enabled = state != DoorState.InvisibleWall;

    }

    public void SetLit(bool lit)
    {
        isLit = lit;
    }
}

public enum DoorState
{
    Closed,
    Open,
    InvisibleWall
}


