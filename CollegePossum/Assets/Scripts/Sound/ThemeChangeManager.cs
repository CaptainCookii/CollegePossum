using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeChangeManager : MonoBehaviour
{
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