using UnityEngine;

public class WallObstacle : BaseObstacle
{
    public float bounceForce = 15f;

    [Header("Audio Settings")]
    public string crashSFXName = "crash";

    public override void Interact(GameObject player)
    { }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CarMovement car = collision.gameObject.GetComponent<CarMovement>();

            if (car == null) return;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(crashSFXName);
            }

            Vector2 bounceDirection = -(Vector2)collision.transform.up;
            car.ApplyKnockback(bounceDirection * bounceForce);
        }
    }
}
