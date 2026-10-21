using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown displayModeDropDown;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private int windowedWidth = 1280;
    [SerializeField] private int windowedHeight = 720;

    private int pendingDisplayMode;

    private float appliedMasterVolume = 1f, appliedMusicVolume = 1f, appliedSFXVolume = 1f;
    private float pendingMasterVolume, pendingMusic, pendingSFX;


    void OnEnable()
    {
        pendingDisplayMode = CurrentDisplayModeIndex();
        displayModeDropDown.SetValueWithoutNotify(pendingDisplayMode);

        pendingMasterVolume = appliedMasterVolume;
        pendingMusic = appliedMusicVolume;
        pendingSFX = appliedSFXVolume;
        masterSlider.SetValueWithoutNotify(appliedMasterVolume);
        musicSlider.SetValueWithoutNotify(appliedMusicVolume);
        sfxSlider.SetValueWithoutNotify(appliedSFXVolume);


    }

    public void OnDisplayModeChanged(int index) { pendingDisplayMode = index; }

    public void OnMasterVolumeChanged(float value)
    {
        Debug.Log("Slider moved: " + value);
        pendingMasterVolume = value;
    }

    public void OnMusicVolumeChanged(float value) { pendingMusic = value; }

    public void OnSFXVolumeChanged(float value) { pendingSFX = value; }

    public void AcceptAndClose()
    {
        Debug.Log("Accept pressed, saving " + pendingMasterVolume);
        ApplySettings();
        gameObject.SetActive(false);

    }

    public void CloseWithoutApplying()
    {
        gameObject.SetActive(false);
    }



    private int CurrentDisplayModeIndex()
    {
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen: return 0;
            case FullScreenMode.Windowed: return 2;
            default: return 1;

        }
    }


    private void ApplySettings()
    {
        int nativeWidth = Display.main.systemWidth;
        int nativeHeight = Display.main.systemHeight;

        switch (pendingDisplayMode)
        {
            case 0:
                Screen.SetResolution(nativeWidth, nativeHeight, FullScreenMode.ExclusiveFullScreen);
                break;
            case 1:
                Screen.SetResolution(nativeWidth, nativeHeight, FullScreenMode.FullScreenWindow);
                break;
            case 2:
                Screen.SetResolution(windowedWidth, windowedHeight, FullScreenMode.Windowed);
                break;
        }

        appliedMasterVolume = pendingMasterVolume;
        appliedMusicVolume = pendingMusic;
        appliedSFXVolume = pendingSFX;
    }

}
