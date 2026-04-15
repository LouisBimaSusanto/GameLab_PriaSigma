using UnityEngine;

public class WallObstacle : BaseObstacle
{
    public float bounceForce = 15f;
    public override void Interact(GameObject player)
    { }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CarMovement car = collision.gameObject.GetComponent<CarMovement>();

            if (car == null) return;

            Vector2 bounceDirection = -(Vector2)collision.transform.up;
            car.ApplyKnockback(bounceDirection * bounceForce);
        }
    }
}
