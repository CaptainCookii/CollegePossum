using UnityEngine;
using UnityEngine.InputSystem;

public class PieceDrag : MonoBehaviour
{
    private Camera mainCamera;
    private bool draggingOn;
    private Vector3 movementOffset;
    
    private void Awake()
    {
        mainCamera = Camera.main; 
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
                movementOffset = transform.position - worldCoords;
            }
        }

        if (draggingOn && Mouse.current.leftButton.isPressed)
        {
            transform.position = worldCoords + movementOffset;
        }

        if (draggingOn && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            draggingOn = false;
        }

        if (draggingOn && Keyboard.current.rKey.wasPressedThisFrame)
        {
            transform.Rotate(0, 0, 45);
        }
    }
}
