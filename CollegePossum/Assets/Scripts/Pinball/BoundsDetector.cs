using UnityEngine;

public class BoundsDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        // If the ball is no longer in play, inform our scripts that 
        // rely on that information.
        if (c.CompareTag("Ball"))
        {
            GameManager.Instance.RemoveBall();
            
            ScoreCounter sc = FindFirstObjectByType<ScoreCounter>();
            sc.RemoveBall();

            AnxietyManager am = FindFirstObjectByType<AnxietyManager>();
            am.RemoveBall();

            Destroy(c.gameObject);
        }
    }
}
