using System.Collections;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    [SerializeField] private Transform leftSpawn;
    [SerializeField] private Transform rightSpawn;

    [SerializeField] private GameObject leftBubble;
    [SerializeField] private GameObject rightBubble;

    [SerializeField] private int bubbles = 10;
    [SerializeField] private float spawnInterval = 7f;

    void Start()
    {
        StartCoroutine(SpawnBubbles());
        
    }

    private IEnumerator SpawnBubbles()
    {
        for (int i = 0; i < bubbles; i++)
        {
            bool left = Random.Range(0, 2) == 0;

            GameObject p;
            Transform s;

            if (left)
            {
                p = leftBubble;
                s = leftSpawn;
            }
            else
            {
                p = rightBubble;
                s = rightSpawn;
            }

            Instantiate(p, s.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}









































