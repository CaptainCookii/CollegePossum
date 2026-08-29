using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class CharacterDialogue : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    [Header("Dialogue Nodes")]
    public string dialogue1;
    public string dialogue2;

    public GameObject outline;

    private bool interactable = true;
    private int dialogue = 0;

    private void Start()
    {
        // Make sure the outline starts invisible
        outline.SetActive(false);
    }

    private void Update()
    {
        CheckMouseHover();

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Vector2 worldPosition =
                Camera.main.ScreenToWorldPoint(mousePosition);

            Collider2D hit = Physics2D.OverlapPoint(worldPosition);

            if (hit != null && hit.gameObject == gameObject && !dialogueRunner.IsDialogueRunning && interactable)
            {
                StartDialogue();
            }
        }
    }

    private void CheckMouseHover()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector2 worldPosition =
            Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D hit = Physics2D.OverlapPoint(worldPosition);

        bool mouseOver = hit != null && hit.gameObject == gameObject;

        outline.SetActive(false);
        if (interactable && !dialogueRunner.IsDialogueRunning)
        {
            outline.SetActive(mouseOver);
        }
    }

    public void StartDialogue()
    {
        if (dialogue == 0)
        {
            dialogueRunner.StartDialogue(dialogue1);
            dialogue += 1;
        }
        else
        {
            dialogueRunner.StartDialogue(dialogue2);
            interactable = false;
        }
    }
}