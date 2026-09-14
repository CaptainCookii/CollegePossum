using UnityEngine;

public class LeverController : MonoBehaviour
{
    public enum Side
    {
        Left,
        Right 
    }

    [SerializeField] private Side side;
    [SerializeField] private float force = 1000f;
    [SerializeField] private float stoppedForce = 500f;

    private HingeJoint2D hinge;
    private JointMotor2D motor;

    private void Awake()
    {
        hinge = GetComponent<HingeJoint2D>();
        motor = hinge.motor;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
