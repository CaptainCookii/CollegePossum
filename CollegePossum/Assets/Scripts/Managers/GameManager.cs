using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("[TEMPORARY] Round End & Lose Condition UI")]
    [SerializeField] private int activeBalls = 0;
    private bool gameOver = false;

    [SerializeField] private GameObject roundEndUI;
    [SerializeField] private PregameSequence ps;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // coolMessage.text = "Cool: " + GlobalVars.cool;
            // topicsLeftText.text = "Topics Left: " + totalTopicsPerConversation;
            totalTopicsLeft = totalTopicsPerConversation;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnLevelWasLoaded(int level)
    {
        if (!AudioManager.instance.partyOn && level == 1)
        {
            AudioManager.instance.ChangeThemeParty();
            if (AudioManager.instance.pachinkoOn)
            {
                AudioManager.instance.ChangeThemePachinko();
            }
        }
        if (!AudioManager.instance.pachinkoOn && (level == 7 || level == 8 || level == 9))
        {
            AudioManager.instance.ChangeThemePachinko();
        }
    }

    public void SubtractTopic(int amount)
    {
        totalTopicsLeft -= amount;
        //topicsLeftText.text = "Topics Left: " + totalTopicsLeft;
    }


    public void AddScore(int amount)
    {
        GlobalVars.cool += amount;

        //coolMessage.text = "Cool: " + GlobalVars.cool;
        //Debug.Log("Score: " + GlobalVars.cool);
    }

    // [NOTE] This code will not stay like this. This is because we don't have 
    // topic balls right now.

    // Next two functions: tracking balls in play
    public void AddBall()
    {
        activeBalls++;
    }

    public void RemoveBall()
    {
        activeBalls--;

        if (activeBalls <= 0 && !gameOver)
        {
            activeBalls = 0;
            RoundEndUI();
        } 
    }

    // When round is complete, allow players to restart pregame sequence
    // or exit the conversation.
    private void RoundEndUI()
    {
        roundEndUI.SetActive(true); 
    }

    // [FOR MICAH] THIS IS WHERE THEY LOSE.
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            SceneManager.LoadScene("YarnSpinnerPrototype");
        }
    }

    // If the player has not lost and chooses to play Pinball again.
    public void PlayAgain()
    {
        roundEndUI.SetActive(false);

        UnlockPieces();

        ps.RestartSequence();
    }

    // [FOR MICAH] THIS IS WHERE THEY EXIT THE GAME EARLY.
    public void ExitPressed()
    {
        SceneManager.LoadScene("YarnSpinnerPrototype");
    }

    // Unlocks pieces when sequence is restarted.
    private void UnlockPieces()
    {
        PieceDrag[] pieces = FindObjectsByType<PieceDrag>(FindObjectsSortMode.None);
        foreach (PieceDrag piece in pieces)
        {
            piece.enabled = true;
        }
    }


}

