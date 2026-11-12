using System;
using System.Runtime.CompilerServices;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Bubble : MonoBehaviour
{

    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private GameObject ball;

    private Vector2 spawnVector;
    private bool popped = false;
    private float horizontal;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        spawnVector = new Vector2(UnityEngine.Random.Range(0.7f, 1f), UnityEngine.Random.Range(0f, 1f));

        if (gameObject.name.Contains("Left"))
        {
            horizontal = 1f;
        }
        else
        {
            horizontal = -1f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spawnVector.x = Mathf.Clamp01(spawnVector.x - 0.5f * Time.deltaTime);
        spawnVector.y = Mathf.Clamp01(spawnVector.y + 0.5f * Time.deltaTime);

        Vector2 direction = new Vector2(spawnVector.x * horizontal * 5f, spawnVector.y);
        rb.MovePosition(rb.position + direction.normalized * floatSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("PopZone") && !popped)
        {
        Pop();
        }
    }

    private void Pop()
    { 
        popped = true;

        Instantiate(ball, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }
}
