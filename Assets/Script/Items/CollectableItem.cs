using UnityEngine;

public class CollectableItem : MonoBehaviour, IInteractHandler
{
    public ItemClass item;

    public void Interact()
    {
        if(InventoryManager.Instance.GetItem() == null)
        {
            InventoryManager.Instance.AddItem(item);
            Destroy(this.gameObject);
        }
    }

    public void SetItem(ItemClass itemToSet)
    {
        item = itemToSet;
        GetComponent<SpriteRenderer>().sprite = item.itemIcon;
    }
}
