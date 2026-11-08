using NUnit.Framework.Interfaces;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class Peg : MonoBehaviour
{
    // Color differentiator.
    public enum Color
    {
        Blue,
        Pink 
    }

    // Peg variables, some of these will be deleted. 
    [SerializeField] public Color pegColor;

    private Camera mainCamera;

    private Vector3 movementOffset;

    private PolygonCollider2D objectCollider;
    private Vector3 storedPosition;

    [SerializeField] private EventReference[] hitSounds;
    [SerializeField] private EventReference[] placeSounds;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private float touchTimer = 0f;

    // Health variables
    [Header("Piece Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int pieceHealth;

    [SerializeField] private SpriteRenderer innerCircle;

    // For now, finds the camera, peg collider, stores position and
    // rotation, health, and updates the color. 
    private void Awake()
    {
        mainCamera = Camera.main; 
        objectCollider = GetComponent<PolygonCollider2D>();

        Vector2[] pts = objectCollider.points;

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        pieceHealth = maxHealth;
        UpdateColor();
    }

    // If piece is hit, play sounds, and deal damage.
    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ball"))
        {
            if (hitSounds != null && hitSounds.Length > 0)
            {
                RuntimeManager.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)], transform.position);
            }

            Ball ball = c.gameObject.GetComponent<Ball>();
            if (ball.touchedByPaddle)
            {
                DealDamage(34);
            }
        
        }
    }

    // Breaks the piece if it's stuck for too long.
    // We have repulsion, so this is an edge case.
    private void OnCollisionStay2D(Collision2D c) {
        if (c.gameObject.CompareTag("Ball"))
        {
            touchTimer += Time.deltaTime;
            if (touchTimer >= 3.5f)
            {
                BreakPiece();
                touchTimer = 0f;
            }
        }
    }

    // Does damage to a piece based on hit
    public void DealDamage(int damage) 
    {
        pieceHealth -= damage;

        if (pieceHealth <= 0)
        {
            BreakPiece();
        }
        else
        {
            UpdateColor();
        }
    }

    // Breaks piece after (for right now) hard-coded 3 hits. 
    // Respawn logic is commented out for the time being.
    private void BreakPiece()
    {
        Destroy(gameObject);
        Debug.Log("bye bye peg");

        // pieceHealth = maxHealth;
        // transform.position = spawnPosition;
        // transform.rotation = spawnRotation;
    }

    // Updates peg color according to peg hp
    private void UpdateColor()
    {
        string hex = "";

        if (pegColor == Color.Pink)
        {
            if (pieceHealth > 66)
            {
                hex = "#FF69B4";
            }
            else if (pieceHealth > 32)
            {
                hex = "#DE688F";
            }
            else
            {
                hex = "#B65674";
            }
        }

        if (pegColor == Color.Blue)
        {
            if (pieceHealth > 66)
            {
                hex = "#5BA8EC";
            }
            else if (pieceHealth > 32)
            {
                hex = "#4782B4";
            }
            else
            {
                hex = "#315C81";
            }
        }

        // I have never tried this before. Converts HEX code to a color that can
        // be used in Unity.
        if (ColorUtility.TryParseHtmlString(hex, out UnityEngine.Color c))
        {
            innerCircle.color = c;
        }

    }

}
