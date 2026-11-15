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
