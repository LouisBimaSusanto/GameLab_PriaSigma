using UnityEngine;

public class WallObstacle : BaseObstacle
{
    public float bounceForce = 8f;

    public override void Interact(GameObject player)
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CarMovement car = collision.gameObject.GetComponent<CarMovement>();

            Vector2 bounceDirection = collision.contacts[0].normal;

            car.ApplyKnockback(bounceDirection * bounceForce);
        }
    }
}
