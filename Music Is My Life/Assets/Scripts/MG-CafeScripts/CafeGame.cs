using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CafeGame : MonoBehaviour
{
    public GameObject[] FruitsPrefabs;
    public GameObject[] ReceiptPrefabs;

    public List<GameObject> Order1 = new List<GameObject>();
    public List<GameObject> Order2 = new List<GameObject>();
    public List<GameObject> Order3 = new List<GameObject>();
    public List<GameObject> Order4 = new List<GameObject>();
    public List<GameObject> Order5 = new List<GameObject>();

    public List<GameObject> TotalClickedFruits = new List<GameObject>();
    public List<GameObject> TotalFruitImageObject = new List<GameObject>();
    public VerticalLayoutGroup VerticalLayoutGroup;

    private GameObject ReceiptObj1;
    private GameObject ReceiptObj2;
    private GameObject ReceiptObj3;
    private GameObject ReceiptObj4;
    private GameObject ReceiptObj5;

    public int money = 0;
    private float yPosBox =2;
    
    [SerializeField]
    private TextMeshProUGUI moneyNumText;

    void Start()
    {
        money = 0;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //클릭한 과일 인식
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity);
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                TotalClickedFruits.Add(clickedObject);

                Sprite fruitSprite = clickedObject.GetComponent<SpriteRenderer>().sprite; //클릭한 과일의 스프라이트를 가져옴
                GameObject fruitImageObject = new GameObject("FruitImage", typeof(RectTransform)); //RectTransform 컴포넌트를 가진 오브젝트 생성
                fruitImageObject.AddComponent<Image>().sprite = fruitSprite; //생성된 오브젝트에 이미지 컴포넌트를 추가하고 클릭한 과일의 이미지를 넣음
                RectTransform RectTransform = fruitImageObject.GetComponent<RectTransform>();
                RectTransform.SetParent(VerticalLayoutGroup.transform, false); //버티컬 레이아웃의 자식요소로 들어가게 함
                TotalFruitImageObject.Add(fruitImageObject); //생성된 오브젝트를 리스트에 추가
            }
        }
        //벌은 돈
        moneyNumText.SetText(money.ToString()+"만원");
    }

    public void SpawnFruits_1()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float yPos = 2.8f - 0.6f*i;
            Vector3 spawnPosition = new Vector3(-5.6f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order1.Add(FruitsObj);
        }
        Vector3 spawnPositionBox = new Vector3(-5f, yPosBox, 0);
        GameObject ReceiptObj = ReceiptPrefabs[0];
        ReceiptObj1 = Instantiate(ReceiptObj, spawnPositionBox, Quaternion.identity);
    }

    public void SpawnFruits_2()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float yPos = 2.8f - 0.6f*i;
            Vector3 spawnPosition = new Vector3(-3.1f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order2.Add(FruitsObj);
        }
        Vector3 spawnPositionBox = new Vector3(-2.5f, yPosBox, 0);
        GameObject ReceiptObj = ReceiptPrefabs[0];
        ReceiptObj2 = Instantiate(ReceiptObj, spawnPositionBox, Quaternion.identity);
    }

    public void SpawnFruits_3()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float yPos = 2.8f - 0.6f*i;
            Vector3 spawnPosition = new Vector3(-0.6f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order3.Add(FruitsObj);
        } 
        Vector3 spawnPositionBox = new Vector3(0f, yPosBox, 0);
        GameObject ReceiptObj = ReceiptPrefabs[0];
        ReceiptObj3 = Instantiate(ReceiptObj, spawnPositionBox, Quaternion.identity);
    }

    public void SpawnFruits_4()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float yPos = 2.8f - 0.6f*i;
            Vector3 spawnPosition = new Vector3(1.9f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order4.Add(FruitsObj);
        }
        Vector3 spawnPositionBox = new Vector3(2.5f, yPosBox, 0);
        GameObject ReceiptObj = ReceiptPrefabs[0];
        ReceiptObj4 = Instantiate(ReceiptObj, spawnPositionBox, Quaternion.identity);
    }

    public void SpawnFruits_5()
    {
        int fruitsCount = Random.Range(3, 6);
        for (int i = 0; i < fruitsCount; i++)
        {
            float yPos = 2.8f - 0.6f*i;
            Vector3 spawnPosition = new Vector3(4.4f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order5.Add(FruitsObj);
        }
        Vector3 spawnPositionBox = new Vector3(5f, yPosBox, 0);
        GameObject ReceiptObj = ReceiptPrefabs[0];
        ReceiptObj5 = Instantiate(ReceiptObj, spawnPositionBox, Quaternion.identity);
    }

    public void ReceiptManager()
    {
        if (Order1.Count == 0)
            Destroy(ReceiptObj1);
        if (Order2.Count == 0)
            Destroy(ReceiptObj2);
        if (Order3.Count == 0)
            Destroy(ReceiptObj3);
        if (Order4.Count == 0)
            Destroy(ReceiptObj4);
        if (Order5.Count == 0)
            Destroy(ReceiptObj5);
    }

    public void DestroyOrder(List<GameObject> Order)
    {
        for (int i = 0; i < Order.Count; i++)
        {
            Destroy(Order[i]);
        }
        Order.Clear();   
    }

    public void moneyManager()
    {
        money += 1;
    }

    // public void ClearClickedObj()
    // {
    //     TotalClickedFruits.Clear();
    //     for (int i = 0; i < TotalFruitImageObject.Count; i++)
    //     {
    //         Destroy(TotalFruitImageObject[i]);
    //     }
    //     TotalFruitImageObject.Clear(); 
    // }

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