using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

     // Global Variables
    public int totalSocialPoints = 0;
    public int currSocialPoints = 0;
    public int totalCoolPoints = 0;
    public int currCoolPoints = 0;

    public int mathValue = 0;
    public int socialStudiesValue = 0;
    public int scienceValue = 0;
    public int englishValue = 0;

    // cool from each conversation
    [SerializeField] public TextMeshProUGUI coolMessage;

    // topic balls
    [SerializeField] public TextMeshProUGUI topicsLeftText;
    [SerializeField] private int totalTopicsPerConversation;
    public int totalTopicsLeft; // could get/set this instead of public

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            coolMessage.text = "Cool: " + GlobalVars.cool;
            topicsLeftText.text = "Topics Left: " + totalTopicsPerConversation;
            totalTopicsLeft = totalTopicsPerConversation;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void SubtractTopic(int amount)
    {
        totalTopicsLeft -= amount;
        topicsLeftText.text = "Topics Left: " + totalTopicsLeft;
    }


    public void AddScore(int amount)
    {
        GlobalVars.cool += amount;

        coolMessage.text = "Cool: " + GlobalVars.cool;
        //Debug.Log("Score: " + GlobalVars.cool);
    }
}

