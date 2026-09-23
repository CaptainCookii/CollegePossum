using UnityEngine;
using System.Collections;
using Yarn;
using Yarn.Unity;
using System.Collections.Generic;

public static class GameState
{
    public static string pinballScene;
    public static bool terryInteractable = true;
    public static bool pamellaInteractable = true;
    public static bool beauInteractable = true;

    private static Dictionary<string, bool> characterInteractable =
        new Dictionary<string, bool>();

    public static bool GetInteractable(string characterID)
    {
        if (characterInteractable.ContainsKey(characterID))
        {
            return characterInteractable[characterID];
        }
        characterInteractable[characterID] = true;
        return true;
    }

    public static void SetInteractable(string characterID, bool value)
    {
        characterInteractable[characterID] = value;
    }

    public static void ResetCharacterInteractable()
    {
        characterInteractable.Clear();
    }
}


public class CharacterManager : MonoBehaviour
{

    /*
     the purpose of this script is to create the effect of characters talking to each other
     */

    //sets up all the gameobjects required as well as a duration for each action
    public float fadeDuration = 1.5f;
    [Header("Characters")]
    private Character[] characters;
    
    private SpriteRenderer activeL;
    private SpriteRenderer activeR;

    // a function which places a character on the left side of the narritive

    [YarnCommand("set_left")] //yarn commands allow them to be called in yarn scripts
    public void Set_Left(string character)
    {
        foreach (Character name in characters)
        {
            if (name.characterName == character)
            {
                StartCoroutine(FadeTo(1f, name.left));
                activeL = name.left;
            }
        }
    }

    // a function which places a character on the right side of the narritive

    [YarnCommand("set_right")]
    public void Set_Right(string character)
    {
        foreach (Character name in characters)
        {
            if (name.characterName == character)
            {
                StartCoroutine(FadeTo(1f, name.right));
                activeR = name.right;
            }
        }
    }

    // creates the effect of the left character talking by dimming the right character

    [YarnCommand("left")]
    public void LeftTalk()
    {
        StartCoroutine(FadeToBlack(0.6f, activeR));
        StartCoroutine(FadeToBlack(0f, activeL));
    }

    // creates the effect of the right character talking by dimming the left character

    [YarnCommand("right")]
    public void RightTalk()
    {
        StartCoroutine(FadeToBlack(0.6f, activeL));
        StartCoroutine(FadeToBlack(0f, activeR));
    }

    // fades the characters when dialogue ends

    [YarnCommand("ending")]
    public void Ending()
    {
        StartCoroutine(FadeTo(0f, activeL));
        StartCoroutine(FadeTo(0f, activeR));
    }

    // a function which edits the alpha values

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

    // a function which edits color on characters to create a dimming effect

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

    private void Start()
    {
        characters = FindObjectsByType<Character>(FindObjectsSortMode.None);

        CharacterDialogue characterDialogue = new CharacterDialogue();

        foreach (Character name in characters)
        {
            if (GameState.pinballScene == name.pinballScene)
            {
                characterDialogue = name.character.GetComponent<CharacterDialogue>();
                GameState.SetInteractable(name.characterName, false);
                characterDialogue.StartDialogue();
            }

            if (GameState.GetInteractable(name.characterName) == false)
            {
                characterDialogue = name.character.GetComponent<CharacterDialogue>();
                characterDialogue.interactable = false;
            }
        }
    }
}
