using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeGame : MonoBehaviour
{
    public GameObject[] FruitsPrefabs;
    public List<GameObject> TotalOrder = new List<GameObject>();
    public GameObject[] SpawnedFruits;

    void Start()
    {
        SpawnFruits();
    }

    void Update()
    {

    }

    void SpawnFruits()
    {
        for (int i = 0; i < 12; i++)
        {
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            TotalOrder.Add(RandomFruit);
            Debug.Log(TotalOrder[i]);
        }

        for (int i = 0; i < 3; i++)
        {
            float xPos = 1.5f * i - 5;
            Vector3 spawnPosition = new Vector3(xPos, 3, 0);
            Instantiate(TotalOrder[i], spawnPosition, Quaternion.identity);
        }


        for (int i = 3; i < 6; i++)
        {
            float xPos = 2 + (i - 3) * 1.5f;
            Vector3 spawnPosition = new Vector3(xPos, 3, 0);
            Instantiate(TotalOrder[i], spawnPosition, Quaternion.identity);
        }
    }
}