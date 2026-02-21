using System.Collections;
using UnityEngine;

public class ItemRequirement : MonoBehaviour, IInteractHandler
{
    [Header("Requirement")]
    [SerializeField] private ItemClass requiredItem;
    [SerializeField] private bool consumeItemOnSuccess = true;
    [SerializeField] private bool oneTimeConsumption = true;
    [SerializeField] private bool oneTimeTrigger = false;
    [SerializeField] private string OnInteractUIText = "E";

    private IInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<IInteractable>();
        if (interactable == null)
            Debug.LogError($"ItemRequirement requires an IInteractable on the same GameObject: {gameObject.name}", this);
    }

    public void Interact()
    {
        var inventory = InventoryManager.Instance;
        var heldItem = inventory.GetItem();

        if (requiredItem == null)
        {
            // no requirement
            interactable.OnInteract();
            return;
        }

        if (heldItem != requiredItem)
        {
            // wrong or missing item
            TextDisplayManager.New3D(PlayerManager.Instance.PlayerPosition, 0.1f).WithInitialText($"I need a {requiredItem.itemName}").WithAutoDestroy(2f).Build();
            SoundManager.Instance.PlaySFXAtPosition(SoundManager.OpenLockDoor, PlayerManager.Instance.PlayerPosition);
            return;
        }

        // success
        interactable.OnInteract();

        if (consumeItemOnSuccess)
            inventory.RemoveItem();

        if (oneTimeConsumption){
            requiredItem = null;
        }
        if (oneTimeTrigger) {
            
            Destroy(this);
        }
        return;
    }

    public string GetUIText()
    {
        return OnInteractUIText;
    }
}
