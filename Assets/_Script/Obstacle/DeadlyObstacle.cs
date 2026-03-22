using UnityEngine;

public class DeadlyObstacle : BaseObstacle
{
    public override void Interact(GameObject player)
    {
        PlayerRespawn respawn = player.GetComponent<PlayerRespawn>();

        if (respawn != null)
        {
            respawn.Respawn();
        }
        else
        {
            Debug.LogError("PlayerRespawn tidak ditemukan!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Interact(collision.gameObject);
        }
    }
}
