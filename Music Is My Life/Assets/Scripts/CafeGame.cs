using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeGame : MonoBehaviour
{
    public GameObject[] FruitsPrefabs;
    public List<GameObject> TotalOrder = new List<GameObject>();
    public static int turn;

    void Start()
    {
        SpawnFruits();
    }

    void SpawnFruits()
    {

        turn = 0;
        int index = 0;
        for (int i = 0; i < 12; i++)
        {
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            TotalOrder.Add(RandomFruit);
        }

        for (int i = 0; i < 6; i++)
        {
            float xPos = 1.5f * i - 5;
            Vector3 spawnPosition = new Vector3(xPos, 3, 0);
            GameObject FruitsObj = Instantiate(TotalOrder[i], spawnPosition, Quaternion.identity);
            CafeGameFruits objIndex = FruitsObj.GetComponent<CafeGameFruits>();
            objIndex.GetIndex(index++);
        }
    }
}