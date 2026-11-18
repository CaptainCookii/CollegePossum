using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using FMODUnity;

public class PinballManager : MonoBehaviour
{
    // Instance and Reference variables
    public static PinballManager Instance;
    [SerializeField] private GameObject roundEndUI;
    [SerializeField] private TMP_Text instructions;
    [SerializeField] private TMP_Text timerUI;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform[] chutes;
    [SerializeField] private EventReference chuteLaunchSound;

    [SerializeField] private GameObject francineDialogue;
    [SerializeField] private GameObject NPCDialogue;
    [SerializeField] private GameObject[] uhmDialogues;

    [SerializeField] private Transform leftSpawn;
    [SerializeField] private Transform rightSpawn;

    [SerializeField] private GameObject leftBubble;
    [SerializeField] private GameObject rightBubble;

    [SerializeField] private float spawnInterval = 7f;
    public DidHit hit;
    private List<GameObject> activeObjects = new List<GameObject>();
    private Dictionary<GameObject, Coroutine> timers =
       new Dictionary<GameObject, Coroutine>();

    // Cool variables
    [SerializeField] private TMP_Text coolText;
    public float cool;

    // Anxiety variables
    [SerializeField] private Image anxietyMeter;
    private float maxAnxiety;
    private float currAnxiety;

    // General variables
    
    private int activeBalls = 0;
    private int destroyedBalls = 0;
    private int maxBallsPerRound = 0;
    [SerializeField] private float force = 8f;

    [SerializeField] private int currentTopicBalls = 5;

    [SerializeField] private GameObject panicCanvas;
    private bool gameOver = false;
    
    // Single instance per scene, does not persist on load
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOver = false;

        //Update vars from GameManager
        cool = GameManager.Instance.currCoolPoints;
        maxAnxiety = GameManager.Instance.maxAnxiety;
        currAnxiety = GameManager.Instance.currAnxiety;
        maxBallsPerRound = currentTopicBalls;

        // Check if the player has already lost before starting the game
        if (GameManager.Instance.currAnxiety > maxAnxiety)
        {
            Debug.Log("Game Over from start: " + currAnxiety / maxAnxiety);
            instructions.enabled = false;
            instructions.transform.GetChild(0).gameObject.SetActive(false);
            coolText.enabled = false;
            timerUI.enabled = false;
            StartCoroutine(GameOverSequence());
            return;
        }

        //Update cool UI text
        UpdateUI();

