using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Global Variables
    public int totalSocialPoints = 0;
    public int currSocialPoints = 0;
    public int totalCoolPoints = 0;
    public int currCoolPoints = 0;

    public int mathValue = 0;
    public int socialStudiesValue = 0;
    public int scienceValue = 0;
    public int englishValue = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
