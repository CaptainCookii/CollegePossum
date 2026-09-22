using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ClassButtonController: MonoBehaviour
{
    //variable to change what scene they go to or use "" to choose for later

    [SerializeField]
    public TextMeshProUGUI gameMessage;


    public void Start()
    {
        GlobalVars.bankedCool += GlobalVars.cool;
        GlobalVars.cool = 0;
        gameMessage.text = "Cool: " + GlobalVars.bankedCool;
    }

    private void SubtractCool(int amount)
    {
        GlobalVars.bankedCool -= amount;
        gameMessage.text = "Cool: " + GlobalVars.bankedCool;
    }

    public void GoToClass()
    {
        SubtractCool(10);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
