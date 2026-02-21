using UnityEngine;

public class ItemInteractionAdaptor : MonoBehaviour, IInteractHandler
{
    private IInteractable interactable;
    [SerializeField] private string OnInteractUIText = "E";

    void Awake()
    {
        interactable = GetComponent<IInteractable>();
    }

    public void Interact()
    {
        interactable?.OnInteract();
    }

    public string GetUIText()
    {
        return OnInteractUIText;
    }
}