        //Start pregame sequence
        StartSequence();
    }

    private void Update()
    {
        // [TEMP DEV FORCE EXIT]
        if (Keyboard.current.jKey.isPressed)
        {
            GameOver();
        }

        // [TEMP SCORING/ANXIETY FUNCTION]
        // Commented for now.
        // Every second add cool and anxiety equal to number of active balls

        // Currently: If the ball collides with a piece, increase cool.

        // if (activeBalls > 0)
        // {
        //     // timer += Time.deltaTime;

        //     // if (timer >= 1f)
        //     // {
        //     //     cool += activeBalls;
        //     //     timer = 0f;
        //     // }

        //     currAnxiety += activeBalls * Time.deltaTime;
        // }

        if (currAnxiety/maxAnxiety > .25)
        {
            MusicManager.Instance.SetAnxietyLayer(1);
        } else if (currAnxiety/maxAnxiety > .5)
        {
            MusicManager.Instance.SetAnxietyLayer(2);
        } else if (currAnxiety/maxAnxiety > .75)
        {
            MusicManager.Instance.SetAnxietyLayer(3);
        } else
        {
            MusicManager.Instance.SetAnxietyLayer(0);
        }

            //Update UI
            UpdateUI();

        //Check to see if round should end
        checkGameState();
    }

    // Updates cool text.
    private void UpdateUI()
    {
        if (coolText != null)
        {
            coolText.text = "Cool: " + Mathf.Floor(cool);
        }
        if (anxietyMeter != null)
        {
            anxietyMeter.fillAmount = Mathf.Clamp01(currAnxiety / maxAnxiety);
        }
        
    }

    private void checkGameState()
    {
        if (currAnxiety/maxAnxiety >= 1)
        {
            GameOver();
        }
    }

    // Sets the activity of on-screen UI elements, calls Sequence
    // Coroutine to begin.
    void StartSequence()
    {
        instructions.gameObject.SetActive(false);
        timerUI.gameObject.SetActive(false);
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        // Sets the instructions, creates a 3 second delay, removes instructions
        // then begins ball timerUI.

        instructions.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        instructions.gameObject.SetActive(false);

        timerUI.gameObject.SetActive(true);

        // Ball timerUI that ticks down one number per second.
        for (int i = 3; i > 0; i--)
        {
            timerUI.text = "Conversation Begins In: " + i + "s";
            yield return new WaitForSeconds(1f);
        }

        // Removes timerUI from visibility once it hits 0 seconds.
        timerUI.gameObject.SetActive(false);

        yield return StartCoroutine(ShootSequence());
    }

    // Shoot sequence!! For now, it shoots the first ball when the timerUI ends.
    // Then, each ball comes 2.5 seconds after the previous. Change the
    // value in Line 77 to edit.
    private IEnumerator ShootSequence()
    {
        NPCDialogue.SetActive(true);
        for (int i = 0; i < currentTopicBalls; i++)
        {
            bool left = Random.Range(0, 2) == 0;

            GameObject p;
            Transform s;

            if (left)
            {
                p = leftBubble;
                s = leftSpawn;
            }
            else
            {
                p = rightBubble;
                s = rightSpawn;
            }

            Instantiate(p, s.position, Quaternion.identity);

            if (i < currentTopicBalls - 1)
            {
                yield return new WaitForSeconds(spawnInterval);
            }
    
        }

        francineDialogue.SetActive(true);
    }

    // Restarts the game when called, currently when "Play Again" is selected.
    public void RestartSequence()
    {
        StartCoroutine(Sequence());
    }


    // Next two functions: tracking balls in play
    public void AddBall()
    {
        activeBalls++;
    }

    // Removes a ball from the active list and adds it to the destroyed list.
    public void RemoveBall()
    {
        activeBalls--;
        destroyedBalls++;

        if (destroyedBalls >= maxBallsPerRound)
        {
            EndRound();
        }
    }

    // When the paddle is hit.
    public void PaddleHit()
    {
        currAnxiety += 2;
        if (hit.collided)
        {
            ActivateFranText();
        }
        else
        {
            ActivateUhms();
        }
    }

    // Activates Fran's Text.
    public void ActivateFranText()
    {
        StartCoroutine(ActivateForSeconds());
    }

    // Wait time for text activity.
    private IEnumerator ActivateForSeconds()
    {
        francineDialogue.SetActive(true);

        yield return new WaitForSeconds(3f);

        francineDialogue.SetActive(false);
    }

    // Activates Fran's Uhms.
    public void ActivateUhms()
    {
        // Find inactive objects
        List<GameObject> availableObjects = new List<GameObject>();

        foreach (GameObject obj in uhmDialogues)
        {
            if (obj != null && !obj.activeSelf)
                availableObjects.Add(obj);
        }

        if (availableObjects.Count == 0)
            return;

        // Select a random inactive object
        GameObject selected =
            availableObjects[Random.Range(0, availableObjects.Count)];

        // If 3 are active, deactivate the oldest immediately
        if (activeObjects.Count >= 3)
        {
            GameObject oldest = activeObjects[0];

            if (timers.TryGetValue(oldest, out Coroutine timer))
            {
                StopCoroutine(timer);
                timers.Remove(oldest);
            }

            oldest.SetActive(false);
            activeObjects.RemoveAt(0);
        }

        // Activate the selected object
        selected.SetActive(true);
        activeObjects.Add(selected);

        // Start its 3-second timer
        timers[selected] = StartCoroutine(DeactivateAfterThreeSeconds(selected));
    }

    // Turns text off.
    private IEnumerator DeactivateAfterThreeSeconds(GameObject obj)
    {
        yield return new WaitForSeconds(3f);

        obj.SetActive(false);
        activeObjects.Remove(obj);
        timers.Remove(obj);
    }

    // When round is complete, allow players to restart pregame sequence
    // or exit the conversation.
    private void EndRound()
    {
        activeBalls = 0;
        roundEndUI.SetActive(true);

        // add lockout function to stop all player unputs except for interacting with the UI

    }

    // This is where they lose.
    public void GameOver()
    {
        if (gameOver)
        {
            return;
        }

        gameOver = true;

        Debug.Log("Game Over from function: " + currAnxiety/maxAnxiety);

        StartCoroutine(GameOverSequence());
        // GameManager.Instance.currCoolPoints += cool;
        // GameManager.Instance.totalCoolPoints += cool;
        // GameManager.Instance.currAnxiety += currAnxiety;
        // MusicManager.Instance.SetMusicLayer("Pachinko", false);
        // SceneManager.LoadScene("YarnSpinnerPrototype");
        // Debug.Log("game over");
    }

    // Allows for the panic screen.
    private IEnumerator GameOverSequence()
    {
        GameManager.Instance.currCoolPoints += cool;
        GameManager.Instance.totalCoolPoints += cool;
        GameManager.Instance.currAnxiety = 0;
        MusicManager.Instance.SetMusicLayer("Pachinko", false);
        MusicManager.Instance.SetAnxietyLayer(0);

        coolText.gameObject.SetActive(false);
        francineDialogue.SetActive(false);
        foreach (GameObject uhm in uhmDialogues)
        {
            uhm.SetActive(false);
        }

        DisablePaddles();

        NPCDialogue.SetActive(false);
        panicCanvas.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("ClassesMenu");

    }

    // Disables paddles when panic screen loads.
    private void DisablePaddles()
    {
        PaddleController[] paddles = FindObjectsByType<PaddleController>(FindObjectsSortMode.None);
        foreach (PaddleController pc in paddles)
        {
            pc.enabled = false;
        }
    }
   

    // If the player has not lost and chooses to play Pinball again.
    public void PlayAgain()
    {
        roundEndUI.SetActive(false);
        maxBallsPerRound = currentTopicBalls;
        destroyedBalls = 0;
        RestartSequence();
    }

    public void ExitConversation()
    {
        GameManager.Instance.currCoolPoints += cool;
        GameManager.Instance.totalCoolPoints += cool;
        GameManager.Instance.currAnxiety += currAnxiety;
        MusicManager.Instance.SetMusicLayer("Pachinko", false);
        Debug.Log("Exiting conversation, " + "Cool: " + cool + " Anxiety: " + currAnxiety);
        SceneManager.LoadScene("YarnSpinnerPrototype");
    }

}
