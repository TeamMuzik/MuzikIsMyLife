using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SnsScrollController : MonoBehaviour
{
    public GameObject spritePanelPrefab; // 스프라이트를 표시할 패널 프리팹
    public Transform contentParent; // 패널을 배치할 부모 오브젝트
    public List<Sprite> allSprites; // 모든 스프라이트 리스트
    public float maxSpriteWidth = 500f; // 스프라이트의 최대 너비
    public TMP_Text playerName;
    public GameObject dividerTextPrefab; // 구분선을 표시할 텍스트 프리팹 (TMP_Text 또는 Text)

    void Start()
    {
        string PlayerName = PlayerPrefs.GetString("PlayerName");
        if (PlayerName == "연보라")
        {
            playerName.text = "연보라\n@lavender_S2";
        }
        else
        {
            playerName.text = PlayerName;
        }

        if (spritePanelPrefab == null || contentParent == null || allSprites == null || dividerTextPrefab == null)
        {
            Debug.LogError("필수 설정이 누락되었습니다.");
            return;
        }

        // 프리팹을 처음에는 비활성화 상태로 설정
        spritePanelPrefab.SetActive(false);
        dividerTextPrefab.SetActive(false);

        List<string> spriteNames = SpriteUtils.GetAllSavedSprites();
        Debug.Log($"저장된 스프라이트 수: {spriteNames.Count}");

        // contentParent의 기존 자식 오브젝트 삭제 (프리팹 자체는 삭제하지 않도록)
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            GameObject child = contentParent.GetChild(i).gameObject;
            if (child != spritePanelPrefab && child != dividerTextPrefab)  // 프리팹은 삭제하지 않도록 체크
            {
                Destroy(child.gameObject);
            }
        }

        // Force update canvas to ensure correct layout calculations
        Canvas.ForceUpdateCanvases();

        RectTransform contentRectTransform = contentParent.GetComponent<RectTransform>();
        if (contentRectTransform == null)
        {
            Debug.LogError("contentParent에 RectTransform이 없습니다.");
            return;
        }

        for (int i = 0; i < spriteNames.Count; i++)
        {
            string spriteName = spriteNames[i];
            Debug.Log($"로드 중인 스프라이트 이름: {spriteName}");
            Sprite sprite = allSprites.Find(s => s.name == spriteName);
            if (sprite != null)
            {
                // 스프라이트 패널 생성
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

                    // 다음 스프라이트가 있을 경우에만 구분선 텍스트 추가
                    if (i < spriteNames.Count - 1) // 마지막 스프라이트가 아닌 경우
                    {
                        GameObject divider = Instantiate(dividerTextPrefab, contentParent);
                        TMP_Text dividerText = divider.GetComponent<TMP_Text>();
                        if (dividerText != null)
                        {
                            dividerText.text = "- - - - -"; // 구분선 텍스트 설정
                            divider.SetActive(true);
                        }
                    }
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
