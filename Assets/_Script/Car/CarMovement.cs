using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour
{
    public float speed = 10f;
    private float baseSpeed;
    Rigidbody2D rb;

    bool isKnockBack = false;
    float knockBackTimer = 0f;
    public float knockBackDuration = 0.4f;

    private Coroutine activeBoostCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        baseSpeed = speed;
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
            return;
        }

        Vector2 currentRightVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

        rb.linearVelocity = (Vector2)(transform.up * speed) + currentRightVelocity;
    }

    public void ApplyKnockback(Vector2 force)
    {
        isKnockBack = true;
        knockBackTimer = knockBackDuration;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ApplyBoost(float percentage, float duration)
    {
        if (activeBoostCoroutine != null)
        {
            StopCoroutine(activeBoostCoroutine);
        }

        activeBoostCoroutine = StartCoroutine(BoostRoutine(percentage, duration));
    }

    private IEnumerator BoostRoutine(float percentage, float duration)
    {
        speed = baseSpeed + (baseSpeed * (percentage / 100f));

        yield return new WaitForSeconds(duration);

        speed = baseSpeed;
        activeBoostCoroutine = null;
    }
}