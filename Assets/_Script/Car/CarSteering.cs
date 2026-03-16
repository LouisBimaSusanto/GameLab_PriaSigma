using UnityEngine;

public class CarSteering : MonoBehaviour
{
    public float turnSpeed = 200f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Steer(float direction)
    {
        float rotationAmount = -direction * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);
    }
}
