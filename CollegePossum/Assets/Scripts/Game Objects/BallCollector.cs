using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BallCollector : MonoBehaviour
{
    private int scoreValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ball"))
            return;

        // collected 1 ball
        GameManager.Instance.SubtractTopic(1);

        GameManager.Instance.AddScore(scoreValue);
        TurnManager.Instance.OnBallDestroyed();
        Destroy(collision.gameObject);
    }
}

