using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryGameKeyboard : MonoBehaviour
{
    private FactoryGame FactoryGameInstance;
    private FactoryGameTimer FactoryGameTimerInstance;
    public KeyCode assignedKey; //객체의 key

    public Sprite LeftKeyboard; //좌
    public Sprite RightKeyboard; //우
    public Sprite DownKeyboard; //하
    public Sprite UpKeyboard; //상
    public Sprite SpaceKeyboard; //스페이스바
    public int objectIndex;
    public static int keyState = 0; //키보드가 눌렸는지 체크하는 변수
    public static bool allowControl = true;

    private SpriteRenderer spriteRenderer;
    private Color redColor = Color.red;
    private Color whiteColor = Color.white;
    public Color blackColor = Color.black;


    public void Start()
    {
        FactoryGameInstance = FindObjectOfType<FactoryGame>();
        FactoryGameTimerInstance = FindObjectOfType<FactoryGameTimer>();
        allowControl = true;
    }

    public void SetKeySprite(KeyCode key, int index)
    {
        assignedKey = key; //이 스프라이트의 키를 저장
        objectIndex = index;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch (assignedKey)
        {
            case KeyCode.LeftArrow:
                spriteRenderer.sprite = LeftKeyboard;
                break;
            case KeyCode.RightArrow:
                spriteRenderer.sprite = RightKeyboard;
                break;
            case KeyCode.DownArrow:
                spriteRenderer.sprite = DownKeyboard;
                break;
            case KeyCode.UpArrow:
                spriteRenderer.sprite = UpKeyboard;
                break;
            case KeyCode.Space:
                spriteRenderer.sprite = SpaceKeyboard;
                break;
        }
    }

    void Update()
    {
        if (allowControl)
        {
            if (objectIndex == FactoryGame.factoryTurn)
            {
                if (Input.GetKeyDown(assignedKey))
                {
                    keyState = 1;
                }
                if (keyState == 1 && !Input.anyKeyDown) //한번 입력되고 다른 키가 입력되지 않을 때
                {
                    FactoryGame.ChangeHandIndexInTurn(); // 손 인덱스 바꿈 (0/1)
                    keyState = 0;
                    FactoryGame.factoryTurn++;
                    FactoryGameInstance.PlayDollMakingSound(); // 인형 꿰메지는 사운드 재생
                    // StartCoroutine(SuccessChangeColor(gameObject));
                    Destroy(gameObject);
                    if (FactoryGameTimerInstance.MistakePanel.activeSelf)
                    {
                        FactoryGameTimerInstance.MistakePanel.SetActive(false);
                    }
                }
                else if (Input.anyKeyDown && !Input.GetKeyDown(assignedKey) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) //실수로 다른 키를 눌렀을 때
                {
                    FactoryGame.currentHandIndex = 2; // 손 인덱스 2로 바꿈
                    keyState = 0;
                    allowControl = false;
                    StartCoroutine(MistakeChangeColor());
                    StartCoroutine(FactoryGameTimerInstance.BlinkText(FactoryGameTimer.totalTime));
                    FactoryGameTimer.totalTime -= 2f;
                    FactoryGameInstance.PlayMistakeSound();

                    // foreach (GameObject keyboard in FactoryGameInstance.spawnedKeyboards)
                    // { Destroy(keyboard); }
                    // FactoryGameInstance.spawnedKeyboards.Clear();
                }
            }
        }
        
    }

    public IEnumerator MistakeChangeColor()
    {
        float elapsedTime = 0f; // 누적 경과 시간
        float fadedTime = 0.5f; // 총 소요 시간
        spriteRenderer = GetComponent<SpriteRenderer>();
        while (elapsedTime <= fadedTime)
        {
            // 이미지 색상 변경
            spriteRenderer.color = Color.Lerp(redColor, whiteColor, elapsedTime / fadedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // public IEnumerator SuccessChangeColor(GameObject keyObj)
    // {
    //     float elapsedTime = 0f; // 누적 경과 시간
    //     float fadedTime = 0.5f; // 총 소요 시간
    //     spriteRenderer = GetComponent<SpriteRenderer>();
    //     while (elapsedTime <= fadedTime)
    //     {
    //         // 이미지 색상 변경
    //         spriteRenderer.color = Color.Lerp(blackColor, whiteColor, elapsedTime / fadedTime);
    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    //     Destroy(gameObject);
    // }
}
