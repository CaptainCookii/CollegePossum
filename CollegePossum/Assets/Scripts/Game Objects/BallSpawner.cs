using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float moveRange = 5f;

    [Header("Ball")]
    public GameObject ballPrefab;
    public float dropCooldown = 0.3f;

    private float startX;
    private float cooldownTimer;

    void Start()
    {
        startX = transform.position.x;
    }

    void Update()
    {
        HandleMovement();
        HandleDrop();
    }

    void HandleMovement()
    {
        float x = startX + Mathf.Sin(Time.time * moveSpeed) * moveRange;
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    void HandleDrop()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) &&
            cooldownTimer <= 0f &&
            TurnManager.Instance.CanDropBall())
        {
            Instantiate(ballPrefab, transform.position, Quaternion.identity);

            TurnManager.Instance.OnBallSpawned();

            cooldownTimer = dropCooldown;
        }
    }
}
