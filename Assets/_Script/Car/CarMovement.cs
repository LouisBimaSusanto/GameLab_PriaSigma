using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 10f;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveForward()
    {
        rb.linearVelocity = transform.up * speed;
    }
}
