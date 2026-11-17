using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using Yarn.Unity;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject doorMenu;
    [SerializeField] CharacterSpawner spawner;

    [SerializeField] TMP_Text goalText;
    [SerializeField] Button nextRoomButton;

    [Tooltip("Points needed to ENTER each room. Index = destination room.")]
    [SerializeField] float[] pointsToEnter = { 0, 10, 25 };

    int NextRoomIndex => (GameState.currentRoom + 1) % spawner.RoomCount;

    float PointsNeeded =>
        pointsToEnter[Mathf.Clamp(NextRoomIndex, 0, pointsToEnter.Length - 1)];

    bool CanAdvance =>
        GameManager.Instance.currCoolPoints >= PointsNeeded;

    void RefreshMenu()
    {
        float have = GameManager.Instance.currCoolPoints;
        float need = PointsNeeded;

        nextRoomButton.interactable = have >= need;
        goalText.text = have >= need
            ? $"Alright, I think i'm cool enough to proceed ({have}/{need})"
            : $"I don't think i'm cool enough to go in there yet ({have}/{need})";
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Vector2 worldPosition =
                Camera.main.ScreenToWorldPoint(mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPosition);

            if (hit != null && hit.gameObject == GameObject.FindGameObjectWithTag("Door"))
            {
                RefreshMenu();
                doorMenu.SetActive(true);
            }
        }
    }

    public void EndRun()
    {
        SceneManager.LoadScene("MainMenu");
        GameState.ResetRun();
    }

    public void Nevermind()
    {
        doorMenu.SetActive(false);
    }

    public void NextRoom()
    {
        if (!CanAdvance) return;

        int next = (GameState.currentRoom + 1) % spawner.RoomCount;
        spawner.ShowRoom(next);
        doorMenu.SetActive(false);
    }
}
