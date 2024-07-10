using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactoryGame : MonoBehaviour
{
    private FactoryGameTimer FactoryGameTimerInstance;

    public GameObject[] keyBoardPrefabs; //키보드 이미지를 가진 오브젝트 리스트
    public KeyCode[] possibleKeys; //실제 입력할 키보드 리스트 (방향키, 스페이스바)
    public List<GameObject> spawnedKeyboards = new List<GameObject>();

    public GameObject doll;
    public GameObject[] DollPrefab;
    //public GameObject hand; // 손
    //public Sprite[] handSprite; // 손 스프라이트들
    public static int currentHandIndex;

    [SerializeField]
    private TextMeshProUGUI stageNumText;
    [SerializeField]
    private TextMeshProUGUI moneyNumText;

    public AudioClip successSound;
    public AudioClip mistakeSound;
    public AudioClip dollMakingSound;
    private AudioSource audioSource;

    public static int factoryTurn; //지금 어떤 오브젝트를 입력할 차례인지 (0에서 시작)
    private static int stageNum = 0;
    public static int RandNum = 0;
    public int money = 0;

    private Transform canvasPos;

    private bool isSpawning;

    private void Start()
    {
        stageNum = 0;
        money = 0;
        FactoryGameTimerInstance = FindObjectOfType<FactoryGameTimer>();
        audioSource = GetComponent<AudioSource>();
        // 손 스프라이트 인덱스
        currentHandIndex = 0;
        canvasPos = GameObject.Find("Canvas").transform;

        isSpawning = false;
            
    }

    public void StartSpawning()
    {   
        SpawnKeyBoards();
        if (FactoryGameTimer.totalTime > 0 && !isSpawning && !FactoryGameTimer.isEnd)
        {
            //float spawnInterval = FactoryGameTimer.totalTime >= 15f ? 2.0f : 1.0f;
            float spawnInterval = 1.0f;
            InvokeRepeating("SpawnKeyBoards", spawnInterval, spawnInterval);
        }
    }


    public void SpawnKeyBoards()
    {
        RandNum = Random.Range(3, 7);
    
        switch (RandNum)
        {
            case 3:
                //인형 생성
                GameObject doll0Prefab = DollPrefab[0];
                Vector3 bearSpawn = new Vector3 (0.16f, 7.05f, 0);
                doll = Instantiate(doll0Prefab, bearSpawn, Quaternion.identity, canvasPos);
            break;

            case 4:
                GameObject doll1Prefab = DollPrefab[1];
                Vector3 catSpawn = new Vector3 (0.5f, 8f, 0);
                doll = Instantiate(doll1Prefab, catSpawn, Quaternion.identity, canvasPos);
            break;

            case 5:
                GameObject doll2Prefab = DollPrefab[2];
                Vector3 foxSpawn = new Vector3 (0.35f, 7.8f, 0);
                doll = Instantiate(doll2Prefab, foxSpawn, Quaternion.identity, canvasPos);
            break;

            case 6:
                GameObject doll3Prefab = DollPrefab[3];
                Vector3 dogSpawn = new Vector3 (0, 7, 0);
                doll = Instantiate(doll3Prefab, dogSpawn, Quaternion.identity, canvasPos);
            break;
        }
    }

    public void increaseStageNum()
    {
        stageNum++;
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

    public void PlayMistakeSound()
    {
        audioSource.clip = mistakeSound;
        audioSource.Play();
    }

    public void PlayDollMakingSound()
    {
        audioSource.clip = dollMakingSound;
        audioSource.Play();
    }
    
    void Update()
    {
        //hand.GetComponent<SpriteRenderer>().sprite = handSprite[currentHandIndex]; // 손 계속 업데이트

        if (FactoryGameTimer.isEnd)
        {
            CancelInvoke("SpawnKeyBoards");
            isSpawning = false;
            return;
        }

        moneyNumText.SetText(money.ToString() + "만원");
        stageNumText.SetText(stageNum.ToString() + "개");
    }

    public void StartBlinkText()
    {
        StartCoroutine(FactoryGameTimerInstance.BlinkText(FactoryGameTimer.totalTime));
    }

    public static void ChangeHandIndexInTurn()
    {
        currentHandIndex = factoryTurn % 2;
    }
}