using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public GameObject[] illustrationObjects; // 일러스트 게임 오브젝트 배열
    private int currentIllustrationIndex = 0; // 현재 일러스트 인덱스

    public Button nextButton;
    public Button endButton; // 다음 일러스트로 넘어가는 버튼
     // 버튼의 텍스트 컴포넌트
    SceneMove sceneMover; // SceneMove 클래스의 인스턴스 생성

    void Start()
    {
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
                nextButton.gameObject.SetActive(true);
            }
        }
    }
}
