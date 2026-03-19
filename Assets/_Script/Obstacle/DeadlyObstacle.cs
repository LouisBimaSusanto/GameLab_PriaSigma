using UnityEngine;

public class DeadlyObstacle : BaseObstacle
{
    public override void Interact(GameObject player)
    {
        Destroy(player);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Interact(collision.gameObject);
        }
    }
}
