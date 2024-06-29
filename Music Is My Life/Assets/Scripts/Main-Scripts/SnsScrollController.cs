using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnsScrollController : MonoBehaviour
{
    public GameObject spritePanelPrefab; // 스프라이트를 표시할 패널 프리팹
    public Transform contentParent; // 패널을 배치할 부모 오브젝트

    void Start()
    {
        if (spritePanelPrefab == null || contentParent == null)
        {
            Debug.LogError("필수 설정이 누락되었습니다.");
            return;
        }

        List<string> spriteNames = SpriteUtils.GetAllSavedSprites();

        foreach (string spriteName in spriteNames)
        {
            Sprite sprite = SpriteUtils.LoadSprite(spriteName);
            if (sprite != null)
            {
                GameObject panel = Instantiate(spritePanelPrefab, contentParent);
                Image image = panel.GetComponent<Image>(); // Image 컴포넌트를 바로 가져옵니다
                if (image != null)
                {
                    image.sprite = sprite;
                    Debug.Log($"Loaded and set sprite: {spriteName}"); // 디버그 로그 추가
                }
                else
                {
                    Debug.LogError("패널 프리팹에 Image 컴포넌트가 없습니다.");
                }
            }
            else
            {
                Debug.LogError($"스프라이트 로드 실패: {spriteName}"); // 디버그 로그 추가
            }
        }
    }
}
