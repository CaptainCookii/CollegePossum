using System;
using System.Collections;
using UnityEngine;

public class DidHit : MonoBehaviour
{
    public bool collided;
    public float bufferTime = 0.1f;

    private bool addCool = true;

    private Coroutine exitCoroutine;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cancel the countdown if we touch something again
        if (collision.gameObject.CompareTag("Ball"))
        {
    
            if (exitCoroutine != null)
            {
                StopCoroutine(exitCoroutine);
                exitCoroutine = null;
            }

            collided = true;

            if (addCool)
            {
                PinballManager.Instance.cool++;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Wait before setting collided to false
            if (exitCoroutine != null)
                StopCoroutine(exitCoroutine);

            exitCoroutine = StartCoroutine(ResetCollision());
        }
    }

    private IEnumerator ResetCollision()
    {
        yield return new WaitForSeconds(bufferTime);

        collided = false;
        exitCoroutine = null;
    }

    public bool GetCollided()
    {
        return collided;
    }
}
