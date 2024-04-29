using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

    public List<GameObject> Order1Name = new List<GameObject>();
    public List<GameObject> Order2Name = new List<GameObject>();
    public List<GameObject> Order3Name = new List<GameObject>();
    public List<GameObject> Order4Name = new List<GameObject>();
    public List<GameObject> Order5Name = new List<GameObject>();

    public VerticalLayoutGroup Order1NameVertical;
    public VerticalLayoutGroup Order2NameVertical;
    public VerticalLayoutGroup Order3NameVertical;
    public VerticalLayoutGroup Order4NameVertical;
    public VerticalLayoutGroup Order5NameVertical;

    public List<GameObject> TotalClickedFruits = new List<GameObject>();
    public List<GameObject> TotalFruitImage = new List<GameObject>();
    public List<GameObject> TotalFruitName = new List<GameObject>();

    public VerticalLayoutGroup ClickFruitsVertical;
    public VerticalLayoutGroup ClickNameVertical;

    private GameObject ReceiptObj1;
    private GameObject ReceiptObj2;
    private GameObject ReceiptObj3;
    private GameObject ReceiptObj4;
    private GameObject ReceiptObj5;

    [SerializeField] private GameObject BlenderBtn;
    [SerializeField] private Sprite[] BlenderSprite;

    public int money = 0;
    private float yPosBox =2;

    private int fruitsCount;

    public int clickCount = 0;
    
    public TMP_FontAsset customFont;

    [SerializeField]
    private TextMeshProUGUI moneyNumTextInStartPanel;

    public AudioClip successSound; // 성공 사운드 클립

    private AudioSource audioSource;

    private int fortuneId;


    void Start()
    {
        money = 0;
        clickCount = 0;
        audioSource = GetComponent<AudioSource>();

        fortuneId = DayFortune.GetTodayFortuneId();
        Debug.Log("운세번호: " + fortuneId);
    }
    
    void Update()
    {
        if (TotalClickedFruits.Count < 5)
        {
            if (Input.GetMouseButtonDown(0)) //클릭한 과일 인식
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity);
                if (hit.collider != null && hit.collider.gameObject.name != "GarbageBtn")
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    TotalClickedFruits.Add(clickedObject);
                    clickCount++;

                    //클릭한 과일 이미지 생성
                    Sprite fruitSprite = clickedObject.GetComponent<SpriteRenderer>().sprite; //클릭한 과일의 스프라이트를 가져옴
                    GameObject fruitImageObject = new GameObject("FruitImage", typeof(RectTransform)); //RectTransform 컴포넌트를 가진 오브젝트 생성
                    fruitImageObject.AddComponent<Image>().sprite = fruitSprite; //생성된 오브젝트에 이미지 컴포넌트를 추가하고 클릭한 과일의 이미지를 넣음
                    RectTransform RectTransform1 = fruitImageObject.GetComponent<RectTransform>();
                    RectTransform1.SetParent(ClickFruitsVertical.transform, false); //버티컬 레이아웃의 자식요소로 들어가게 함
                    TotalFruitImage.Add(fruitImageObject); //생성된 오브젝트를 리스트에 추가

                    // 클릭한 과일 이름 생성
                    string FruitName = clickedObject.name;
                    GameObject FruitNameObj = new GameObject("FruitNameText");
                    FruitNameObj.AddComponent<TextMeshProUGUI>().text = FruitName;
                    TextMeshProUGUI textMeshPro = FruitNameObj.GetComponent<TextMeshProUGUI>();
                    textMeshPro.font = customFont;
                    textMeshPro.color = Color.black;
                    textMeshPro.fontSize = 63;
                    RectTransform rectTransform2 = FruitNameObj.GetComponent<RectTransform>();
                    rectTransform2.SetParent(ClickNameVertical.transform, false); //버티컬 레이아웃의 자식요소로 들어가게 함
                    rectTransform2.sizeDelta = new Vector2(260f, rectTransform2.sizeDelta.y);
                    TotalFruitName.Add(FruitNameObj);
                }
        }

        //실시간 번 돈 나타내기
        moneyNumTextInStartPanel.SetText(money.ToString()+"만원");
        
        //과일을 클릭한 횟수에 따라 블랜더의 이미지 변경
        Image imageComponent = BlenderBtn.GetComponent<Image>();
        switch(clickCount)
        {
            case 0:
                imageComponent.sprite = BlenderSprite[0];
                imageComponent.SetNativeSize();
                break;
            case 1:
                imageComponent.sprite = BlenderSprite[1];
                imageComponent.SetNativeSize();
                break;
            case 2:
                imageComponent.sprite = BlenderSprite[2];
                imageComponent.SetNativeSize();
                break;
            case 3:
                imageComponent.sprite = BlenderSprite[3];
                imageComponent.SetNativeSize();
                break;
            case 4:
                imageComponent.sprite = BlenderSprite[4];
                imageComponent.SetNativeSize();
                break;
            case 5:
                imageComponent.sprite = BlenderSprite[5];
                imageComponent.SetNativeSize();
                break;
        }

        // if(TotalClickedFruits.Count == 0)
        // {
        //     imageComponent.sprite = BlenderSprite[0];
        //     imageComponent.SetNativeSize();
        // }

        //과일 이름 표시
        // string allClickedObjectNames = "";
        // foreach (var FruitsObject in TotalClickedFruits)
        // {
        //     allClickedObjectNames += FruitsObject.name + "\n";
        // }
        // clickedObjectNameText.SetText(allClickedObjectNames);
        }
    }

    public void SpawnFruits_1()
    {
        fruitsCount = Random.Range(3, 6); //한 주문에 나오는 과일의 개수 3~5개 사이
        for (int i = 0; i < fruitsCount; i++)
        {
            //과일 생성
            float yPos = 2.8f - 0.6f*i;
            Vector3 spawnPosition = new Vector3(-5.6f, yPos, 0);
            int randNum = Random.Range(0, FruitsPrefabs.Length);
            GameObject RandomFruit = FruitsPrefabs[randNum];
            GameObject FruitsObj = Instantiate(RandomFruit, spawnPosition, Quaternion.identity);
            Order1.Add(FruitsObj);
            //과일 이름 생성
            string fruitName = FruitsObj.name.Replace("(Clone)", ""); //생성된 과일 오브젝트의 이름에서 클론을 떼고 저장
            GameObject FruitNameObj = new GameObject("FruitNameText");
            FruitNameObj.AddComponent<TextMeshProUGUI>().text = fruitName;
            TextMeshProUGUI textMeshPro = FruitNameObj.GetComponent<TextMeshProUGUI>();
            textMeshPro.font = customFont;
            textMeshPro.color = Color.black;
            textMeshPro.fontSize = 15;
            //배치 버티컬 구조
            RectTransform rectTransform = FruitNameObj.GetComponent<RectTransform>();
            rectTransform.SetParent(Order1NameVertical.transform, false); //버티컬 레이아웃의 자식요소로 들어가게 함
            Order1Name.Add(FruitNameObj);
        }
        //주문 영수증 오브젝트 생성
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

            GameObject FruitNameObj = new GameObject("FruitNameText");
            string fruitName = FruitsObj.name.Replace("(Clone)", "");
            FruitNameObj.AddComponent<TextMeshProUGUI>().text = fruitName;
            TextMeshProUGUI textMeshPro = FruitNameObj.GetComponent<TextMeshProUGUI>();
            textMeshPro.font = customFont;
            textMeshPro.color = Color.black;
            textMeshPro.fontSize = 15;
            RectTransform rectTransform = FruitNameObj.GetComponent<RectTransform>();
            rectTransform.SetParent(Order2NameVertical.transform, false); //버티컬 레이아웃의 자식요소로 들어가게 함
            Order2Name.Add(FruitNameObj);
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

            GameObject FruitNameObj = new GameObject("FruitNameText");
            string fruitName = FruitsObj.name.Replace("(Clone)", "");
            FruitNameObj.AddComponent<TextMeshProUGUI>().text = fruitName;
            TextMeshProUGUI textMeshPro = FruitNameObj.GetComponent<TextMeshProUGUI>();
            textMeshPro.font = customFont;
            textMeshPro.color = Color.black;
            textMeshPro.fontSize = 15;
            RectTransform rectTransform = FruitNameObj.GetComponent<RectTransform>();
            rectTransform.SetParent(Order3NameVertical.transform, false); //버티컬 레이아웃의 자식요소로 들어가게 함
            Order3Name.Add(FruitNameObj);
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

            GameObject FruitNameObj = new GameObject("FruitNameText");
            string fruitName = FruitsObj.name.Replace("(Clone)", "");
            FruitNameObj.AddComponent<TextMeshProUGUI>().text = fruitName;
            TextMeshProUGUI textMeshPro = FruitNameObj.GetComponent<TextMeshProUGUI>();
            textMeshPro.font = customFont;
            textMeshPro.color = Color.black;
            textMeshPro.fontSize = 15;
            RectTransform rectTransform = FruitNameObj.GetComponent<RectTransform>();
            rectTransform.SetParent(Order4NameVertical.transform, false); //버티컬 레이아웃의 자식요소로 들어가게 함
            Order4Name.Add(FruitNameObj);
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

            GameObject FruitNameObj = new GameObject("FruitNameText");
            string fruitName = FruitsObj.name.Replace("(Clone)", "");
            FruitNameObj.AddComponent<TextMeshProUGUI>().text = fruitName;
            TextMeshProUGUI textMeshPro = FruitNameObj.GetComponent<TextMeshProUGUI>();
            textMeshPro.font = customFont;
            textMeshPro.color = Color.black;
            textMeshPro.fontSize = 15;
            RectTransform rectTransform = FruitNameObj.GetComponent<RectTransform>();
            rectTransform.SetParent(Order5NameVertical.transform, false); //버티컬 레이아웃의 자식요소로 들어가게 함
            Order5Name.Add(FruitNameObj);
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

    public void DestroyOrder(List<GameObject> Order, List<GameObject> OrderName)
    {
        for (int i = 0; i < Order.Count; i++)
        {
            Destroy(Order[i]);
        }
        Order.Clear();

        for (int i = 0; i < OrderName.Count; i++)
        {
            Destroy(OrderName[i]);
        }
        OrderName.Clear();
    }

    public void moneyManager()
    {
        money += 1;
    }

    public void PlaySuccessSound()
    {
        audioSource.clip = successSound;
        audioSource.Play();
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