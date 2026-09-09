using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

public class PieceDrag : MonoBehaviour
{
    private Camera mainCamera;
    private bool draggingOn;
    private Vector3 movementOffset;

    private PolygonCollider2D objectCollider;
    private Vector3 storedPosition;
    
    private void Awake()
    {
        mainCamera = Camera.main; 
        objectCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }

        Vector2 screenCoords = Mouse.current.position.ReadValue();
        Vector3 worldCoords = mainCamera.ScreenToWorldPoint(new Vector3(screenCoords.x, screenCoords.y, -mainCamera.transform.position.z));

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Collider2D hit = Physics2D.OverlapPoint(worldCoords);

            if (hit != null && hit.gameObject == gameObject)
            {
                draggingOn = true;
                storedPosition = transform.position;
                movementOffset = transform.position - worldCoords;
            }
        }

        if (draggingOn && Mouse.current.leftButton.isPressed)
        {
            transform.position = worldCoords + movementOffset;
        }

        if (draggingOn && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (IsOverlapping())
            {
                transform.position = storedPosition;
            }
            else
            {
                draggingOn = false;
            }
        }

        if (draggingOn && Keyboard.current.rKey.wasPressedThisFrame)
        {
            transform.Rotate(0, 0, 45);
        }
    }

    private bool IsOverlapping()
    {
        ContactFilter2D overlapCheck = new ContactFilter2D();
        overlapCheck.useTriggers = false;

        // should hopefully not need 10, can be raised
        PolygonCollider2D[] collisions = new PolygonCollider2D[10];

        int n = objectCollider.Overlap(overlapCheck, collisions);

        for (int i = 0; i < n; i++)
        {
            if (collisions[i] != objectCollider)
            {
                return true;
            }
        }

        return false;
    }
}
