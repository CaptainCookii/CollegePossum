using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnManager : MonoBehaviour
{
    public static CharacterSpawnManager Instance { get; private set; }

    public bool HasLineup { get; private set; }

    public int PlayerCharacterIndex { get; private set; }

    // Element 0 belongs to NPC spawn point 0, etc.
    public List<int> NPCCharacterIndices { get; private set; }
        = new List<int>();

    private int savedCharacterCount;
    private int savedNPCCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void EnsureLineup(
        int characterCount,
        int playerIndex,
        int npcCount)
    {
        // Keep the existing lineup if it is still compatible.
        if (HasLineup &&
            savedCharacterCount == characterCount &&
            savedNPCCount == npcCount)
        {
            return;
        }

        CreateNewLineup(characterCount, playerIndex, npcCount);
    }

    public void CreateNewLineup(
        int characterCount,
        int playerIndex,
        int npcCount)
    {
        if (characterCount < npcCount + 1 ||
            playerIndex < 0 ||
            playerIndex >= characterCount)
        {
            Debug.LogError("Invalid character lineup settings.");
            HasLineup = false;
            return;
        }

        PlayerCharacterIndex = playerIndex;
        NPCCharacterIndices.Clear();

        // Add every character except the player.
        for (int i = 0; i < characterCount; i++)
        {
            if (i != playerIndex)
            {
                NPCCharacterIndices.Add(i);
            }
        }

        // Shuffle the NPC selection and their slot assignments.
        for (int i = NPCCharacterIndices.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            int temp = NPCCharacterIndices[i];
            NPCCharacterIndices[i] = NPCCharacterIndices[j];
            NPCCharacterIndices[j] = temp;
        }

        // Keep only the number of NPCs we need.
        NPCCharacterIndices.RemoveRange(
            npcCount,
            NPCCharacterIndices.Count - npcCount
        );

        savedCharacterCount = characterCount;
        savedNPCCount = npcCount;
        HasLineup = true;
    }

    // Call this when the player chooses New Game.
    public void StartNewRun()
    {
        HasLineup = false;
        NPCCharacterIndices.Clear();
    }
}
