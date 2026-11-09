using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;

public class PanicText : MonoBehaviour
{

    [SerializeField] private RectTransform[] texts;
    [SerializeField] private float minMoveTime = 0.5f;
    [SerializeField] private float maxMoveTime = 2f;
    [SerializeField] private float speed = 10f;
    private Vector2[] positions;

    // Initialize starting positions for each element.
    private void OnEnable()
    {
        positions = new Vector2[texts.Length];

        for (int i = 0; i < texts.Length; i++)
        {
            positions[i] = RandomPosition();
        }

        StartCoroutine(MoveTexts());
    }

    // Moves the text to a random position based on the parameters.
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

    // Move towards position at the given speed.
    private void Update()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].anchoredPosition = Vector2.MoveTowards(texts[i].anchoredPosition, positions[i], speed * Time.deltaTime);
        }
    }

    // Creates the random positions.
    private Vector2 RandomPosition()
    {
        RectTransform rects = GetComponent<RectTransform>();
        float x = Random.Range(-rects.rect.width / 2, rects.rect.width / 2);
        float y = Random.Range(-rects.rect.height / 2, rects.rect.height / 2); 
        return new Vector2(x, y);
    }
    
}
