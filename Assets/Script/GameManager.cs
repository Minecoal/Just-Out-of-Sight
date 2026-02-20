using System;
using System.Collections;
using UnityEngine;

public class GameManager : PersistentGenericSingleton<GameManager>
{
    [Header("Game Settings")]
    [SerializeField] private float fadeDuration = 1;

    public void PlayerHit()
    {
        // SoundManager.Instance.PlaySFXAtPosition(SoundManager.Tinnitus, PlayerManager.Instance.PlayerPosition);
        // StartCoroutine(FadeOutInVolumeRoutine(fadeDuration));
        // Debug.Log("Player Hit");
        // InventoryManager.Instance.DropItem();
        // ChaserManager.Instance.ForceResetAllChasers();
        // PlayerManager.Instance.ResetPlayerPosition();
        // PlayerManager.Instance.ToggleFlashlight(false);
    }

    public void PlayerDeath()
    {
        Debug.Log("Player Ded");
        // Reset Scene
    }

    private Coroutine fadeVolumeRoutine;
    public void FadeMasterVolume(bool fadeIn, float duration)
    {
        if (fadeVolumeRoutine != null)
            StopCoroutine(fadeVolumeRoutine);

        fadeVolumeRoutine = StartCoroutine(FadeVolumeRoutine(fadeIn, duration));
    }

    public IEnumerator FadeOutInVolumeRoutine(float duration)
    {
        yield return FadeVolumeRoutine(false, duration / 2f);
        yield return FadeVolumeRoutine(true, duration / 2f);
    }

    private IEnumerator FadeVolumeRoutine(bool fadeIn, float duration)
    {
        float start = AudioListener.volume;
        float end = fadeIn ? 1f : 0f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            AudioListener.volume = Mathf.Lerp(start, end, timer / duration);
            yield return null;
        }

        AudioListener.volume = end;
        fadeVolumeRoutine = null;
    }
}
