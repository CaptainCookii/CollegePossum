using UnityEngine;

public class AnxietyManager : MonoBehaviour
{

    [SerializeField] private float growthRate = 0.018f;
    private int activeBalls = 0;
    private int leverHits = 0;

    // Updates the height of the meter based on anxiety calculation.
    void Update()
    {
        if (activeBalls > 0)
        {
            float scale = growthRate * activeBalls * Time.deltaTime;
            transform.localScale += new Vector3(0f, scale, 0f); 
            transform.position += new Vector3(0f, scale / 2f, 0f);
        }
    }

    // Adding and removing balls from the active count to manage the
    // activeBalls variable.
    public void AddBall()
    {
         activeBalls++;
    }

    public void RemoveBall()
    {
        activeBalls--;
    }  
}
