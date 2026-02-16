using System;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 MoveInputRaw { get; private set; }
    public Vector3 MoveInputNormalized { get; private set; }
    public Vector3 MousePosition { get; private set; }

    public KeyCode interactButton = KeyCode.E;
    public Action OnInteract;

    void Update()
    {
        MoveInputRaw = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (MoveInputRaw.sqrMagnitude > 0.01f){
            MoveInputNormalized = MoveInputRaw.normalized;
        } else {
            MoveInputNormalized = Vector3.zero;
        }

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (Input.GetKeyDown(interactButton))
        {
            OnInteract?.Invoke();
        }
    }
}
