using UnityEngine;

public class PlayerManager : GenericSingleton<PlayerManager>
{
    [SerializeField] Player Player;
    [SerializeField] Transform FlashLight;
    public Vector3 PlayerPosition => Player.transform.position;
    public Vector3 PlayerRotation => Player.transform.rotation.eulerAngles;
    public Vector3 FlashLightPosition => FlashLight.transform.position;
    public Vector3 FlashLightRotation => FlashLight.transform.rotation.eulerAngles;
}
