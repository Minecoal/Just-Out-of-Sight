using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private Collider2D interactCollider;
    [SerializeField] private InteractUI interactUI;

    private IInteractHandler currentInteractable;

    void Awake()
    {
        interactCollider.isTrigger = true;
    }

    void Update()
    {
        // interactable destroyed or disabled
        if (currentInteractable != null)
        {
            var mb = currentInteractable as MonoBehaviour;
            if (mb == null || !mb || !mb.gameObject.activeInHierarchy)
            {
                ClearInteractable();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IInteractHandler interactable = other.GetComponent<IInteractHandler>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            Vector3 colliderPosition = other.bounds.center;
            interactUI.Show(colliderPosition, interactable.GetUIText());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IInteractHandler interactable = other.GetComponent<IInteractHandler>();
        if (interactable == currentInteractable)
        {
            currentInteractable = null;
            interactUI.Hide();
        }
    }

    private void ClearInteractable()
    {
        currentInteractable = null;
        interactUI.Hide();
    }
}
