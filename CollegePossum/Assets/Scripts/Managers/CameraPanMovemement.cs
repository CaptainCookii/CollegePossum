using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPanMovemement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float edgeSize = 20f;
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 20f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;
    [SerializeField] private Camera cam;
    private bool canMove = true;
    public GameObject currentMenu;

    private void Update()
    {
        if (canMove)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 movement = Vector3.zero;

            if (mousePosition.x <= edgeSize)
            {
                movement.x = -1f;
            }
            else if (mousePosition.x >= Screen.width - edgeSize)
            {
                movement.x = 1f;
            }

            if (mousePosition.y <= edgeSize)
            {
                movement.y = -1f;
            }
            else if (mousePosition.y >= Screen.height - edgeSize)
            {
                movement.y = 1f;
            }

            transform.position += movement * moveSpeed * Time.deltaTime;
            float cameraHeight = cam.orthographicSize;
            float cameraWidth = cameraHeight * cam.aspect;

            float clampedX = Mathf.Clamp(
                transform.position.x,
                minX + cameraWidth,
                maxX - cameraWidth
            );

            float clampedY = Mathf.Clamp(
                transform.position.y,
                minY + cameraHeight,
                maxY - cameraHeight
            );

            transform.position = new Vector3(
                clampedX,
                clampedY,
                transform.position.z
            );
        }
        else
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                currentMenu.SetActive(false);
                LockUnlockMovement();
            }
        }
    }

    public void LockUnlockMovement()
    {
        canMove = !canMove;
    }

    public bool GetLockStatus()
    {
        return canMove;
    }
}
