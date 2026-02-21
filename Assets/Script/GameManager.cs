using System;
using System.Collections;
using UnityEngine;

public class GameManager : PersistentGenericSingleton<GameManager>
{
    [Header("Game Settings")]
    [SerializeField] private float fadeDuration = 2f;
    private bool isTransitioning = false;

    public IEnumerator PlayerHit()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;
        Debug.Log("Player Hit");
        SoundManager.Instance.PlaySFXAtPosition(SoundManager.Tinnitus, PlayerManager.Instance.PlayerPosition);
        FadeManager.Instance.StartFade(fadeDuration, fadeDuration, fadeDuration);
        PlayerManager.Instance.ToggleInput(false);
        yield return FadeVolumeRoutine(false, fadeDuration);
        InventoryManager.Instance.DropItem();
        ChaserManager.Instance.ForceResetAllChasers();
        PlayerManager.Instance.ResetPlayerPosition();
        PlayerManager.Instance.ToggleFlashlight(false);
        yield return FadeVolumeRoutine(true, fadeDuration);
        PlayerManager.Instance.ToggleInput(true);
        isTransitioning = false;
    }

    private Coroutine fadeVolumeRoutine;
    public void FadeMasterVolume(bool fadeIn, float duration)
    {
        if (fadeVolumeRoutine != null)
            StopCoroutine(fadeVolumeRoutine);

        fadeVolumeRoutine = StartCoroutine(FadeVolumeRoutine(fadeIn, duration));
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
