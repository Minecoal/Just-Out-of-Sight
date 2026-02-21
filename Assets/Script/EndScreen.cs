using System.Collections;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private AudioClip footsteps;

    private float fadeDelay = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayFootsteps(1f, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    


    public void PlayFootsteps(float volume, float fadeDuration)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        source.clip = footsteps;
        source.volume = volume;
        source.loop = false;
        source.playOnAwake = false;

        source.Play();
        StartCoroutine(FadeOutAndDestroy(source, fadeDuration));
    }

    private IEnumerator FadeOutAndDestroy(AudioSource source, float fadeDuration)
    {
        yield return new WaitForSeconds(fadeDelay);

        float startVolume = source.volume;

        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        source.Stop();
        Destroy(source);
    }
}
