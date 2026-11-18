using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private EventInstance musicInstance;

    private void Awake()
    {
        // Singleton protection
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartMusic(EventReference currentEvent)
    {
        if (musicInstance.isValid())
            return;

        musicInstance = RuntimeManager.CreateInstance(currentEvent);
        musicInstance.start();
    }

    // -------------------------
    // ACTIVATE MUSIC LAYERS
    // -------------------------

    public void SetMusicLayer(string musicLayer, bool active)
    {
        if (!musicInstance.isValid())
            return;

        musicInstance.setParameterByName(musicLayer, active ? 1f : 0f);
        Debug.Log("Music Layer: " + musicLayer + " Active: " +  active);
    }

    public void SetAnxietyLayer(string musicLayer, bool active)
    {
        if (!musicInstance.isValid())
            return;

        musicInstance.setParameterByName(musicLayer, active ? 1f : 0f);
        Debug.Log("Music Layer: " + musicLayer + " Active: " + active);
    }

    // -------------------------
    // CLEANUP
    // -------------------------

    public void ClearInstance()
    {
        if (!musicInstance.isValid())
            return;

        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void OnDestroy()
    {
        ClearInstance();
        musicInstance.release();
    }
}