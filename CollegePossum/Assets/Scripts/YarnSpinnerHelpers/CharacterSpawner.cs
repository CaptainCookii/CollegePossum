using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [Header("All Available Character Prefabs")]
    [SerializeField]
    private List<GameObject> characterPrefabs =
        new List<GameObject>();

    [Header("Player")]
    [SerializeField] private int playerCharacterIndex = 0;
    [SerializeField] private Transform playerSpawn;

    [Header("NPC Spawn Points")]
    [SerializeField] private Transform[] npcSpawnPoints = new Transform[3];

    [Header("Dialogue Positions")]
    [SerializeField] private Transform dialogueLeft;
    [SerializeField] private Transform dialogueRight;

    [Header("Spawned Characters")]
    public GameObject playerCharacter;
    public List<GameObject> spawnedNPCs = new List<GameObject>();

    private void Awake()
    {
        SpawnCharacters();
    }

    public void SpawnCharacters()
    {
        if (!ValidateSetup())
            return;

        if (CharacterSpawnManager.Instance == null)
        {
            Debug.LogError(
                "CharacterSpawnManager is missing from the scene.",
                this
            );
            return;
        }

        CharacterSpawnManager manager =
            CharacterSpawnManager.Instance;

        manager.EnsureLineup(
            characterPrefabs.Count,
            playerCharacterIndex,
            npcSpawnPoints.Length
        );

        if (!manager.HasLineup)
            return;

        // Spawn the saved player character.
        playerCharacter = Instantiate(
            characterPrefabs[manager.PlayerCharacterIndex],
            playerSpawn.position,
            playerSpawn.rotation
        );

        PositionDialogueAvatars(playerCharacter);

        spawnedNPCs.Clear();

        // Spawn each saved NPC at its assigned point.
        for (int i = 0; i < npcSpawnPoints.Length; i++)
        {
            int characterIndex = manager.NPCCharacterIndices[i];

            GameObject npc = Instantiate(
                characterPrefabs[characterIndex],
                npcSpawnPoints[i].position,
                npcSpawnPoints[i].rotation
            );

            PositionDialogueAvatars(npc);
            spawnedNPCs.Add(npc);
        }
    }

    private void PositionDialogueAvatars(GameObject character)
    {
        Transform leftAvatar = null;
        Transform rightAvatar = null;

        foreach (Transform child in
            character.GetComponentsInChildren<Transform>(true))
        {
            if (child == character.transform)
                continue;

            if (child.name.EndsWith("Left"))
                leftAvatar = child;
            else if (child.name.EndsWith("Right"))
                rightAvatar = child;
        }

        if (leftAvatar != null && dialogueLeft != null)
        {
            leftAvatar.position = dialogueLeft.position;
            leftAvatar.rotation = dialogueLeft.rotation;
        }

        if (rightAvatar != null && dialogueRight != null)
        {
            rightAvatar.position = dialogueRight.position;
            rightAvatar.rotation = dialogueRight.rotation;
        }
    }

    private bool ValidateSetup()
    {
        if (characterPrefabs.Count < npcSpawnPoints.Length + 1)
        {
            Debug.LogError(
                "You need at least one player character and " +
                "one unique NPC for each spawn point.",
                this
            );
            return false;
        }

        if (playerCharacterIndex < 0 ||
            playerCharacterIndex >= characterPrefabs.Count)
        {
            Debug.LogError(
                "Player character index is out of range.",
                this
            );
            return false;
        }

        if (playerSpawn == null)
        {
            Debug.LogError("Assign the player spawn point.", this);
            return false;
        }

        foreach (GameObject prefab in characterPrefabs)
        {
            if (prefab == null)
            {
                Debug.LogError(
                    "One or more character prefab slots are empty.",
                    this
                );
                return false;
            }
        }

        foreach (Transform point in npcSpawnPoints)
        {
            if (point == null)
            {
                Debug.LogError(
                    "One or more NPC spawn points are missing.",
                    this
                );
                return false;
            }
        }

        return true;
    }
}