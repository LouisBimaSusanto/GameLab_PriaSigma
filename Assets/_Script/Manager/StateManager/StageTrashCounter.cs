using UnityEngine;

public class StageTrashCounter : MonoBehaviour
{
    [SerializeField]
    private bool registerOnStart = true;

    private void Start()
    {
        if (registerOnStart)
        {
            RegisterTrashInStage();
        }
    }

    public void RegisterTrashInStage()
    {
        if (StageManager.Instance == null)
        {
            Debug.LogError(
                "[StageTrashCounter] " +
                "StageManager.Instance NULL!"
            );

            return;
        }

        CollectibleItem[] trash =
            FindObjectsByType<CollectibleItem>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

        Debug.Log(
            $"[StageTrashCounter] " +
            $"Menemukan {trash.Length} sampah di scene."
        );

        StageManager.Instance.ResetTrashCounter();

        if (trash.Length == 0)
        {
            Debug.LogWarning(
                "[StageTrashCounter] " +
                "TIDAK ADA CollectibleItem ditemukan!"
            );

            return;
        }

        StageManager.Instance.RegisterTotalTrash(
            trash.Length
        );

        Debug.Log(
            $"[StageTrashCounter] " +
            $"Total trash terdaftar: {trash.Length}"
        );
    }
}