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
    
    // Sets and stores camera and PolygonCollider2D components for use later on.
    private void Awake()
    {
        mainCamera = Camera.main; 
        objectCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        // If the mouse is not in use, nothing happens.
        if (Mouse.current == null)
        {
            return;
        }

        // Unity's conversion from screen to world points.
        Vector2 screenCoords = Mouse.current.position.ReadValue();
        Vector3 worldCoords = mainCamera.ScreenToWorldPoint(new Vector3(screenCoords.x, screenCoords.y, -mainCamera.transform.position.z));

        // If the mouse was pressed over a piece's collider, enable dragging for the piece.
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Collider2D[] hits = Physics2D.OverlapPointAll(worldCoords);

            foreach (Collider2D hit in hits)
            {
                if (hit.gameObject == gameObject)
                {
                    draggingOn = true;
                    storedPosition = transform.position;
                    movementOffset = transform.position - worldCoords;
                    break;
                }  
            }
        }

        // If dragging is enabled, update the piece's position accordingly.
        if (draggingOn && Mouse.current.leftButton.isPressed)
        {
            transform.position = worldCoords + movementOffset;
        }

        // If the piece is released on another piece or a part of the gameplay, 
        // don't permit the drop, and send it back to its last valid location.
        // Otherwise, allow the drop and leave the piece in the new location.

        if (draggingOn && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (IsOverlapping())
            {
                transform.position = storedPosition;
            }
            
            draggingOn = false;
            
        }

        // Supports the rotating of pieces. Currently at a 45 degree angle per
        // rotation, number can easily be changed.
        if (draggingOn && Keyboard.current.rKey.wasPressedThisFrame)
        {
            transform.Rotate(0, 0, 45);
        }
    }

    // Helper function that checks if pieces are overlapping with other pieces
    // or GameObjects when the mouse is released.
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
