using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform player;

    public float startAfterZ = 150f;      // l'Enemy commence après cette distance
    public float spawnDistance = 60f;     // il spawn devant Mario
    public float minInterval = 10f;
    public float maxInterval = 20f;

    public float laneOffset = 2f;         // lanes: -2 / 0 / +2 (à adapter)

    float nextTime;

    void Start()
    {
        nextTime = Time.time + Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        if (player == null || enemyPrefab == null) return;

        // Pas au début
        if (player.position.z < startAfterZ) return;

        if (Time.time >= nextTime)
        {
            Spawn();
            nextTime = Time.time + Random.Range(minInterval, maxInterval);
        }
    }

    void Spawn()
    {
        float[] lanes = { -laneOffset, 0f, laneOffset };
        float x = lanes[Random.Range(0, lanes.Length)];

        Vector3 pos = new Vector3(x, 1f, player.position.z + spawnDistance);
        Instantiate(enemyPrefab, pos, Quaternion.identity);
    }
}
