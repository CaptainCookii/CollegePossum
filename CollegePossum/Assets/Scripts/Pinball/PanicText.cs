using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

public class PanicText : MonoBehaviour
{

    [SerializeField] private RectTransform[] texts;
    [SerializeField] private float minMoveTime = 0.1f;
    [SerializeField] private float maxMoveTime = 0.1f;
    [SerializeField] private float speed = 800f;
    private Vector2[] positions;

    private void OnEnable()
    {
        positions = new Vector2[texts.Length];

        for (int i = 0; i < texts.Length; i++)
        {
            positions[i] = RandomPosition();
        }

        StartCoroutine(MoveTexts());
    }

    private IEnumerator MoveTexts()
    {
        while (true)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                positions[i] = RandomPosition();
            }

            yield return new WaitForSeconds(Random.Range(minMoveTime, maxMoveTime));
        }
    }

    private void Update()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].anchoredPosition = Vector2.MoveTowards(texts[i].anchoredPosition, positions[i], speed * Time.deltaTime);
        }
    }

    private Vector2 RandomPosition()
    {
        RectTransform rects = GetComponent<RectTransform>();
        float x = Random.Range(-rects.rect.width / 2, rects.rect.width / 2);
        float y = Random.Range(-rects.rect.height / 2, rects.rect.height / 2); 
        return new Vector2(x, y);
    }
    
}
