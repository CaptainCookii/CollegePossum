using NUnit.Framework.Interfaces;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Piece : MonoBehaviour
{
    private Camera mainCamera;
    private bool draggingOn;
    private Vector3 movementOffset;

    private PolygonCollider2D objectCollider;
    private Vector3 storedPosition;
    

    [Header("Piece Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int pieceHealth;
    
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private float touchTimer = 0f;
    private bool ballOnPiece = false;

    
    // Sets and stores camera and PolygonCollider2D components for use later on.
    private void Awake()
    {
        mainCamera = Camera.main; 
        objectCollider = GetComponent<PolygonCollider2D>();

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        pieceHealth = maxHealth;
    }

    private void Update()
    {
    
        if (ballOnPiece)
        {
            touchTimer += Time.deltaTime;

            if (touchTimer >= 10f)
            {
                RespawnPiece();
                ballOnPiece = false;
                touchTimer = 0f;
            }
        }

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

    }

    // If a piece is hit by a ball, for now, deal 10 damage. Number is
    // easily changed.
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ball"))
        {
            DealDamage(10);
        }
    }

    private void OnCollisionStay2D(Collision2D c) {
        if (c.gameObject.CompareTag("Ball"))
        {
            touchTimer += Time.deltaTime;
            if (touchTimer >= 10f)
            {
                RespawnPiece();
                touchTimer = 0f;
            }
        }
        
    }

    // Resets touching piece tag.
    private void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ball"))
        {
            touchTimer = 0f;
        }
    }

    // Does damage to a piece based on hit
    public void DealDamage(int damage)
    {
        pieceHealth -= damage;

        if (pieceHealth <= 0)
        {
            RespawnPiece();
        }
    }

    // For now, puts piece back into playing field. Can be moved again 
    // if new round starts.
    private void RespawnPiece()
    {
        pieceHealth = maxHealth;
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        draggingOn = false;
        
    }

    // Helper function that checks if pieces are overlapping with other pieces
    // or GameObjects when the mouse is released.
    private bool IsOverlapping()
    {
        ContactFilter2D overlapCheck = new ContactFilter2D();
        overlapCheck.useTriggers = true;

        // should hopefully not need 10, can be raised
        PolygonCollider2D[] collisions = new PolygonCollider2D[10];

        int n = objectCollider.Overlap(overlapCheck, collisions);

        for (int i = 0; i < n; i++)
        {
            if (collisions[i] != objectCollider)
            {
                if (collisions[i].CompareTag("Chute") || !collisions[i].isTrigger)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
