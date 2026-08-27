using UnityEngine;
using UnityEngine.InputSystem;
using Yarn;
using Yarn.Unity;

public class CharacterDialogue : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    [Header("Dialogue Nodes")]
    public string dialogue1;
    public string dialogue2;

    private bool interactable = true;

    private void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Vector2 worldPosition =
                Camera.main.ScreenToWorldPoint(mousePosition);

            Collider2D hit = Physics2D.OverlapPoint(worldPosition);

            if (hit != null && hit.gameObject == gameObject && !dialogueRunner.IsDialogueRunning && interactable)
            {
                interactable = false;
                StartRandomDialogue();
            }
        }
    }

    public void StartRandomDialogue()
    {
        int randomDialogue = Random.Range(0, 2);

        if (randomDialogue == 0)
        {
            dialogueRunner.StartDialogue(dialogue1);
        }
        else
        {
            dialogueRunner.StartDialogue(dialogue2);
        }
    }
}