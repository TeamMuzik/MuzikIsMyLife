using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnsScrollController : MonoBehaviour
{
    public GameObject spritePanelPrefab; // 스프라이트를 표시할 패널 프리팹
    public Transform contentParent; // 패널을 배치할 부모 오브젝트
    public List<Sprite> allSprites; // 모든 스프라이트 리스트
    public float maxSpriteWidth = 500f; // 스프라이트의 최대 너비

    void Start()
    {
        if (spritePanelPrefab == null || contentParent == null || allSprites == null)
        {
            Debug.LogError("필수 설정이 누락되었습니다.");
            return;
        }

        // 프리팹을 처음에는 비활성화 상태로 설정
        spritePanelPrefab.SetActive(false);

        List<string> spriteNames = SpriteUtils.GetAllSavedSprites();
        Debug.Log($"저장된 스프라이트 수: {spriteNames.Count}");

        // Force update canvas to ensure correct layout calculations
        Canvas.ForceUpdateCanvases();

        RectTransform contentRectTransform = contentParent.GetComponent<RectTransform>();
        if (contentRectTransform == null)
        {
            Debug.LogError("contentParent에 RectTransform이 없습니다.");
            return;
        }

        foreach (string spriteName in spriteNames)
        {
            Debug.Log($"로드 중인 스프라이트 이름: {spriteName}");
            Sprite sprite = allSprites.Find(s => s.name == spriteName);
            if (sprite != null)
            {
                GameObject panel = Instantiate(spritePanelPrefab, contentParent);
                RectTransform panelRectTransform = panel.GetComponent<RectTransform>();
                Image image = panel.GetComponent<Image>();

                if (image != null && panelRectTransform != null)
                {
                    image.sprite = sprite;

                    // Get the prefab's width and use it to set the new width
                    float newWidth = spritePanelPrefab.GetComponent<RectTransform>().rect.width;
                    float aspectRatio = sprite.rect.width / sprite.rect.height;
                    float newHeight = newWidth / aspectRatio;

                    // Ensure the dimensions are positive
                    newWidth = Mathf.Abs(newWidth);
                    newHeight = Mathf.Abs(newHeight);

                    panelRectTransform.sizeDelta = new Vector2(newWidth, newHeight);

                    // 활성화 시킴
                    panel.SetActive(true);

                    Debug.Log($"Loaded and set sprite: {spriteName}, New size: {newWidth}x{newHeight}");
                }
                else
                {
                    Debug.LogError("패널 프리팹에 Image 또는 RectTransform 컴포넌트가 없습니다.");
                }
            }
            else
            {
                Debug.LogError($"스프라이트 로드 실패: {spriteName}");
            }
        }
    }
}
