using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class PregameTimer : MonoBehaviour
{

    [SerializeField] private TMP_Text instructions;
    [SerializeField] private TMP_Text timer;

    
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
        for (int i = 20; i > 0; i--)
        {
            timer.text = "Balls Drop In: " + i + "s";
            yield return new WaitForSeconds(1f);
        }

        // Removes timer from visibility once it hits 0 seconds.
        timer.gameObject.SetActive(false);

    }
}
