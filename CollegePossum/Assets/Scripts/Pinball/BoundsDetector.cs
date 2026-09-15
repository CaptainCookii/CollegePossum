using UnityEngine;

public class BoundsDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Ball"))
        {
            ScoreCounter sc = FindFirstObjectByType<ScoreCounter>();
            sc.RemoveBall();

            Destroy(c.gameObject);
        }
    }
}
