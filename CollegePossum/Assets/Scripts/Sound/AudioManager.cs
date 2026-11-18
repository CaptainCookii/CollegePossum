using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Music Events")]
    [SerializeField] private EventReference homeMusic;
    [SerializeField] private EventReference partyMusic;

    private EventInstance musicInstance;

    public enum MusicEvent
    {
        None,
        Home,
        Party
    }

    public MusicEvent CurrentMusic { get; private set; } = MusicEvent.None;

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

    // =========================================================
    // MUSIC EVENT CONTROL
    // =========================================================

    public void SwitchMusic(MusicEvent newMusic)
    {
        // Don't restart the same music.
        if (CurrentMusic == newMusic && musicInstance.isValid())
            return;

        StopCurrentMusic();

        EventReference newEvent = GetEventReference(newMusic);

        if (newEvent.IsNull)
        {
            CurrentMusic = MusicEvent.None;
            return;
        }

        musicInstance = RuntimeManager.CreateInstance(newEvent);

        musicInstance.start();

        CurrentMusic = newMusic;
    }

    private EventReference GetEventReference(MusicEvent music)
    {
        switch (music)
        {
            case MusicEvent.Home:
                return homeMusic;

            case MusicEvent.Party:
                return partyMusic;

            default:
                return default;
        }
    }

    public void StopCurrentMusic()
    {
        if (!musicInstance.isValid())
            return;

        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
        musicInstance.clearHandle();

        CurrentMusic = MusicEvent.None;
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

    public void SetAnxietyLayer(int level)
    {
        if (!musicInstance.isValid())
            return;

        musicInstance.setParameterByName("Anxiety", level);
        //Debug.Log("Anxiety Level: " + level);
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