using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class PartySceneManager : MonoBehaviour
{
    [SerializeField] private EventReference partyEvent;

    public void Start()
    {
        Invoke(nameof(StartMusic), .1f);
    }

    private void StartMusic()
    {
        MusicManager.Instance.SwitchMusic(MusicManager.MusicEvent.Party);
        MusicManager.Instance.SetMusicLayer("Party", true);
    }
}
