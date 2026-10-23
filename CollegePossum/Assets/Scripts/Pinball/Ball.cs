using UnityEngine;

public class Ball : MonoBehaviour
{

    private float stuckTime = 3f;
    private float stuckSpeed = 0.1f;
    private float unstuckForce = 10f;
    
    private Rigidbody2D rb;
    private float stuckTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
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
}
