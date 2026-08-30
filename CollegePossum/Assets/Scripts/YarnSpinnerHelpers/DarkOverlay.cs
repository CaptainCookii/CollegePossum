using UnityEngine;
using System.Collections;

public class DarkOverlay : MonoBehaviour
{

    /*
     the purpose of this script is to manage when the screen dims for dialogue 
     */

    //has variables like fadeDuration which effects how fast the screen dims
    public SpriteRenderer overlay;
    public float fadeDuration = 1.5f;

    //dims the scene
    public void FadeIn()
    {
        StartCoroutine(FadeTo(0.85f));
    }

    //returns it to normal
    public void FadeOut()
    {
        StartCoroutine(FadeTo(0f));
    }

    //this function changes the alpha of a black screen, creating the illusion of dimming

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