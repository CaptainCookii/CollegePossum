using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("FMOD Music Event")]
    [SerializeField]
    private EventReference musicEvent;

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

    private void Start()
    {
        StartMusic();
    }

    private void StartMusic()
    {
        if (musicInstance.isValid())
            return;

        musicInstance = RuntimeManager.CreateInstance(musicEvent);
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

    // -------------------------
    // CLEANUP
    // -------------------------

    private void OnDestroy()
    {
        if (!musicInstance.isValid())
            return;

        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        musicInstance.release();
    }
}