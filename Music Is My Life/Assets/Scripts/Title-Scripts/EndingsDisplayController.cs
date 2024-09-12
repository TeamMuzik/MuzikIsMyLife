/*  */using System.Collections.Generic;
using UnityEngine;

public class EndingsDisplayController : MonoBehaviour
{
    public List<GameObject> pageGroups; // 각 페이지에 해당하는 그룹 오브젝트 리스트
    public List<GameObject> lockIcons; // 각 엔딩 아이콘에 대응하는 잠금 아이콘 리스트
    public List<string> endingNames; // 각 엔딩 아이콘에 대응하는 이름 리스트
    public List<GameObject> endingTexts; // 각 엔딩 아이콘에 대응하는 텍스트 리스트

    private int currentPage = 0; // 현재 페이지 인덱스
    private int totalPages = 0; // 총 페이지 수

    public GameObject nextButton; // 다음 페이지 버튼
    public GameObject prevButton; // 이전 페이지 버튼

    void OnEnable()
    {
        // 총 페이지 수 계산
        totalPages = pageGroups.Count;
        UpdateEndingsDisplay(); // 첫 페이지 아이콘 설정
        UpdatePageButtons(); // 버튼 상태 업데이트
    }

    void UpdateEndingsDisplay()
    {
        // 모든 잠금 해제된 엔딩 가져오기
        List<string> unlockedEndings = EndingCollectionManager.GetAllUnlockedEndings();

        // 모든 페이지 그룹 비활성화
        foreach (var group in pageGroups)
        {
            group.SetActive(false);
        }

        // 현재 페이지의 그룹만 활성화
        if (currentPage >= 0 && currentPage < totalPages)
        {
            pageGroups[currentPage].SetActive(true);

            // 현재 페이지의 엔딩 아이콘들에 대해 잠금 상태를 업데이트
            int startIconIndex = GetStartIconIndexForPage(currentPage);
            int iconsInCurrentPage = pageGroups[currentPage].transform.childCount;

            for (int i = startIconIndex; i < startIconIndex + iconsInCurrentPage && i < lockIcons.Count; i++)
            {
                if (unlockedEndings.Contains(endingNames[i]))
                {
                    // 엔딩이 잠금 해제되었으면 잠금 아이콘 비활성화 및 텍스트 활성화
                    lockIcons[i].SetActive(false);
                    if (endingTexts.Count > i)
                    {
                        endingTexts[i].SetActive(true);
                    }
                }
                else
                {
                    // 엔딩이 잠금 해제되지 않았으면 잠금 아이콘 활성화 및 텍스트 비활성화
                    lockIcons[i].SetActive(true);
                    if (endingTexts.Count > i)
                    {
                        endingTexts[i].SetActive(false);
                    }
                }
            }
        }
    }

    int GetStartIconIndexForPage(int page)
    {
        int startIndex = 0;
        for (int i = 0; i < page; i++)
        {
            startIndex += pageGroups[i].transform.childCount;
        }
        return startIndex;
    }

    public void NextPage()
    {
        if (currentPage < totalPages - 1)
        {
            currentPage++;
            UpdateEndingsDisplay();
            UpdatePageButtons();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateEndingsDisplay();
            UpdatePageButtons();
        }
    }

    void UpdatePageButtons()
    {
        // 첫 페이지에서는 이전 버튼 비활성화
        prevButton.SetActive(currentPage > 0);

        // 마지막 페이지에서는 다음 버튼 비활성화
        nextButton.SetActive(currentPage < totalPages - 1);
    }
}
