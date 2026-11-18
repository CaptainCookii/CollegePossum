using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class MainMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private EventReference mainMenuEvent;

    public void Start()
    {
        Invoke(nameof(StartMusic), .1f);
    }

    private void StartMusic()
    {
        MusicManager.Instance.StartMusic(mainMenuEvent);
        MusicManager.Instance.SetMusicLayer("MainMenu", true);
    }

    public void PlayGame()
    {
        GameState.ResetCharacterInteractable();
        GameState.pinballScene = null;
        if (CharacterSpawnManager.Instance != null)
        {
            CharacterSpawnManager.Instance.StartNewRun();
            GameState.ResetRun();
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
