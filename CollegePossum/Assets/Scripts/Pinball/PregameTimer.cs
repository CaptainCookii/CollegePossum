using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class PregameTimer : MonoBehaviour
{

    [SerializeField] private TMP_Text instructions;
    [SerializeField] private TMP_Text timer;

    
    void Start()
    {
        instructions.gameObject.SetActive(false);
        timer.gameObject.SetActive(false);
        StartCoroutine(Sequence()); 
    }

    private IEnumerator Sequence()
    {
        instructions.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        instructions.gameObject.SetActive(false);

        timer.gameObject.SetActive(true);

        for (int i = 20; i > 0; i--)
        {
            timer.text = "Balls Drop In: " + i + "s";
            yield return new WaitForSeconds(1f);
        }

        timer.gameObject.SetActive(false);

    }
}
