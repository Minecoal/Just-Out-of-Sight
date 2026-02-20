using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : GenericSingleton<FadeManager>
{
    [SerializeField] private Image fadeImage;

    override protected void Awake()
    {
        base.Awake();
        if (fadeImage == null)
        {
            // create black image if none assigned
            GameObject go = new GameObject("FadeImage");
            go.transform.SetParent(transform, false);
            fadeImage = go.AddComponent<Image>();
            fadeImage.color = new Color(0, 0, 0, 0);
            RectTransform rt = fadeImage.rectTransform;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }
    }

    public void StartFade(
        float onFadeDuration,
        float duringFadeDuration,
        float afterFadeDuration)
    {
        StopAllCoroutines();
        StartCoroutine(FadeRoutine(onFadeDuration, duringFadeDuration, afterFadeDuration));
    }

    private IEnumerator FadeRoutine(
        float onFadeDuration,
        float duringFadeDuration,
        float afterFadeDuration)
    {
        // fade in
        float timer = 0f;
        while (timer < onFadeDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / onFadeDuration);
            SetAlpha(t);
            yield return null;
        }
        SetAlpha(1f);

        // hold black
        yield return new WaitForSeconds(duringFadeDuration);

        // fade out
        timer = 0f;
        while (timer < afterFadeDuration)
        {
            timer += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(timer / afterFadeDuration);
            SetAlpha(t);
            yield return null;
        }
        SetAlpha(0f);
    }

    private void SetAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = alpha;
            fadeImage.color = c;
        }
    }
}
