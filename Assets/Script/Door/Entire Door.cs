using UnityEngine;

public class EntireDoor : MonoBehaviour,  ILitable
{
    [SerializeField] private Door door1;
    [SerializeField] private Door door2;

    public void SetLit(bool lit)
    {
        door1.SetLit(lit);
        door2.SetLit(lit);
    }
}
