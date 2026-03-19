using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 10f;

    Rigidbody2D rb;

    bool isKnockBack = false;
    float knockBackTimer = 0f;
    public float knockBackDuration = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveForward()
    {
        if (isKnockBack)
        {
            knockBackTimer -= Time.deltaTime;

            if (knockBackTimer <= 0f)
            {
                isKnockBack = false;
            }
            else
            {
                // pelan-pelan hilang efek knockback
                rb.linearVelocity *= 0.95f;
            }

            return;
        }

        rb.linearVelocity = transform.up * speed;
    }

    public void ApplyKnockback(Vector2 force)
    {
        isKnockBack = true;
        knockBackTimer = knockBackDuration;

        rb.linearVelocity = force;
    }
}