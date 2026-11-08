using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DidHit : MonoBehaviour
{
    // Collision Variables.
    public bool collided;
    public float bufferTime = 0.1f;

    private bool addCool = true;

    private Coroutine exitCoroutine;

    [SerializeField] private GameObject popupPrefab;
    
    // If the ball has collided with a peg, award the correct amount of cool.
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

            Peg peg = GetComponent<Peg>();
            Ball ball = collision.gameObject.GetComponent<Ball>();

            if (addCool && ball.touchedByPaddle)
            {
                if (peg != null && peg.pegColor == Peg.Color.Pink)
                {
                    PinballManager.Instance.cool++;
                    ShowCoolPopup("+1");
                    Debug.Log("+1");
                }

                if (peg.pegColor == Peg.Color.Blue)
                {
                    PinballManager.Instance.cool += 3;
                    ShowCoolPopup("+3");
                    Debug.Log("+3");
                }
            }
        }
    }

    // After the collision has been exited.
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

    // Resets collision state.
    private IEnumerator ResetCollision()
    {
        yield return new WaitForSeconds(bufferTime);

        collided = false;
        exitCoroutine = null;
    }

    // Has there been a collision?
    public bool GetCollided()
    {
        return collided;
    }

    // Call the cool pop-up if a ball has hit a peg.
    private void ShowCoolPopup(string s)
    {
        GameObject c = GameObject.FindGameObjectWithTag("Cool Pop-up Canvas");
        GameObject popup = Instantiate(popupPrefab, c.transform);
        RectTransform popupRect = popup.GetComponent<RectTransform>();
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        popupRect.position = pos;
        popup.GetComponent<CoolPopup>().ShowText(s);
    }

}
