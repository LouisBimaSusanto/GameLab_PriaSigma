using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform spawnPoint;

    public void Respawn()
    {
        if (spawnPoint == null)
        {
            Debug.LogWarning("Spawn point belum di assign!");
            return;
        }

        transform.position = spawnPoint.position;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
    }
}