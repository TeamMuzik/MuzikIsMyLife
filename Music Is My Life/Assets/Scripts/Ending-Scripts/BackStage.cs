using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackStage : MonoBehaviour
{
    public float typingSpeed; //타이핑 속도, 높을 수록 속도가 느림
    public float nextSceneSpeed; //다음 페이지로 넘어가는 속도, 높을 수록 속도가 느림

    public TMP_Text targetTxt;
    public TMP_Text targetTxt2;
    public TMP_Text playerName1;
    public TMP_Text playerName2;
    public TMP_Text playerName3;

    RectTransform rectTransform;
    TextMeshProUGUI textMeshPro;

    public string[] Dialogue;
    string[] dialogues;

    int dialogNum = 0;

    public GameObject[] illustrationObjects; // 일러스트 게임 오브젝트 배열
    private int currentIllustrationIndex = 0; // 현재 일러스트 인덱스

    public Button nextButton;
    public Button endButton; // 다음 일러스트로 넘어가는 버튼
    SceneMove sceneMover; // SceneMove 클래스의 인스턴스 생성

    public AudioClip Sound1;
    public AudioClip Sound2;
    public AudioClip Sound3;
    public AudioClip Sound4;

    private AudioSource audioSource;
    public AudioSource audioSource2;

    public GameObject[] Light;
    public GameObject audience;

    void Awake()
    {
        rectTransform = targetTxt.GetComponent<RectTransform>();
        textMeshPro = targetTxt.GetComponent<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        rectTransform.anchoredPosition = new Vector2 (114, -215);

        audioSource.clip = Sound1;
        audioSource.Play();

        StartTalk(Dialogue);

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

    void StartTalk (string[] talks)
    {
        dialogues = talks;
        StartCoroutine(Typing(dialogues[dialogNum]));
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
            targetTxt.text += talk[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(nextSceneSpeed);
        //마지막 일러스트가 아닐 경우 다음 다이얼로그 진행
        if (currentIllustrationIndex != illustrationObjects.Length - 1) NextTalk();
    }

    void NextTalk()
    {
        targetTxt.text = null;
        dialogNum++;

        if (dialogNum == dialogues.Length)
        {
            dialogNum = 0;
            return;
        }
        NextIllustration();
        TypingAndIllustration();
        StartCoroutine(Typing(dialogues[dialogNum]));
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
                // nextButton.gameObject.SetActive(true);
            }
        }
    }

    void TypingAndIllustration()
    {
        switch (currentIllustrationIndex)
        {
            case 1:
                textMeshPro.color = Color.black;
                rectTransform.anchoredPosition = new Vector2 (139, -246);
                break;
            case 2:
                textMeshPro.color = Color.white;
                rectTransform.anchoredPosition = new Vector2 (15, -254);
                playerName1.text = PlayerPrefs.GetString("PlayerName");
                break;
            case 3:
                textMeshPro.color = Color.black;
                break;
            case 4: //연락해주다니
                textMeshPro.color = Color.white;
                rectTransform.anchoredPosition = new Vector2 (-13, 268);
                StartCoroutine(talk());
                break;
            case 5: //sns 초대장면
                nextSceneSpeed = 3.5f;
                playerName2.text = PlayerPrefs.GetString("PlayerName");
                break;
            case 6: //공연장면
                StartCoroutine(ChangeLight());
                audioSource.Stop();
                audioSource.clip = Sound2;
                audioSource.Play();
                rectTransform.anchoredPosition = new Vector2 (-22, 255);
                break;
            case 7: //감격 (성덕이 되다)
                nextSceneSpeed = 3f;
                audioSource.Stop();
                audioSource.clip = Sound3;
                audioSource.Play();
                Dialogue[7] = "나 " + PlayerPrefs.GetString("PlayerName") + "\n성덕 중의 성덕이 되었다";
                break;
            case 8:
                audioSource2.clip = Sound4;
                audioSource2.Play();
                break;
            case 9:
                playerName3.text = PlayerPrefs.GetString("PlayerName");
                break;

        }
    }

    IEnumerator ChangeLight()
    {   
        //5번만 반복
        for (int i = 0; i < 5; i++)
        {
            int randNum = Random.Range(0, 5);
            yield return new WaitForSeconds(0.7f);
            Light[randNum].SetActive(true);
            audience.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            Light[randNum].SetActive(false);
            audience.SetActive(false);
        }
    }

    IEnumerator talk()
    {   
        nextSceneSpeed = 3f;
        yield return new WaitForSeconds(1.8f);
        targetTxt2.text = null;
        string talk = "야옹이 내 영상을 봤어?!!\n야옹에 합류하라니!!!";
        for (int i = 0; i < talk.Length; i++)
        {
            targetTxt2.text += talk[i];
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(0.5f);
        targetTxt2.text = null;
        nextSceneSpeed = 1.5f;
    }
}
