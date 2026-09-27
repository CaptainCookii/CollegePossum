using UnityEngine;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour
{
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject panelObject;
    private Color originalColor;

    private void Awake()
    {
        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (mainCamera.GetComponent<CameraPanMovemement>().GetLockStatus())
        {

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                spriteRenderer.color = highlightColor;
            }
            else
            {
                spriteRenderer.color = originalColor;
            }


            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    panelObject.SetActive(true);
                    mainCamera.GetComponent<CameraPanMovemement>().LockUnlockMovement();
                    mainCamera.GetComponent<CameraPanMovemement>().currentMenu = panelObject;
                    spriteRenderer.color = originalColor;
                }
            }
        }
    }
}
