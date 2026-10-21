using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Instance variable
    public static GameManager Instance;

    // Global variables
    public float totalSocialPoints = 0;
    public float currSocialPoints = 0;
    public float totalCoolPoints = 0;
    public float currCoolPoints = 0;
    public float maxAnxiety = 250;
    public float currAnxiety = 0;

    private void Awake()
    {
        // Singleton protection
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

