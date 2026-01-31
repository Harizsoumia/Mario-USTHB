using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    public GameObject roadPrefab;
    public Transform player;
    public float roadLength = 20f;
    public int numberOfRoads = 5;
    
    private List<GameObject> activeRoads = new List<GameObject>();
    private float nextSpawnZ = 0f;

    void Start()
    {
        // Trouver le joueur automatiquement
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        
        // Créer les premières routes
        for (int i = 0; i < numberOfRoads; i++)
        {
            SpawnRoad();
        }
    }

void Update()
{
    if (player == null) return;

    // Spawn quand le joueur arrive près de la dernière route posée
    if (player.position.z + (numberOfRoads * roadLength) > nextSpawnZ)
    {
        SpawnRoad();
        DestroyOldRoad();
    }
}


    void SpawnRoad()
    {
        if (roadPrefab == null) return;
        
        Vector3 spawnPosition = new Vector3(0, 0, nextSpawnZ);
        GameObject newRoad = Instantiate(roadPrefab, spawnPosition, Quaternion.identity);
        activeRoads.Add(newRoad);
        
        nextSpawnZ += roadLength;
    }

    void DestroyOldRoad()
    {
        if (activeRoads.Count > numberOfRoads)
        {
            Destroy(activeRoads[0]);
            activeRoads.RemoveAt(0);
        }
    }
}
