using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class PregameSequence : MonoBehaviour
{

    [SerializeField] private TMP_Text instructions;
    [SerializeField] private TMP_Text timer;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform[] chutes;
    [SerializeField] private float force = 8f;

    // Sets the activity of on-screen UI elements, calls Sequence
    // Coroutine to begin.
    void Start()
    {
        instructions.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        StartCoroutine(Sequence()); 
    }

    private IEnumerator Sequence()
    {
        // Sets the instructions, creates a 3 second delay, removes instructions
        // then begins ball timer.

        instructions.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        instructions.gameObject.SetActive(false);

        timer.gameObject.SetActive(true);

        // Ball timer that ticks down one number per second.
        for (int i = 10; i > 0; i--)
        {
            timer.text = "Balls Drop In: " + i + "s";
            yield return new WaitForSeconds(1f);
        }

        // Removes timer from visibility once it hits 0 seconds.
        timer.gameObject.SetActive(false);

        LockPieces();
        yield return StartCoroutine(ShootSequence());
    }

    // Shoot sequence!! For now, it shoots the first ball when the timer ends.
    // Then, each ball comes 2.5 seconds after the previous. Change the
    // value in Line 77 to edit.
    private IEnumerator ShootSequence()
    {
        ScoreCounter sc = FindFirstObjectByType<ScoreCounter>();
        AnxietyManager am = FindFirstObjectByType<AnxietyManager>();

        for (int i = 0; i < chutes.Length; i++)
        {
            Transform chute = chutes[i];

            GameObject ball = Instantiate(ballPrefab, chute.position, chute.rotation);
            GameManager.Instance.AddBall();

            sc.AddBall();
            am.AddBall();

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
    }

    // Locks pieces when timer ends and disables the dragging of them.
    private void LockPieces()
    {
        PieceDrag[] pieces = FindObjectsByType<PieceDrag>(FindObjectsSortMode.None);

        foreach (PieceDrag piece in pieces)
        {
            piece.enabled = false;
        }
        
    }

    // Restarts the game when called, currently when "Play Again" is selected.
    public void RestartSequence()
    {
        StartCoroutine(Sequence());
    }
}
