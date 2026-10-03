using UnityEngine;

public class DriftController : MonoBehaviour
{
    [Header("Drift Settings")]
    [Range(0f, 1f)] public float lurusFriction = 0.1f;
    [Range(0f, 1f)] public float belokFriction = 0.95f;

    public bool isDrifting { get; private set; }
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void HandelDrift(bool isDriftButtonHeld)
    {
        isDrifting = isDriftButtonHeld;
    }

    public void ApplyDriftPhysics()
    {
        float driftFactor = isDrifting ? belokFriction : lurusFriction;

        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = forwardVelocity + rightVelocity * driftFactor;
    }
}