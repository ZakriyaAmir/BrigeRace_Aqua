using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnOntoShape : MonoBehaviour
{
    public GameObject[] objectsToSpawn; // Assign the array of prefabs you want to spawn
    public int spawnCountPerObject = 2; // Number of times each object should be spawned
    public LayerMask avoidanceLayer; // Set the layer to avoid overlapping
    public Transform bricksParent;

    private MeshCollider meshCollider;
    private MeshFilter meshFilter;

    void Start()
    {
        // Get the mesh collider or filter from the 3D model
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();

        if (meshCollider == null && meshFilter == null)
        {
            Debug.LogError("Mesh Collider or Mesh Filter not found!");
            return;
        }

        Bounds bounds = (meshCollider != null) ? meshCollider.bounds : meshFilter.mesh.bounds;

        // Iterate through each object in the array
        foreach (GameObject objectPrefab in objectsToSpawn)
        {
            // Spawn each object the specified number of times
            for (int i = 0; i < spawnCountPerObject; i++)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    0 - 0.3f,
                    Random.Range(bounds.min.z, bounds.max.z)
                );

                // Check if the random position is inside the model and not overlapping with other objects
                if (IsPointInsideModel(randomPosition, meshCollider) && !IsOverlapping(randomPosition))
                {
                    // Spawn the current object at the random position
                    GameObject brick = Instantiate(objectPrefab, randomPosition, Quaternion.identity);
                    brick.transform.parent = bricksParent;
                }
            }
        }
    }

    public void SpawnOnDemand(int playerColorIndex) 
    {
        Bounds bounds = (meshCollider != null) ? meshCollider.bounds : meshFilter.mesh.bounds;

        Vector3 randomPosition = new Vector3(
        Random.Range(bounds.min.x, bounds.max.x),
        0 - 0.3f,
        Random.Range(bounds.min.z, bounds.max.z)
                    );

        // Check if the random position is inside the model and not overlapping with other objects
        if (IsPointInsideModel(randomPosition, meshCollider) && !IsOverlapping(randomPosition))
        {
            // Spawn the current object at the random position
            GameObject brick = Instantiate(objectsToSpawn[playerColorIndex], randomPosition, Quaternion.identity);
            brick.transform.parent = bricksParent;
        }
    }

    bool IsPointInsideModel(Vector3 point, MeshCollider collider)
    {
        // Perform a raycast to check if the point is inside the model
        RaycastHit hit;
        Ray ray = new Ray(point + Vector3.up * 1000f, Vector3.down);

        return collider.Raycast(ray, out hit, Mathf.Infinity);
    }

    bool IsOverlapping(Vector3 position)
    {
        // Check if there are colliders in the specified layer at the given position
        Collider[] colliders = Physics.OverlapSphere(position, 0.1f, avoidanceLayer);

        return colliders.Length > 0;
    }
}
