using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
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
        //SceneManager.LoadScene("SettingsMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
