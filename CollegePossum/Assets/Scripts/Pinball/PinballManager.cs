using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using FMODUnity;
using TMPro.EditorUtilities;

public class PinballManager : MonoBehaviour
{
    //Instance and Reference variables
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
    public DidHit hit;
    private List<GameObject> activeObjects = new List<GameObject>();
    private Dictionary<GameObject, Coroutine> timers =
       new Dictionary<GameObject, Coroutine>();

    //Cool variables
    [SerializeField] private TMP_Text coolText;
    public float cool;

    //Anxiety variables
    [SerializeField] private Image anxietyMeter;
    private float maxAnxiety;
    private float currAnxiety;

    //General variables
    private float timer = 0f;
    private int activeBalls = 0;
    private int destroyedBalls = 0;
    private int maxBallsPerRound = 0;
    [SerializeField] private float force = 8f;
    
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
        //Update vars from GameManager
        cool = GameManager.Instance.currCoolPoints;
        maxAnxiety = GameManager.Instance.maxAnxiety;
        GameManager.Instance.currAnxiety *= .8f;
        currAnxiety = GameManager.Instance.currAnxiety;
        maxBallsPerRound = chutes.Length;

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

        //     // currAnxiety += activeBalls * Time.deltaTime;
        // }

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
        for (int i = 15; i > 0; i--)
        {
            timerUI.text = "Balls Launched In: " + i + "s";
            yield return new WaitForSeconds(1f);
        }

        // Removes timerUI from visibility once it hits 0 seconds.
        timerUI.gameObject.SetActive(false);

        LockPieces();
        yield return StartCoroutine(ShootSequence());
    }

    // Locks pieces when timerUI ends and disables the dragging of them.
    private void LockPieces()
    {
        Piece[] pieces = FindObjectsByType<Piece>(FindObjectsSortMode.None);
        foreach (Piece piece in pieces)
        {
            piece.LockPiece();
        }
    }

    // Shoot sequence!! For now, it shoots the first ball when the timerUI ends.
    // Then, each ball comes 2.5 seconds after the previous. Change the
    // value in Line 77 to edit.
    private IEnumerator ShootSequence()
    {
        NPCDialogue.SetActive(true);
        for (int i = 0; i < chutes.Length; i++)
        {
            Transform chute = chutes[i];

            GameObject ball = Instantiate(ballPrefab, chute.position, chute.rotation);
            RuntimeManager.PlayOneShot(chuteLaunchSound, transform.position);
            AddBall();

            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(chute.right * force, ForceMode2D.Impulse);
            }

            if (i < chutes.Length - 1)
            {
                yield return new WaitForSeconds(2.5f);
            }
        }
        francineDialogue.SetActive(true);
        RetractChutes();
    }

    // Tells the chutes to retract.
    private void RetractChutes()
    {
        foreach (Transform chute in chutes)
        {
            ChuteInlay inlay = chute.GetComponentInParent<ChuteInlay>();
            if (inlay != null)
            {
                inlay.RetractChute();
            }
            else
            {
                Debug.Log("no script found");
            }
            ;
        }
    }

    // Tells the chutes to extend.
    private void ExtendChutes()
    {
        foreach (Transform chute in chutes)
        {
            ChuteInlay inlay = chute.GetComponentInParent<ChuteInlay>();
            if (inlay != null)
            {
                inlay.ExtendChute();
            }
            else
            {
                Debug.Log("no script found");
            }
        }
    }

    // Restarts the game when called, currently when "Play Again" is selected.
    public void RestartSequence()
    {
        ExtendChutes();
        StartCoroutine(Sequence());
    }


    // Next two functions: tracking balls in play
    public void AddBall()
    {
        activeBalls++;
    }

    public void RemoveBall()
    {
        activeBalls--;
        destroyedBalls++;

        if (destroyedBalls >= maxBallsPerRound)
        {
            EndRound();
        }
    }

    public void LeverHit()
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

    public void ActivateFranText()
    {
        StartCoroutine(ActivateForSeconds());
    }

    private IEnumerator ActivateForSeconds()
    {
        francineDialogue.SetActive(true);

        yield return new WaitForSeconds(3f);

        francineDialogue.SetActive(false);
    }

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
        GameManager.Instance.currCoolPoints += cool;
        GameManager.Instance.totalCoolPoints += cool;
        GameManager.Instance.currAnxiety += currAnxiety;
        MusicManager.Instance.SetMusicLayer("Pachinko", false);
        SceneManager.LoadScene("YarnSpinnerPrototype");
        Debug.Log("game over");
    }

    // If the player has not lost and chooses to play Pinball again.
    public void PlayAgain()
    {
        roundEndUI.SetActive(false);
        maxBallsPerRound = chutes.Length;
        destroyedBalls = 0;
        UnlockPieces();
        RestartSequence();
    }

    // Unlocks pieces when sequence is restarted.
    private void UnlockPieces()
    {
        Piece[] pieces = FindObjectsByType<Piece>(FindObjectsSortMode.None);
        foreach (Piece piece in pieces)
        {
            piece.enabled = true;
        }
    }
}
