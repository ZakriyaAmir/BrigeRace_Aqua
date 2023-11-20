using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class levelBehavior : MonoBehaviour
{
    public Transform[] spawnPositions;
    private PlayerScript[] allPlayers;
    public int totalEnemies;

    public void AssignUniqueSpawnPositions()
    {
        allPlayers = GameManager.instance.allPlayers;
   
        // Make sure there are enough spawn positions for all players
        if (allPlayers.Length > spawnPositions.Length)
        {
            Debug.LogError("Not enough spawn positions for all players!");
            return;
        }

        // Shuffle the spawn positions
        ShuffleArray(spawnPositions);

        // Assign positions to players
        for (int i = 0; i < allPlayers.Length; i++)
        {
            allPlayers[i].transform.position = spawnPositions[i].position;
        }
    }

    // Fisher-Yates shuffle algorithm
    void ShuffleArray(Transform[] array)
    {
        int n = array.Length;
        System.Random rng = new System.Random();

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Transform value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
}
