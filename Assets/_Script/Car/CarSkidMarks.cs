using UnityEngine;

public class CarSkidMarks : MonoBehaviour
{
    [Header("Referensi")]
    [Tooltip("Drag TrailLeft dan TrailRight ke sini (bukan SkidLeft / SkidRight).")]
    [SerializeField] private TrailRenderer[] skidTrails;

    [Tooltip("Opsional: asap yang ikut menyala saat drift.")]
    [SerializeField] private ParticleSystem carSmoke;

    [Header("Pengaturan Drift")]
    [Tooltip("Kecepatan putar minimum (derajat/detik) agar dianggap sedang belok. " +
             "Kalau Turn Speed di CarSteering = 200, nilai 40-80 cocok.")]
    [SerializeField] private float turnRateThreshold = 60f;

    [Tooltip("Kecepatan samping minimum agar dianggap slide.")]
    [SerializeField] private float driftThreshold = 2f;

    [Tooltip("Kecepatan total minimum agar jejak muncul (mencegah jejak saat mobil diam).")]
    [SerializeField] private float minSpeed = 1f;

    [Tooltip("Jejak tetap menyala sebentar setelah berhenti agar tidak putus-putus.")]
    [SerializeField] private float stopDelay = 0.1f;

    private Rigidbody2D rb;
    private Vector2 lastPosition;
    private float lastAngle;
    private float driftTimer;
    private bool isEmitting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        lastPosition = transform.position;
        lastAngle = GetAngle();
        SetEmitting(false);
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // Kecepatan gerak
        Vector2 velocity;
        if (rb != null && rb.bodyType == RigidbodyType2D.Dynamic)
            velocity = rb.linearVelocity;
        else
            velocity = ((Vector2)transform.position - lastPosition) / dt;

        lastPosition = transform.position;

        // Kecepatan putar (derajat/detik) dari perubahan rotasi
        float angle = GetAngle();
        float turnRate = Mathf.Abs(Mathf.DeltaAngle(lastAngle, angle)) / dt;
        lastAngle = angle;

        // Kecepatan ke arah samping (depan mobil = transform.up)
        float lateralSpeed = Mathf.Abs(Vector2.Dot(velocity, transform.right));

        bool movingEnough = velocity.magnitude > minSpeed;
        bool turning = turnRate > turnRateThreshold;
        bool sliding = lateralSpeed > driftThreshold;

        bool drifting = movingEnough && (turning || sliding);

        if (drifting)
            driftTimer = stopDelay;
        else
            driftTimer -= dt;

        SetEmitting(driftTimer > 0f);
    }

    private float GetAngle()
    {
        return rb != null ? rb.rotation : transform.eulerAngles.z;
    }

    private void SetEmitting(bool value)
    {
        if (value == isEmitting) return;
        isEmitting = value;

        foreach (var trail in skidTrails)
        {
            if (trail != null)
                trail.emitting = value;
        }

        if (carSmoke != null)
        {
            var emission = carSmoke.emission;
            emission.enabled = value;
        }
    }
}