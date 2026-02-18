using UnityEngine;


[CreateAssetMenu(fileName = "new Item Class", menuName = "Item")]
public class ItemClass : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
}
