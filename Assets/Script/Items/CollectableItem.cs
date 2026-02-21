using UnityEngine;

public class CollectableItem : MonoBehaviour, IInteractHandler
{
    public ItemClass item;
    [SerializeField] private string OnInteractUIText = "E";

    public void Interact()
    {
        if(InventoryManager.Instance.GetItem() == null)
        {
            InventoryManager.Instance.AddItem(item);
            Destroy(this.gameObject);
        }
        else
        {
            InventoryManager.Instance.DropItem();
            InventoryManager.Instance.AddItem(item);
            Destroy(this.gameObject);
        }
    }

    public void SetItem(ItemClass itemToSet)
    {
        item = itemToSet;
        GetComponent<SpriteRenderer>().sprite = item.itemIcon;
    }

    public string GetUIText()
    {
        return $"{OnInteractUIText} \n {item.itemName}";
    }
}
