using System.Collections;
using UnityEngine;

public class ChuteInlay : MonoBehaviour
{
    private float retractDist = 2f;
    private float moveTime = 0.5f;

    // I have never used this before, had to look up documentation. Could change this?
    private AnimationCurve moveCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private Vector3 outPosition;
    private Vector3 inPosition;

    private void Awake()
    {
        outPosition = transform.localPosition;

        Vector3 movementDirection;

        if (transform.parent != null)
        {
            movementDirection = transform.parent .InverseTransformDirection(-transform.right);
        }
        else
        {
            movementDirection = -transform.right;
        }
        inPosition = outPosition + (movementDirection * retractDist);
    }

    public void RetractChute()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateChute(inPosition));
    }

    public void ExtendChute()
    {
        StopAllCoroutines();
        StartCoroutine(AnimateChute(outPosition));
    }

    private IEnumerator AnimateChute(Vector3 endPosition)
    {
        Vector3 startPosition = transform.localPosition;
        float timeElapsed = 0f;

        while (timeElapsed < moveTime)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / moveTime);
            float curveVal = moveCurve.Evaluate(t);

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, curveVal);
            yield return null;
        }
        transform.localPosition = endPosition;
    }
}
