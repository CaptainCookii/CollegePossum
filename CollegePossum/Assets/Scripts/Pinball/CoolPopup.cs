using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CoolPopup : MonoBehaviour
{
    [SerializeField] private float time = 2f;
    [SerializeField] private float moveSpeed = 30f;

    private TextMeshProUGUI popupText;

    private void Awake()
    {
        popupText = GetComponent<TextMeshProUGUI>();
    }

    public void ShowText(string s)
    {
        popupText.text = s;
        StartCoroutine(Animate());
    }

    // Cool animation for funsies
    private IEnumerator Animate()
    {
        float timer = 0f;

        while (timer < time)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    
}
