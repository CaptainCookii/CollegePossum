using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{
    
    //variable to change what scene they go to or use "" to choose for later
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        //SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
