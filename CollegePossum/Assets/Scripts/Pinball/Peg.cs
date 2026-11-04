using NUnit.Framework.Interfaces;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class Peg : MonoBehaviour
{
    public enum Color
    {
        Blue,
        Pink 
    }

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


    [Header("Piece Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int pieceHealth;

    private void Awake()
    {
        mainCamera = Camera.main; 
        objectCollider = GetComponent<PolygonCollider2D>();

        Vector2[] pts = objectCollider.points;

        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        pieceHealth = maxHealth;
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Ball"))
        {
            if (hitSounds != null && hitSounds.Length > 0)
            {
                RuntimeManager.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)], transform.position);
            }
            
            DealDamage(34);
        }
    }


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
    }

    // For now, puts piece back into playing field. Can be moved again 
    // if new round starts.
    private void BreakPiece()
    {
        pieceHealth = maxHealth;
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
    }
 
}
