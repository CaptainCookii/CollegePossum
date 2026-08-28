using UnityEngine;
using System.Collections;
using Yarn;
using Yarn.Unity;

public class CharacterManager : MonoBehaviour
{
    public float fadeDuration = 1.5f;
    [Header("Characters")]
    public SpriteRenderer FrancineL;
    public SpriteRenderer FrancineR;
    public SpriteRenderer BeauL;
    public SpriteRenderer BeauR;
    public SpriteRenderer TerryL;
    public SpriteRenderer TerryR;
    public SpriteRenderer PamellaL;
    public SpriteRenderer PamellaR;

    private SpriteRenderer activeL;
    private SpriteRenderer activeR;

    [YarnCommand("set_left")]
    public void Set_Left(string character)
    {
        if (character == "Francine")
        {
            StartCoroutine(FadeTo(0.85f, FrancineL));
            activeL = FrancineL;
        }
        else if (character == "Beau")
        {
            StartCoroutine(FadeTo(0.85f, BeauL));
            activeL = BeauL;
        }
        else if (character == "Terry")
        {
            StartCoroutine(FadeTo(0.85f, TerryL));
            activeL = TerryL;
        }
        else if (character == "Pamella")
        {
            StartCoroutine(FadeTo(0.85f, PamellaL));
            activeL = PamellaL;
        }
    }

    [YarnCommand("set_right")]
    public void Set_Right(string character)
    {
        if (character == "Francine")
        {
            StartCoroutine(FadeTo(0.85f, FrancineR));
            activeR = FrancineR;
        }
        else if (character == "Beau")
        {
            StartCoroutine(FadeTo(0.85f, BeauR));
            activeR = BeauR;
        }
        else if (character == "Terry")
        {
            StartCoroutine(FadeTo(0.85f, TerryR));
            activeR = TerryR;
        }
        else if (character == "Pamella")
        {
            StartCoroutine(FadeTo(0.85f, PamellaR));
            activeR = PamellaR;
        }
    }

    [YarnCommand("left")]
    public void LeftTalk()
    {
        StartCoroutine(FadeToBlack(0.6f, activeR));
        StartCoroutine(FadeToBlack(0f, activeL));
    }

    [YarnCommand("right")]
    public void RightTalk()
    {
        StartCoroutine(FadeToBlack(0.6f, activeL));
        StartCoroutine(FadeToBlack(0f, activeR));
    }

    [YarnCommand("ending")]
    public void Ending()
    {
        StartCoroutine(FadeTo(0f, activeL));
        StartCoroutine(FadeTo(0f, activeR));
    }

    private IEnumerator FadeTo(float targetAlpha, SpriteRenderer overlay)
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

    private IEnumerator FadeToBlack(float targetBlack, SpriteRenderer overlay)
    {
        if (overlay == null)
            yield break;

        Color normalColor = Color.white;
        Color startColor = overlay.color;

        Color targetColor = Color.Lerp(normalColor, Color.black, targetBlack);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / fadeDuration);

            overlay.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        overlay.color = targetColor;
    }
}
