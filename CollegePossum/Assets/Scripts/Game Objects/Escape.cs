using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class Escape : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //checks if an object can be clicked on then starts appropriate dialogue accordingly

        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            GameState.ResetCharacterInteractable();
            GameState.pinballScene = null;

            SceneManager.LoadScene("MainMenu");
        }
    }
}
