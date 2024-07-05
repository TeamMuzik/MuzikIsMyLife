using UnityEngine;
using UnityEngine.UI;

public class EventSpriteChanger : MonoBehaviour
{
    public static void SetImage(GameObject obj, Sprite newSprite)
    {
        obj.GetComponent<Image>().sprite = newSprite;
    }

    public static void SetSprite(GameObject obj, Sprite sprite)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sprite = sprite;
        else
            Debug.LogError($"{obj.name}의 Sprite Renderer가 설정되지 않았습니다.");
    }
}