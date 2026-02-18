using System;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 MoveInputRaw { get; private set; }
    public Vector3 MoveInputNormalized { get; private set; }
    public Vector3 MousePosition { get; private set; }

    public KeyCode interactButton = KeyCode.E;
    public KeyCode flashlightButton = KeyCode.F;
    public KeyCode dropItemButton = KeyCode.Q;
    public Action OnInteract;
    public Action OnToggleFlashlight;
    public Action OnDropItem;

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
        if (Input.GetKeyDown(flashlightButton))
        {
            OnToggleFlashlight?.Invoke();
        }
        if (Input.GetKeyDown(dropItemButton))
        {
            OnDropItem?.Invoke();
        }
    }
}
