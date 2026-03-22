using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    public GameObject[] trapPrefabs;
    public Transform[] spawnPoints;
    void Start()
    {
        SpawnTraps();
    }

    void SpawnTraps()
    {
        foreach (Transform point in spawnPoints)
        {
            int rand = Random.Range(0, trapPrefabs.Length);

            Instantiate(trapPrefabs[rand], point.position, Quaternion.identity);
        }
    }
}
