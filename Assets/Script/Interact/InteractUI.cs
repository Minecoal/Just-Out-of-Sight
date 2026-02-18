using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField] private TextMesh promptText;

    void Awake()
    {
        if (promptText == null)
            promptText = GetComponent<TextMesh>();

        Hide();
    }

    public void Show(Vector3 position, string message = "E")
    {
        if (promptText == null) return;

        promptText.text = message;

        if (promptText.GetComponent<MeshRenderer>() != null)
            promptText.transform.position = position;
            promptText.GetComponent<MeshRenderer>().enabled = true;
    }

    public void Hide()
    {
        if (promptText == null) return;

        if (promptText.GetComponent<MeshRenderer>() != null)
            promptText.GetComponent<MeshRenderer>().enabled = false;
    }
}
