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

        SpawnBalls();
    }

    // Spawns the balls out of shooter.
    private void SpawnBalls()
    {
        foreach (Transform chute in chutes)
        {
            if (chute == null)
            {
                continue;
            }

            GameObject ball = Instantiate(ballPrefab, chute.position, chute.rotation);

            Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(chute.right * force, ForceMode2D.Impulse);
            }
        }
    }
}
