using UnityEngine;

public class Ball : MonoBehaviour
{
    // Ball Variables.
    private float stuckTime = 1.5f;
    private float stuckSpeed = 0.1f;
    private float unstuckForce = 10f;
    
    private Rigidbody2D rb;
    private float stuckTimer = 0f;

    public bool touchedByPaddle = false;

    // Finds the Ball's Rigidbody.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // If the ball is stuck, add a force in a randomized direction until it
        // becomes unstuck.

        if (rb.linearVelocity.magnitude < stuckSpeed)
        {
            stuckTimer += Time.deltaTime;

            if (stuckTimer >= stuckTime)
            {
                Vector2 dir = Random.insideUnitCircle.normalized;
                rb.AddForce(dir * unstuckForce, ForceMode2D.Impulse);
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
    }

    // Checks if ball touched paddle. This products peg HP, disables the black hole,
    // and disables score until set as true.
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Paddle"))
        {
            touchedByPaddle = true;
            Debug.Log("ball touched paddle!");
        }
    }
}
