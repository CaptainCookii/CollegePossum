using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Start()
    {
        Invoke(nameof(StartMusic), .1f);
    }

    private void StartMusic()
    {
        MusicManager.Instance.SetMusicLayer("Party", true);
    }

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
