using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeGame : MonoBehaviour
{
    public GameObject[] FruitsPrefabs;
    public List<GameObject> TotalOrder = new List<GameObject>();
    public List<GameObject> TotalClickedFruits = new List<GameObject>();

    void Start()
    {
        SpawnFruits();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity);
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                TotalClickedFruits.Add(clickedObject);
            }
        }
    }
    public void SpawnFruits()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float xPos = 1.5f * i - 5;
            Vector3 spawnPosition = new Vector3(xPos, 3, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            TotalOrder.Add(FruitsObj);
        }
    }
    public void DestroyOrderAndClicked()
    {
        for (int i = 0; i < TotalOrder.Count; i++)
        {
            Destroy(TotalOrder[i]);
        }
        TotalOrder.Clear();
        TotalClickedFruits.Clear();
    }
}