using UnityEngine;
using UnityEngine.InputSystem;

public class LeverController : MonoBehaviour
{

    // Enum to distinguish which side is which.
    public enum Side
    {
        Left,
        Right 
    }

    [SerializeField] private Side side;
    [SerializeField] private float force = 1000f;
    [SerializeField] private float stoppedForce = 500f;

    private HingeJoint2D hinge;
    private JointMotor2D motor;


    // Stores hinge component and attacks motor to hinge.
    private void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();
        motor = hinge.motor;
    }

    // Moves the levers their respective directions with their respective forces.
    // Angles are edited in Unity engine.
    void Update()
    {
        bool keyPressed = false;
        bool keyPressedThisFrame = false;

        if (side == Side.Right)
        {
            keyPressed = Keyboard.current.leftArrowKey.isPressed;
            keyPressedThisFrame = Keyboard.current.leftArrowKey.wasPressedThisFrame;
        }
        else if (side == Side.Left)
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

        if (keyPressed)
        {
            if (side == Side.Left)
            {
                motor.motorSpeed = force;
            }
            else
            {
                motor.motorSpeed = -force;
            }
        }
        else
        {
            if (side == Side.Left)
            {
                motor.motorSpeed = -stoppedForce;
            }
            else
            {
                motor.motorSpeed = stoppedForce;
            } 
        }

        hinge.motor = motor;  
    }
}
