using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private int score = 0;
    private float timer = 0f;
    private int activeBalls = 0;

    private void Start()
    {
        UpdateScore();
    }

    private void Update()
    {
        if (activeBalls > 0)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                score += activeBalls;
                timer = 0f;
                UpdateScore();
            }
        }
        
    }

    public void AddBall()
    {
        activeBalls++;
    }

    public void RemoveBall()
    {
        activeBalls--;
    }

    private void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
