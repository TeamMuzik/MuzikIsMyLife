using System.Collections.Generic;
using UnityEngine;

public class ReplaceableFurniture : Furniture
{
    public string category;
    public List<Sprite> availableSprites; // 가구의 여러 스프라이트를 저장하는 리스트

    public void replaceFurniture(int spriteIndex)
    {
        // Sprite Renderer 컴포넌트 찾기
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && spriteIndex >= 0 && spriteIndex < availableSprites.Count)
        {
            // 선택한 인덱스에 해당하는 스프라이트 할당
            spriteRenderer.sprite = availableSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Sprite Renderer가 설정되지 않았거나, 인덱스가 잘못되었습니다.");
        }
    }
}