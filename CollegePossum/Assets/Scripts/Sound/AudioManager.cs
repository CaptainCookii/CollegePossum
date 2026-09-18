using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances;

    public static AudioManager instance { get; private set; }

    private EventInstance musicEventInstance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one Audio Manager in the scene.");
        }
        instance = this;

        eventInstances = new List<EventInstance>();

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.instance.themes); 
    }


    /// <summary>
    /// plays oneshot sound
    /// </summary>
    /// <param name="sound">what am i playing</param>
    /// <param name="worldPos">where am i playing</param>
    public void PlayOneShot(EventReference sound, Vector2 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }


    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference);
        musicEventInstance.start();
    }

    /// <summary>
    /// set value of a theme
    /// </summary>
    /// <param name="parameterName">what theme</param>
    /// <param name="parameterValue">value of the theme (0 = off, 1 = on, or more for scale)</param>
    public void SetThemeParameter(string parameterName, float parameterValue)
    {
        musicEventInstance.setParameterByName(parameterName, parameterValue);
    }

    /// <summary>
    /// create an instance of a sound
    /// </summary>
    /// <param name="eventReference">what sound?</param>
    /// <returns></returns>
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    //which music is on/off
    public bool hollisOn = false;
    public bool pachinkoOn = false;
    public bool pamOn = false;
    public bool partyOn = false;
    public bool terrenceOn = false;
    public bool beauOn = false;

    public void ChangeThemeHollis()
    {
        if (hollisOn)
        {
            AudioManager.instance.SetThemeParameter("Hollis", 0);
            hollisOn = false;
            return;
        }
        AudioManager.instance.SetThemeParameter("Hollis", 1);
        hollisOn = true;
    }

    public void ChangeThemePachinko()
    {
        if (pachinkoOn)
        {
            AudioManager.instance.SetThemeParameter("Pachinko", 0);
            pachinkoOn = false;
            return;
        }
        AudioManager.instance.SetThemeParameter("Pachinko", 1);
        pachinkoOn = true;
    }

    public void ChangeThemePam()
    {
        if (pamOn)
        {
            AudioManager.instance.SetThemeParameter("Pam", 0);
            pamOn = false;
            return;
        }
        AudioManager.instance.SetThemeParameter("Pam", 1);
        pamOn = true;
    }

    public void ChangeThemeParty()
    {
        if (partyOn)
        {
            AudioManager.instance.SetThemeParameter("Party", 0);
            partyOn = false;
            return;
        }
        AudioManager.instance.SetThemeParameter("Party", 1);
        partyOn = true;
    }

    public void ChangeThemeTerrence()
    {
        if (terrenceOn)
        {
            AudioManager.instance.SetThemeParameter("Terrence", 0);
            terrenceOn = false;
            return;
        }
        AudioManager.instance.SetThemeParameter("Terrence", 1);
        terrenceOn = true;
    }

    public void ChangeThemeBeau()
    {
        if (beauOn)
        {
            AudioManager.instance.SetThemeParameter("Beau", 0);
            beauOn = false;
            return;
        }
        AudioManager.instance.SetThemeParameter("Beau", 1);
        beauOn = true;
    }

}