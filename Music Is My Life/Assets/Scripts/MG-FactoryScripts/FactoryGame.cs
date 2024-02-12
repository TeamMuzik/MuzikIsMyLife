using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FactoryGame : MonoBehaviour
{
    public GameObject[] keyBoardPrefabs; //키보드 이미지를 가진 오브젝트 리스트
    public KeyCode[] possibleKeys; //실제 입력할 키보드 리스트 (방향키, 스페이스바)

    //public Transform keySpawnArea; // 키보드가 스폰할 지역 (BackGround 오브젝트)
    public float xOffset = 1.5f; // X축 간격 조절 값

    public static int turn; //지금 어떤 오브젝트를 입력할 차례인지 (0에서 시작)
    private static int stageNum = 1;
    public List<GameObject> spawnedKeyboards = new List<GameObject>();

    [SerializeField]
    private TextMeshProUGUI stageNumText;
    [SerializeField]
    private TextMeshProUGUI moneyNumText;

    [SerializeField]
    private int RandNum = 0;
    public int money = 0;

    private void Start()
    {
        stageNum = 1;
        money = 0;
    }

    public void SpawnKeyBoards()
    {
        foreach (GameObject keyboard in spawnedKeyboards)
        {
            Destroy(keyboard);
        }
        spawnedKeyboards.Clear();
        turn = 0;
        int index = 0;   //현재 생성된 키보드 오브젝트가 몇번째로 생성됐는지 나타내는 index변수
        RandNum = Random.Range(3, 7);
        switch (RandNum)
        {
            case 3:
                for (int i = 0; i < RandNum; i++)
                {
                    float xPos = -1.5f + 1.5f*i;
                    Vector3 spawnPosition = new Vector3 (xPos, -2.5f, 0);
                    int randNum = Random.Range(0, possibleKeys.Length);
                    GameObject selectedPrefab = keyBoardPrefabs[randNum];
                    GameObject keyObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                    spawnedKeyboards.Add(keyObject);
                    FactoryGameKeyboard keyScript = keyObject.GetComponent<FactoryGameKeyboard>(); //생성된 오브젝트의 키보드 스크립트 가져오기
                    keyScript.SetKeySprite(possibleKeys[randNum], index++);
                }
            break;

            case 4:
                for (int i = 0; i < RandNum; i++)
                {
                    float xPos = -2.25f + 1.5f*i;
                    Vector3 spawnPosition = new Vector3 (xPos, -2.5f, 0);
                    int randNum = Random.Range(0, possibleKeys.Length);
                    GameObject selectedPrefab = keyBoardPrefabs[randNum];
                    GameObject keyObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                    spawnedKeyboards.Add(keyObject);
                    FactoryGameKeyboard keyScript = keyObject.GetComponent<FactoryGameKeyboard>(); //생성된 오브젝트의 키보드 스크립트 가져오기
                    keyScript.SetKeySprite(possibleKeys[randNum], index++);
                }
            break;

            case 5:
                for (int i = 0; i < RandNum; i++)
                {
                    float xPos = -3f + 1.5f*i;
                    Vector3 spawnPosition = new Vector3 (xPos, -2.5f, 0);
                    int randNum = Random.Range(0, possibleKeys.Length);
                    GameObject selectedPrefab = keyBoardPrefabs[randNum];
                    GameObject keyObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                    spawnedKeyboards.Add(keyObject);
                    FactoryGameKeyboard keyScript = keyObject.GetComponent<FactoryGameKeyboard>(); //생성된 오브젝트의 키보드 스크립트 가져오기
                    keyScript.SetKeySprite(possibleKeys[randNum], index++);
                }
            break;

            case 6:
                for (int i = 0; i < RandNum; i++)
                {
                    float xPos = -3.75f + 1.5f*i;
                    Vector3 spawnPosition = new Vector3 (xPos, -2.5f, 0);
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
        stageNumText.SetText(stageNum.ToString());
    }

    public void moneyManager()
    {
        money += 1;
        moneyNumText.SetText(money.ToString() + " 만원");
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
        
        if (RandNum != 0)
        {
            if (turn == RandNum)
            {
                turn = 0;
                // FactoryGameTimer.stageTime = 0;
                moneyManager();
                increaseStageNum();
                SpawnKeyBoards();
            }
        }


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