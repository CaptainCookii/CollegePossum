using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum TurnState
{
    PlayerPlacing,
    PlayerDropping
}

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public TurnState currentTurn;

    [Header("Settings")]
    public int ballsPerTurn = 5;

    private int ballsRemainingToDrop;
    private int ballsCurrentlyActive;

    [Header("Piece Prefabs")]
    public GameObject playerPiecePrefab;
    public GameObject cpuPiecePrefab;


    //Anxiety 
    public int anxietyPerSec = 1;
    int progress = 0;
    [SerializeField] public Slider anxietyBar;
    [SerializeField] public float anxietyCooldown = 1f;
    private float anxietyTimer;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartPlayerTurn();
    }
    void StartPlayerTurn()
    {
        currentTurn = TurnState.PlayerPlacing;
        Debug.Log("Player Placing Phase");
    }

    public void BeginBallPhase()
    {
        ballsRemainingToDrop = ballsPerTurn;
        ballsCurrentlyActive = 0;

        if (currentTurn == TurnState.PlayerPlacing)
            currentTurn = TurnState.PlayerDropping;

        Debug.Log("Ball Dropping Phase");
    }

    public void OnBallSpawned()
    {
        ballsRemainingToDrop--;
        ballsCurrentlyActive++;
    }

    public void OnBallDestroyed()
    {
        ballsCurrentlyActive--;

        //if out of topics go to party 
        if (GameManager.Instance.totalTopicsLeft <= 0)
        {
            LoseController.Instance.Party();
        }
        else if (ballsCurrentlyActive <= 0 && ballsRemainingToDrop <= 0)
        {
            EndTurn();
        }
    }

    void EndTurn()
    {
        if (currentTurn == TurnState.PlayerDropping)
            StartPlayerTurn();
    }

    public GameObject GetCurrentPiecePrefab()
    {
        if (currentTurn == TurnState.PlayerPlacing)
            return playerPiecePrefab;

        return null;
    }

    public bool CanDropBall()
    {
        return  currentTurn == TurnState.PlayerDropping
                && ballsRemainingToDrop > 0;
    }

    void Update()
    {

        if (currentTurn == TurnState.PlayerPlacing)
        {
            // only do anxiety when player is placing
            anxietyTimer -= Time.deltaTime;
            
            // if timer goes off then add anxiety to their meter
            if (anxietyTimer <= 0f)
            {
                UpdateAnxiety(anxietyPerSec);
                anxietyTimer = anxietyCooldown;
            }
            //if at max 
            if (progress >= anxietyBar.maxValue)
            {
                //PANIC!!!!!!
                LoseController.Instance.Panic();
                
                //CHANGE
                GameObject.Find("Main Camera").GetComponent<ThemesChangeTrigger>().Off();
            }
        }
    }


    // jacobs code below 
    public void OnSliderChanged(float value)
    {
        //valueText.text = "Anxiety: " + value.ToString();
        Debug.Log("Anxiety: " + value.ToString());
    }

    public void UpdateAnxiety(int value)
    {
        progress += value;

        if (anxietyBar != null)
        {
            anxietyBar.value = progress;
            //OnSliderChanged(progress);
        }
    }
}
