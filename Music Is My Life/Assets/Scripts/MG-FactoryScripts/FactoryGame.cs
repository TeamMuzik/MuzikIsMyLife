using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactoryGame : MonoBehaviour
{
    private FactoryGameTimer FactoryGameTimerInstance;
    public GameObject[] keyBoardPrefabs; //키보드 이미지를 가진 오브젝트 리스트
    public KeyCode[] possibleKeys; //실제 입력할 키보드 리스트 (방향키, 스페이스바)
    private GameObject doll;
    public GameObject[] DollPrefab;
    public Sprite[] BearSprites;
    public Sprite[] CatSprites;
    public Sprite[] FoxSprites;
    public Sprite[] DogSprites;
    public List<GameObject> spawnedKeyboards = new List<GameObject>();

    [SerializeField]
    private TextMeshProUGUI stageNumText;
    [SerializeField]
    private TextMeshProUGUI moneyNumText;

    public AudioClip successSound; // 성공 사운드 클립
    public AudioClip mistakeSound;
    private AudioSource audioSource;

    public static int turn; //지금 어떤 오브젝트를 입력할 차례인지 (0에서 시작)
    private static int stageNum = 0;
    public static int RandNum = 0;
    public int money = 0;

    private void Start()
    {
        stageNum = 0;
        money = 0;
        FactoryGameTimerInstance = FindObjectOfType<FactoryGameTimer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SpawnKeyBoards()
    {
        foreach (GameObject keyboard in spawnedKeyboards)
        {
            Destroy(keyboard);
        }
        spawnedKeyboards.Clear();
        turn = 0;
        int index = 0;
        float yPos = 1.5f;
        RandNum = Random.Range(3, 7);
    
        switch (RandNum)
        {
            case 3:
                //인형 생성
                GameObject doll0Prefab = DollPrefab[0];
                Vector3 bearSpawn = new Vector3 (-9f, -2, 0);
                doll = Instantiate(doll0Prefab, bearSpawn, Quaternion.identity);
                //키보드 생성
                for (int i = 0; i < RandNum; i++)
                {   
                    float xPos = -1.5f + 1.5f*i;
                    Vector3 spawnPosition = new Vector3 (xPos, yPos, 0);
                    int randNum = Random.Range(0, possibleKeys.Length);
                    GameObject selectedPrefab = keyBoardPrefabs[randNum];
                    GameObject keyObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                    spawnedKeyboards.Add(keyObject);
                    FactoryGameKeyboard keyScript = keyObject.GetComponent<FactoryGameKeyboard>(); //생성된 오브젝트의 키보드 스크립트 가져오기
                    keyScript.SetKeySprite(possibleKeys[randNum], index++);
                }
            break;

            case 4:
                GameObject doll1Prefab = DollPrefab[1];
                Vector3 catSpawn = new Vector3 (-9f, -1.4f, 0);
                doll = Instantiate(doll1Prefab, catSpawn, Quaternion.identity);
                for (int i = 0; i < RandNum; i++)
                {
                    float xPos = -2.25f + 1.5f*i;
                    Vector3 spawnPosition = new Vector3 (xPos, yPos, 0);
                    int randNum = Random.Range(0, possibleKeys.Length);
                    GameObject selectedPrefab = keyBoardPrefabs[randNum];
                    GameObject keyObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                    spawnedKeyboards.Add(keyObject);
                    FactoryGameKeyboard keyScript = keyObject.GetComponent<FactoryGameKeyboard>(); //생성된 오브젝트의 키보드 스크립트 가져오기
                    keyScript.SetKeySprite(possibleKeys[randNum], index++);
                }
            break;

            case 5:
                GameObject doll2Prefab = DollPrefab[2];
                Vector3 foxSpawn = new Vector3 (-9f, -2, 0);
                doll = Instantiate(doll2Prefab, foxSpawn, Quaternion.identity);
                for (int i = 0; i < RandNum; i++)
                {
                    float xPos = -3f + 1.5f*i;
                    Vector3 spawnPosition = new Vector3 (xPos, yPos, 0);
                    int randNum = Random.Range(0, possibleKeys.Length);
                    GameObject selectedPrefab = keyBoardPrefabs[randNum];
                    GameObject keyObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                    spawnedKeyboards.Add(keyObject);
                    FactoryGameKeyboard keyScript = keyObject.GetComponent<FactoryGameKeyboard>(); //생성된 오브젝트의 키보드 스크립트 가져오기
                    keyScript.SetKeySprite(possibleKeys[randNum], index++);
                }
            break;

            case 6:
                GameObject doll3Prefab = DollPrefab[3];
                Vector3 dogSpawn = new Vector3 (-9f, -2.17f, 0);
                doll = Instantiate(doll3Prefab, dogSpawn, Quaternion.identity);
                for (int i = 0; i < RandNum; i++)
                {
                    float xPos = -3.75f + 1.5f*i;
                    Vector3 spawnPosition = new Vector3 (xPos, yPos, 0);
                    int randNum = Random.Range(0, possibleKeys.Length);
                    GameObject selectedPrefab = keyBoardPrefabs[randNum];
                    GameObject keyObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                    spawnedKeyboards.Add(keyObject);
                    FactoryGameKeyboard keyScript = keyObject.GetComponent<FactoryGameKeyboard>(); //생성된 오브젝트의 키보드 스크립트 가져오기
                    keyScript.SetKeySprite(possibleKeys[randNum], index++);
                }
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

    void Update()
    {
        if (FactoryGameTimer.totalTime < 0)
        {
            foreach (GameObject keyboard in spawnedKeyboards)
            {
                Destroy(keyboard);
            }
            spawnedKeyboards.Clear();
        }
        
        if (doll != null) 
        {
            SpriteRenderer spriteRenderer = doll.GetComponent<SpriteRenderer>();
            switch (RandNum)
            {
                case 3:
                    spriteRenderer.sprite = BearSprites[turn];
                    break;
                case 4:
                    spriteRenderer.sprite = CatSprites[turn];
                    break;
                case 5:
                    spriteRenderer.sprite = FoxSprites[turn];
                    break;
                case 6:
                    spriteRenderer.sprite = DogSprites[turn];
                    break;
            }

            if (doll.transform.position.x > 9)
            {
                Destroy(doll);
                if (turn < RandNum)
                {
                    StartCoroutine(FactoryGameTimerInstance.BlinkText(FactoryGameTimer.totalTime));
                    FactoryGameTimer.totalTime -= 4f;
                    foreach (GameObject keyboard in spawnedKeyboards)
                    {
                        Destroy(keyboard);
                    }
                    spawnedKeyboards.Clear();
                    SpawnKeyBoards();
                    PlayMistakeSound();
                }
            }
        }

        if (RandNum != 0 && turn == RandNum)
        {
            moneyManager();
            increaseStageNum();
            SpawnKeyBoards();
        }

        moneyNumText.SetText(money.ToString() + "만원");
        stageNumText.SetText(stageNum.ToString() + "개");

        // if (stageNum == 6)
        // {
        //     stageNum++;
        //     foreach (GameObject keyboard in spawnedKeyboards)
        //     {
        //         Destroy(keyboard);
        //     }
        //     spawnedKeyboards.Clear();
        //     StartPanel.SetActive(false);
        //     EndPanel.SetActive(true);
        //     moneyNumText.SetText(money.ToString()+"만원");
        //     StatusChanger.EarnMoney(money);
        //     //GameObject.FindWithTag("StatusChanger").GetComponent<StatusChanger>().earnMoney(money);
        // }
    }

    // if (Timer.LimitTime < 0)
    // {
    //     StageNumPanel.SetActive(false);
    //     EndStagePanel.SetActive(true);
    //     stageNum = 1;
    //     Timer.LimitTime = 15f;
            
    //     foreach (GameObject keyboard in spawnedKeyboards)
    //     {
    //         Destroy(keyboard);
    //     }
    //     spawnedKeyboards.Clear();
    // }

    // if (turn == 9)
    // {
    //     turn = 0;
    //     increaseStageNum();
    //     SpawnKeyBoards();
    // }
}