using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject before;
    [SerializeField] private GameObject after;
    [SerializeField] private string soundID;

    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private Transform onInteractTransform;

    void Awake()
    {
        SwitchInteracted(false);
    }

    public void OnInteract()
    {
        SwitchInteracted(true);
        if (soundID != null) SoundManager.Instance.PlaySFXAtPosition(soundID, transform.position);
        onInteract?.Invoke();
    }

    private void SwitchInteracted(bool interacted)
    {
        before.SetActive(!interacted);
        after?.SetActive(interacted);
    }

    public void SpawnItem(ItemClass item)
    {
        InventoryManager.Instance.SpawnItem(item, onInteractTransform);
    }
}
