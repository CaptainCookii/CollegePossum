using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class LeverController : MonoBehaviour
{

    // Enum to distinguish which side is which.
    public enum Side
    {
        Left,
        Right 
    }

    [SerializeField] private Side side;
    [SerializeField] private Transform pivot;

    [SerializeField] private float pressAddAngle = 65f;
    [SerializeField] private float flippedUpSpeed = 1200f;
    [SerializeField] private float returnDownSpeed = 1800f;

    private Rigidbody2D rb;
    private float restAngle;
    private float movementAngle;
    private float currentAngle;

    private Vector3 os;


    // Stores hinge component and attacks motor to hinge.
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        restAngle = transform.eulerAngles.z;

        if (restAngle > 180f)
        {
            restAngle -= 360f;
        }

        if (side == Side.Left)
        {
            movementAngle = restAngle + pressAddAngle;
        }
        else
        {
            movementAngle = restAngle - pressAddAngle;
        }

        currentAngle = restAngle;
        os = transform.position - pivot.position;
    }

    // Moves the levers their respective directions with their respective forces.
    // Angles are edited in Unity engine.
    void Update()
    {
        bool keyPressed = false;
        bool keyPressedThisFrame = false;

        if (side == Side.Left)
        {
            keyPressed = Keyboard.current.leftArrowKey.isPressed;
            keyPressedThisFrame = Keyboard.current.leftArrowKey.wasPressedThisFrame;
        }
        else if (side == Side.Right)
        {
            keyPressed = Keyboard.current.rightArrowKey.isPressed;
            keyPressedThisFrame = Keyboard.current.rightArrowKey.wasPressedThisFrame;
        }

        // Adds to anxiety per lever hit.
        if (keyPressedThisFrame)
        {
            AnxietyManager am = FindFirstObjectByType<AnxietyManager>();
            am.LeverHit();
        }

        float goalAngle;
        if (keyPressed)
        {
            goalAngle = movementAngle;
        } 
        else
        {
            goalAngle = restAngle;
        }

        float speed;
        if (keyPressed)
        {
            speed = flippedUpSpeed;
        } 
        else
        {
            speed = returnDownSpeed;
        }

        currentAngle = Mathf.MoveTowardsAngle(currentAngle, goalAngle, speed * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        float angleChange = currentAngle - restAngle;
        Vector3 ros = Quaternion.Euler(0, 0, angleChange) * os;
        rb.MovePosition(pivot.position + ros);
        rb.MoveRotation(currentAngle);
    }
}
