using UnityEngine;

public class PlayerManager : GenericSingleton<PlayerManager>
{
    [SerializeField] Player Player;
    public Vector3 PlayerPosition => Player.transform.position;
}
