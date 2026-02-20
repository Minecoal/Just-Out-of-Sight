using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : GenericSingleton<InventoryManager>
{
    private ItemClass item;

    [SerializeField] private GameObject itemSlot;
    private Image itemSlotImage;

    [SerializeField] private GameObject droppedItemPrefab;


    void Start()
    {
        itemSlotImage = itemSlot.transform.GetChild(0).GetComponent<Image>();
        itemSlotImage.enabled = false;
        itemSlotImage.sprite = null;
    }

    public void AddItem(ItemClass itemToAdd)
    {
        if(item == null)
        {
            item = itemToAdd;
            itemSlotImage.enabled = true;
            itemSlotImage.sprite = item.itemIcon;

            SoundManager.Instance.PlaySFXAtPosition(SoundManager.PickUp, PlayerManager.Instance.PlayerPosition);
        } else
        {
            TextDisplayManager.New3D(Vector3.zero, 0.1f).WithInitialText("I can't hold more things").WithAutoDestroy(2f).Build();
        }
    }

    public void RemoveItem()
    {
        itemSlotImage.enabled = false;
        itemSlotImage.sprite = null;
        item = null;
    }

    public void DropItem()
    {
        if (item != null)
        {
            GameObject g = GameObject.Instantiate(droppedItemPrefab, PlayerManager.instance.PlayerPosition, Quaternion.identity);
            g.GetComponent<CollectableItem>().SetItem(item);
            RemoveItem();
            SoundManager.Instance.PlaySFXAtPosition(SoundManager.PickUp, PlayerManager.Instance.PlayerPosition, pitchOverride:0.8f);
        }
        
    }

    public ItemClass GetItem()
    {
        return item;
    }
}
