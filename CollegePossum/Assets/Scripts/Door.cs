using UnityEngine;
using UnityEngine.EventSystems;


[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class Door : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float requiredCoolPoints = 0f;
    [SerializeField] private GameObject nextRoomPrefab;
    [SerializeField] private Color lockedColor = Color.white;
    [SerializeField] private Color unlockedColor = new Color(0.8f, 0.8f, 0.8f);

    [SerializeField, Range(0f, 1f)] private float hoverBrightness = 0.6f;

    private SpriteRenderer spriteRenderer;
    private bool isHovered;

    void Awake() { spriteRenderer = GetComponent<SpriteRenderer>(); }

    void Update()
    {
        Color color = IsUnlocked() ? unlockedColor : lockedColor;
        if (isHovered)
        {
            color = Color.Lerp(color, Color.white, hoverBrightness);
        }
        spriteRenderer.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsUnlocked())
        {
            Debug.Log($"{name} is locked: needs {requiredCoolPoints} cool points");
            return;
        }

        if (nextRoomPrefab == null)
        {
            Debug.LogWarning($"{name} has no Next Room Prefab set.");
            return;
        }

        if (RoomManager.Instance == null)
        {
            Debug.LogError("No RoomManager in the scene.");
            return;
        }

        RoomManager.Instance.LoadRoom(nextRoomPrefab);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void OnDisable()
    {
        isHovered = false;
    }

    private bool IsUnlocked()
    {
        return GameManager.Instance != null
        && GameManager.Instance.currCoolPoints >= requiredCoolPoints;
    }


}
