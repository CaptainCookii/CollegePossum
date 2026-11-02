using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }

    [SerializeField] private GameObject currentRoom;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    public void LoadRoom(GameObject roomPrefab)
    {
        if (roomPrefab == null)
        {
            Debug.Log("RoomManager: no room prefab given.", this);
            return;
        }

        if (currentRoom != null)
        {
            RemoveSpawnCharacters(currentRoom);
            Destroy(currentRoom);
        }

        currentRoom = Instantiate(roomPrefab);
    }

    private void RemoveSpawnCharacters(GameObject room)
    {
        CharacterSpawner spawner = room.GetComponentInChildren<CharacterSpawner>();
        if (spawner == null)
            return;

        if (spawner.playerCharacter != null)
            Destroy(spawner.playerCharacter.gameObject);

        foreach (GameObject npc in spawner.spawnedNPCs)
        {
            if (npc != null)
            {
                Destroy(npc);
            }
        }
    }
}
