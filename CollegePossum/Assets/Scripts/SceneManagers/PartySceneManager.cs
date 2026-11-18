using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class PartySceneManager : MonoBehaviour
{
    [SerializeField] private EventReference partyEvent;

    public void Start()
    {
        Invoke(nameof(StartMusic), .2f);
    }

    private void StartMusic()
    {
        MusicManager.Instance.ClearInstance();
        MusicManager.Instance.StartMusic(partyEvent);
        MusicManager.Instance.SetMusicLayer("Party", true);
    }
}
