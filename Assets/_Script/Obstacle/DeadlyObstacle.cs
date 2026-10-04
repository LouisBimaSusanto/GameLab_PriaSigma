using UnityEngine;

public class DeadlyObstacle : BaseObstacle
{
    [Header("Audio Settings")]
    [Tooltip("Nama SFX yang diputar saat player terkena obstacle ini")]
    [SerializeField] private string fallSfxName = "fall";

    public override void Interact(GameObject player)
    {
        PlayerRespawn respawn = player.GetComponent<PlayerRespawn>();

        if (respawn != null)
        {
            if (AudioManager.Instance != null && !string.IsNullOrEmpty(fallSfxName))
            {
                AudioManager.Instance.PlaySFX(fallSfxName);
            }

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
