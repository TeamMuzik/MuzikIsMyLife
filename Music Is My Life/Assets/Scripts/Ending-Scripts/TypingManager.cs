using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypingManager : MonoBehaviour
{
    public float typingSpeed; //타이핑 속도, 높을 수록 속도가 느림
    public float nextSceneSpeed; //다음 페이지로 넘어가는 속도, 높을 수록 속도가 느림

    public TMP_Text targetTxt;

    public string[] tutorialDialogue;
    string[] dialogues;

    int talkNum = 0;

    public GameObject[] illustrationObjects; // 일러스트 게임 오브젝트 배열
    private int currentIllustrationIndex = 0; // 현재 일러스트 인덱스

    public Button nextButton;
    public Button endButton; // 다음 일러스트로 넘어가는 버튼
    SceneMove sceneMover; // SceneMove 클래스의 인스턴스 생성

    public GameObject[] dialogBox;

    void Start()
    {
        StartTalk(tutorialDialogue);
        if (illustrationObjects.Length == 1)
        {
            // 바로 End 버튼 활성화
            endButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);

        }
        else
        { endButton.gameObject.SetActive(false);

            // 모든 일러스트를 비활성화
            for (int i = 1; i < illustrationObjects.Length; i++)
            {
                illustrationObjects[i].SetActive(false);
            }

            RectTransform buttonRectTransform = nextButton.GetComponent<RectTransform>();
            buttonRectTransform.SetAsLastSibling();

            // 첫 번째 일러스트만 활성화
            illustrationObjects[0].SetActive(true);

            // 다음 버튼에 클릭 이벤트 추가
            nextButton.onClick.AddListener(NextIllustration);

            // TextMeshPro의 TMP_Text 컴포넌트 가져오기


            // SceneMove 스크립트를 찾아 인스턴스 생성
            sceneMover = FindObjectOfType<SceneMove>();
        }
    }

    public void StartTalk (string[] talks)
    {
        dialogues = talks;
        StartCoroutine(Typing(dialogues[talkNum]));
    }
    
    IEnumerator Typing(string talk)
    {
        targetTxt.text = null;

        if (talk.Contains("  "))
        {
            talk = talk.Replace("  ", "\n");
        }
        for (int i = 0; i < talk.Length; i++)
        {
            targetTxt.text +=  talk[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(nextSceneSpeed);
        NextTalk();
    }

    public void NextTalk()
    {
        targetTxt.text = null;
        talkNum++;

        if (talkNum == dialogues.Length)
        {
            talkNum = 0;
            return;
        }
        NextIllustration();
        StartCoroutine(Typing(dialogues[talkNum]));
    }

    // 다음 일러스트로 넘어가는 함수
    void NextIllustration()
    {
        // 현재 일러스트 숨기기
        illustrationObjects[currentIllustrationIndex].SetActive(false);

        // 다음 일러스트 인덱스 계산
        currentIllustrationIndex = (currentIllustrationIndex + 1) % illustrationObjects.Length;

        // 다음 일러스트 보이기
        illustrationObjects[currentIllustrationIndex].SetActive(true);

        if (currentIllustrationIndex >= 3)
        {
            
            for (int i = 0; i < 1; i++)
            {
                dialogBox[i].gameObject.SetActive(false);
            }
            RectTransform rectTransform = targetTxt.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(-274, 256);

        }

        // 일러스트 개수가 1개일 경우
        if (illustrationObjects.Length == 0)
        {
            // 바로 End 버튼 활성화
            endButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);

        }
        else
        {
            // 마지막 일러스트인 경우
            if (currentIllustrationIndex == illustrationObjects.Length - 1)
            {
                // 버튼 텍스트를 변경

                // Next 버튼 비활성화
                nextButton.gameObject.SetActive(false);

                // End 버튼 활성화
                endButton.gameObject.SetActive(true);
            }
            else
            {
                // 버튼 텍스트를 변경

                // Next 버튼 활성화
                nextButton.gameObject.SetActive(true);
            }
        }
    }




}

    // public static TypingManager instance;
 
    // [Header("Times for each character")]
    // public float timeForCharacter; //0.08이 기본.
 
    // [Header("Times for each character when speed up")]
    // public float timeForCharacter_Fast; //0.03이 빠른 텍스트.
 
    // float characterTime; // 실제 적용되는 문자열 속도.
 
    // //임시 저장되는 대화 오브젝트와 대화내용.
    // string[] dialogsSave;
    // TextMeshProUGUI tmpSave;
 
    // public static bool isDialogEnd;
 
    // bool isTypingEnd = false; //타이핑이 끝났는가?
    // int dialogNumber = 0; //대화 문단 숫자.
 
    // float timer; //내부적으로 돌아가는 시간 타이머
 
    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //     }
    //     timer = timeForCharacter;
    //     characterTime = timeForCharacter;
    // }
 
    // public void Typing(string[] dialogs, TextMeshProUGUI textObj)
    // {
    //     isDialogEnd = false;
    //     dialogsSave = dialogs;
    //     tmpSave = textObj;
    //     if (dialogNumber < dialogs.Length)
    //     {
    //         char[] chars = dialogs[dialogNumber].ToCharArray(); //받아온 다이얼 로그를 char로 변환.
    //         StartCoroutine(Typer(chars, textObj)); //레퍼런스로 넘겨보는거 테스트 해보자.
    //     }
    //     else
    //     {
    //         //문장이 끝났으므로 다른 문장을 받을 준비... 다이얼로그 초기화, 다이얼로그 세이브와 티엠피 세이브 초기화
    //         tmpSave.text = "";
    //         isDialogEnd = true; // 호출자는 다이알로그 엔드를 보고 다음 동작을 진행해주면 됨.
    //         dialogsSave = null;
    //         tmpSave = null;
    //         dialogNumber = 0;
    //     }
    // }
 
    // public void GetInputDown()
    // {
    //     //인풋이 들어왔을때 -> 텍스트가 진행중이면 빠르게 진행되고 텍스트가 마감되어있으면 다음 텍스트로 넘어감.
    //     //그리고 인풋이 캔슬되면 다시 문자열 속도를 정상화 시켜야함.
    //     if (dialogsSave != null)
    //     {
    //         if (isTypingEnd)
    //         {
    //             tmpSave.text = ""; //비어있는 문장 넘겨서 초기화. 
    //             Typing(dialogsSave, tmpSave);
    //         }
    //         else
    //         {
    //             characterTime = timeForCharacter_Fast; //빠른 문장 넘김.
    //         }
    //     }
    // }
 
    // public void GetInputUp()
    // {
    //     //인풋이 끝났을때.
    //     if (dialogsSave != null)
    //     {
    //         characterTime = timeForCharacter;
    //     }
    // }
 
    // IEnumerator Typer(char[] chars, TextMeshProUGUI textObj)
    // {
    //     int currentChar = 0;
    //     int charLength = chars.Length;
    //     isTypingEnd = false;
 
    //     while (currentChar < charLength)
    //     {
    //         if (timer >= 0)
    //         {
    //             yield return null;
    //             timer -= Time.deltaTime;
    //         }
    //         else
    //         {
    //             textObj.text += chars[currentChar].ToString();
    //             currentChar++;
    //             timer = characterTime; //타이머 초기화
    //         }
    //     }
    //     if (currentChar >= charLength)
    //     {
    //         isTypingEnd = true;
    //         dialogNumber++;
    //         NextIllustration();
    //         yield break;
    //     }
    // }




