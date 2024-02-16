using UnityEngine;
using System.Collections.Generic;

public class EndingsDisplayController : MonoBehaviour
{
    public List<GameObject> endingIcons; // Inspector에서 각 엔딩 아이콘 할당
    public List<string> endingNames; // Inspector에서 각 엔딩의 이름 할당

    void OnEnable()
    {
        UpdateEndingsDisplay();
    }

    void UpdateEndingsDisplay()
    {
        // 모든 잠금 해제된 엔딩 가져오기
        List<string> unlockedEndings = EndingCollectionManager.GetAllUnlockedEndings();

        for (int i = 0; i < endingIcons.Count; i++)
        {
            // 각 엔딩의 잠금 해제 여부 확인
            if (unlockedEndings.Contains(endingNames[i]))
            {
                endingIcons[i].SetActive(false); 
            }
            else
            {
                endingIcons[i].SetActive(true);
            }
        }
    }
}
