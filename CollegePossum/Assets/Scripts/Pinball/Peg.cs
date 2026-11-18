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
        Pink,
        Green,
        Orange,
        Purple
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
    
    [SerializeField] private GameObject crack1;
    [SerializeField] private GameObject crack2;
    [SerializeField] private GameObject crack3;

    private int crackCount = 0;

    // For now, finds the camera, peg collider, stores position and
    // rotation, health, and updates the color. 
    private void Awake()
    {
        mainCamera = Camera.main; 
        objectCollider = GetComponent<PolygonCollider2D>();

        Vector2[] pts = objectCollider.points;

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;

        crack1.SetActive(false);
        crack2.SetActive(false);
        crack3.SetActive(false);
       
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
                AddCrack();
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
    private void AddCrack()
    {
        crackCount++;

        if (crackCount == 1)
        {
            crack1.SetActive(true);
        }
        else if (crackCount == 2)
        {
            crack2.SetActive(true);
        }
        else if (crackCount == 3)
        {
            crack3.SetActive(true);
        }
        else
        {
            BreakPiece();
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


}
