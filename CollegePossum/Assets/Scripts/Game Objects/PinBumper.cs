using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PinBumper : MonoBehaviour
{
    public float baseForce = 8f;
    public float sizeForceMultiplier = 2f;

    public int maxHealth = 1;
    private int currentHealth;

    public PlacementNode placementNode;

    public int coolGain = 2;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //for later in case of placed objects intersecting
        if (!collision.collider.CompareTag("Ball"))
            return;

        //ball that collided with this pin
        Rigidbody2D ballRb = collision.rigidbody;
        if (ballRb == null)
            return;

        // find bounce direction for ball
        Vector2 direction = ((ballRb.position - (Vector2)transform.position).normalized + Vector2.up * 0.5f).normalized;

        // amplify bounce by size
        float sizeFactor = transform.localScale.x;
        float finalForce = baseForce + (sizeFactor * sizeForceMultiplier);

        // set previous veloctiy to zero then apply new force
        ballRb.linearVelocity = Vector2.zero;
        ballRb.AddForce(direction * finalForce, ForceMode2D.Impulse);

        // take damage... duh
        TakeDamage(1);


        // make sound that you got hit 
        // AudioManager.instance.PlayOneShot(FMODEvents.instance.pegHit, this.transform.position);

        // add score for hitting pin
        GameManager.Instance.AddScore(coolGain);

    }

    protected virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;

        //temp visual health effect
        transform.localScale *= 0.9f;

        if (currentHealth <= 0)
        {
            //re-open node spot before destorying pin
            placementNode.isOccupied = false;
            Destroy(gameObject);
        }
    }
}

