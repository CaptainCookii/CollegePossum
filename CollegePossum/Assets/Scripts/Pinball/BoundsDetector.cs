using UnityEngine;
using FMODUnity;

public class BoundsDetector : MonoBehaviour
{
    [SerializeField] private EventReference pinballFallSound;
    private void OnTriggerEnter2D(Collider2D c)
    {
        // If the ball is no longer in play, inform our scripts that 
        // rely on that information.
        if (c.CompareTag("Ball"))
        {
            RuntimeManager.PlayOneShot(pinballFallSound, transform.position);
            PinballManager.Instance.RemoveBall();
            Destroy(c.gameObject);
        }
    }
}
