using UnityEngine;

public class SimpleUITooltip : MonoBehaviour, IInteractHandler
{
    [SerializeField, Multiline]
    private string tooltipText;

    public string GetUIText()
    {
        return tooltipText;
    }

    public void Interact()
    {
        
    }
}
