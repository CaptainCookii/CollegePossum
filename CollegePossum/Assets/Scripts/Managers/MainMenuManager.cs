using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;
    public void PlayGame()
    {
        GameState.ResetCharacterInteractable();
        GameState.pinballScene = null;
        if (CharacterSpawnManager.Instance != null)
        {
            CharacterSpawnManager.Instance.StartNewRun();
        }
        SceneManager.LoadScene("YarnSpinnerPrototype");
    }

    public void Settings()
    {
        Debug.Log("Settings button clicked");
        settingsMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
