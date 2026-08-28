using UnityEngine;
using System.Collections;

public class DarkOverlay : MonoBehaviour
{
    public SpriteRenderer overlay;
    public float fadeDuration = 1.5f;

    public void FadeIn()
    {
        StartCoroutine(FadeTo(0.85f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeTo(0f));
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        Color color = overlay.color;
        float startAlpha = color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            overlay.color = color;

            yield return null;
        }

        color.a = targetAlpha;
        overlay.color = color;
    }
}