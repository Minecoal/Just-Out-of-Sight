using UnityEngine;

public class LightDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ILitable>(out var lit))
            lit.SetLit(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<ILitable>(out var lit))
            lit.SetLit(false);
    }
}
