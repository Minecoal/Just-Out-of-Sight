using UnityEngine;

public class PlayerManager : GenericSingleton<PlayerManager>
{
    [SerializeField] Transform PlayerRespawnpoint;
    [SerializeField] Player Player;
    [SerializeField] Transform FlashLight;
    public Transform PlayerTransform => Player.transform;
    public Vector3 PlayerPosition => Player.transform.position;
    public Vector3 PlayerRotation => Player.transform.rotation.eulerAngles;
    public Vector3 FlashLightPosition => FlashLight.transform.position;
    public Vector3 FlashLightRotation => FlashLight.transform.rotation.eulerAngles;

    public void ToggleFlashlight(bool enable)
    {
        Player.ToggleFlashlight(enable);
    }
    public void ResetPlayerPosition()
    {
        Player.transform.position = PlayerRespawnpoint.position;
    }

    public void HitPlayer()
    {
        Player.PlayerHit();
    }
}
