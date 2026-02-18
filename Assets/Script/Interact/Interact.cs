using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private Collider2D interactCollider;
    [SerializeField] private InteractUI interactUI;

    private IInteractable currentInteractable;

    void Awake()
    {
        interactCollider.isTrigger = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.OnInteract();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            Vector3 colliderPosition = other.bounds.center;
            interactUI.Show(colliderPosition, "E");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable == currentInteractable)
        {
            currentInteractable = null;
            interactUI.Hide();
        }
    }
}
