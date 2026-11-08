using UnityEngine;
using FMODUnity;

public class BoundsDetector : MonoBehaviour
{
    [SerializeField] private EventReference pinballFallSound;
    private void OnTriggerEnter2D(Collider2D c)
    {
        // If the ball is no longer in play, inform our scripts that 
        // rely on that information.
        Ball ball = c.gameObject.GetComponent<Ball>();
        if (c.CompareTag("Ball") && ball.touchedByPaddle)
        {
            RuntimeManager.PlayOneShot(pinballFallSound, transform.position);
            PinballManager.Instance.RemoveBall();
            Destroy(c.gameObject);
        }
    }
}
