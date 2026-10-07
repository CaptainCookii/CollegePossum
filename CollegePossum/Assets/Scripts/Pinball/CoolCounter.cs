using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text coolText;

    private int cool = 0;
    private float timer = 0f;
    private int activeBalls = 0;

    private void Start()
    {
        UpdateCool();
    }

    // Constantly updating score based on active balls.
    private void Update()
    {
        if (activeBalls > 0)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                cool += activeBalls;
                timer = 0f;
                UpdateCool();
            }
        }
        
    }

    // Adds and removes balls.
    public void AddBall()
    {
        activeBalls++;
    }

    public void RemoveBall()
    {
        activeBalls--;
    }

    // Updates cool text.
    private void UpdateCool()
    {
        coolText.text = "Cool: " + cool;
    }
}
