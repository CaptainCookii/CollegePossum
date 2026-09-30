using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class CharacterDialogue : MonoBehaviour
{

    /*
     the purpose of this script is to manage dialogue processes, such as what is interactible and when to start which dialogue when
     */

    //setup variables like the different dialogues and the interactible boolean to see if an object can be clicked
    public Character character;
    
    public DialogueRunner dialogueRunner;

    public bool interactable = true;
    private int dialogue = 0;

    private void Start()
    {
        // Make sure the outline starts invisible
        character.outline.SetActive(false);

        dialogue = Random.Range(0, 2);
    }

    private void Update()
    {
        CheckMouseHover();

        //checks if an object can be clicked on then starts appropriate dialogue accordingly

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Vector2 worldPosition =
                Camera.main.ScreenToWorldPoint(mousePosition);

            Collider2D hit = Physics2D.OverlapPoint(worldPosition);

            if (hit != null && hit.gameObject == gameObject && !dialogueRunner.IsDialogueRunning && interactable)
            {
                GameState.pinballScene = character.pinballScene;
                SceneManager.LoadScene(character.pinballScene);
            }
        }
    }

    // a function in charge of the outline which will highlight a gameobject if it is interactible and the mouse is on it

    private void CheckMouseHover()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector2 worldPosition =
            Camera.main.ScreenToWorldPoint(mousePosition);

        Collider2D hit = Physics2D.OverlapPoint(worldPosition);

        bool mouseOver = hit != null && hit.gameObject == gameObject;

        character.outline.SetActive(false);
        if (interactable && !dialogueRunner.IsDialogueRunning)
        {
            character.outline.SetActive(mouseOver);
        }
    }

    // a simple function which orders how dialogue flows

    public void StartDialogue()
    {
        if (dialogue == 0)
        {
            dialogueRunner.StartDialogue(character.dialogue1);
        }
        else
        {
            dialogueRunner.StartDialogue(character.dialogue2);
        }
        interactable = false;
    }
}