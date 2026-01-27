using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public Transform player;
    public float distanceBetweenSpawns = 15f;
    public float spawnAhead = 50f;
    
    private float nextZ;
    private int obstaclesSpawned = 0;

    void Start()
    {
        // Trouver le joueur automatiquement
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
        
        // Position de départ
        nextZ = player.position.z + 20f;
        
        // Créer 10 obstacles au début
        for (int i = 0; i < 100; i++)
        {
            CreateObstacle();
        }
    }

    void Update()
    {
        // Quand le joueur s'approche, créer un nouvel obstacle
        if (player.position.z > nextZ - spawnAhead)
        {
            CreateObstacle();
        }
    }

    void CreateObstacle()
    {
        // Position aléatoire sur X (-2, 0, ou 2)
        float[] lanes = { -2f, 0f, 2f };
        float x = lanes[Random.Range(0, 3)];
        
        // Créer l'obstacle
        Vector3 pos = new Vector3(x, 0.5f, nextZ);
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Instantiate(prefab, pos, Quaternion.identity);
        
        // Prochaine position
        nextZ += distanceBetweenSpawns;
        obstaclesSpawned++;
        
        Debug.Log("Obstacle #" + obstaclesSpawned + " créé à Z=" + pos.z);
    }
}