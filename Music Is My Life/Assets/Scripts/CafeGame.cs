using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CafeGame : MonoBehaviour
{
    public GameObject[] FruitsPrefabs;
    public GameObject[] BoxPrefabs;
    public GameObject[] FruitsButton;
    // public List<List<GameObject>> TotalOrder = new List<List<GameObject>>();

    public List<GameObject> Order1 = new List<GameObject>();
    public List<GameObject> Order2 = new List<GameObject>();
    public List<GameObject> Order3 = new List<GameObject>();
    public List<GameObject> Order4 = new List<GameObject>();
    public List<GameObject> Order5 = new List<GameObject>();

    public List<GameObject> TotalClickedFruits = new List<GameObject>();

    private GameObject BoxObj1;
    private GameObject BoxObj2;
    private GameObject BoxObj3;
    private GameObject BoxObj4;
    private GameObject BoxObj5;

    public int money = 0;
    
    [SerializeField]
    private TextMeshProUGUI moneyNumText;

    void Start()
    {
        SpawnFruits_1();
        SpawnFruits_2();
        SpawnFruits_3();
        SpawnFruits_4();
        SpawnFruits_5();
        money = 0;
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

    public void SpawnFruits_1()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float yPos = 3.5f - 1*i;
            Vector3 spawnPosition = new Vector3(-5.5f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order1.Add(FruitsObj);
        }
        float yPosBox = 1.5f + 0.5f * (5 - fruitsCount); 
        Vector3 spawnPositionBox = new Vector3(-5.5f, yPosBox, 0);
        GameObject BoxObj = BoxPrefabs[fruitsCount - 3];
        BoxObj1 = Instantiate(BoxObj, spawnPositionBox, Quaternion.identity);
    }

    public void SpawnFruits_2()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float yPos = 3.5f - 1*i;
            Vector3 spawnPosition = new Vector3(-3.25f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order2.Add(FruitsObj);
        }
        float yPosBox = 1.5f + 0.5f * (5 - fruitsCount); 
        Vector3 spawnPositionBox = new Vector3(-3.25f, yPosBox, 0);
        GameObject BoxObj = BoxPrefabs[fruitsCount - 3];
        BoxObj2 = Instantiate(BoxObj, spawnPositionBox, Quaternion.identity);
    }

    public void SpawnFruits_3()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float yPos = 3.5f - 1*i;
            Vector3 spawnPosition = new Vector3(-1f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order3.Add(FruitsObj);
        }
        float yPosBox = 1.5f + 0.5f * (5 - fruitsCount); 
        Vector3 spawnPositionBox = new Vector3(-1f, yPosBox, 0);
        GameObject BoxObj = BoxPrefabs[fruitsCount - 3];
        BoxObj3 = Instantiate(BoxObj, spawnPositionBox, Quaternion.identity);
    }

    public void SpawnFruits_4()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float yPos = 3.5f - 1*i;
            Vector3 spawnPosition = new Vector3(1.25f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order4.Add(FruitsObj);
        }
        float yPosBox = 1.5f + 0.5f * (5 - fruitsCount); 
        Vector3 spawnPositionBox = new Vector3(1.25f, yPosBox, 0);
        GameObject BoxObj = BoxPrefabs[fruitsCount - 3];
        BoxObj4 = Instantiate(BoxObj, spawnPositionBox, Quaternion.identity);
    }

    public void SpawnFruits_5()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float yPos = 3.5f - 1*i;
            Vector3 spawnPosition = new Vector3(3.5f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order5.Add(FruitsObj);
        }
        float yPosBox = 1.5f + 0.5f * (5 - fruitsCount); 
        Vector3 spawnPositionBox = new Vector3(3.5f, yPosBox, 0);
        GameObject BoxObj = BoxPrefabs[fruitsCount - 3];
        BoxObj5 = Instantiate(BoxObj, spawnPositionBox, Quaternion.identity);
    }

    public void BoxManager()
    {
        if (Order1.Count == 0)
            Destroy(BoxObj1);
        if (Order2.Count == 0)
            Destroy(BoxObj2);
        if (Order3.Count == 0)
            Destroy(BoxObj3);
        if (Order4.Count == 0)
            Destroy(BoxObj4);
        if (Order5.Count == 0)
            Destroy(BoxObj5);
    }

    public void DestroyOrder(List<GameObject> OrderNum)
    {
        for (int i = 0; i < OrderNum.Count; i++)
        {
            Destroy(OrderNum[i]);
        }
        OrderNum.Clear();   
    }

    public void ClearClickedObj()
    {
        TotalClickedFruits.Clear();
    }

    public void moneyManager()
    {
        money += 1;
        moneyNumText.SetText(money.ToString()+"만원");
    }

    // 2차원 리스트로 과일 주문 5개 생성하기 - 근데 어려워서 포기 
    // public void SpawnFruits() 
    // {
    //     for (int i = 0; i < 5; i++)
    //     {
    //         int fruitCount = Random.Range(3, 6);
    //         float xPos = -5.5f + 2.25f * i;
    //         for (int j = 0; j < fruitCount; j++)
    //         {
    //             float yPos = 3.5f - 1 * j;
    //             Vector3 spawnPosition = new Vector3(xPos, yPos, 0);
    //             int randNum = Random.Range(0, FruitsPrefabs.Length);
    //             GameObject RandomFruit = FruitsPrefabs[randNum];
    //             GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
    //             TotalOrder.Add(new List<GameObject>{FruitsObj});
    //         }
            
    //         float yPosBox = 1.5f + 0.5f * (5 - fruitCount); 
    //         Vector3 spawnPositionBox = new Vector3(xPos, yPosBox, 0);
    //         GameObject BoxObj = BoxPrefabs[fruitCount - 3];
    //         Instantiate(BoxObj, spawnPositionBox, Quaternion.identity);
    //     }
    // }
}