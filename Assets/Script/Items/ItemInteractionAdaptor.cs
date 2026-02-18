using UnityEngine;

public class ItemInteractionAdaptor : MonoBehaviour, IInteractHandler
{
    private IInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<IInteractable>();
    }

    public void Interact()
    {
        interactable?.OnInteract();
    }
}
