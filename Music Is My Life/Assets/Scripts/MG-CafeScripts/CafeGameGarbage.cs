using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CafeGameGarbage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CafeGame cafeGameInstance;
    [SerializeField] private GameObject GarbageBtn;
    [SerializeField] private Sprite[] GarbageSprite;

    void Start()
    {
        cafeGameInstance = FindObjectOfType<CafeGame>();
        Image imageComponent = GarbageBtn.GetComponent<Image>();
        imageComponent.sprite = GarbageSprite[0];
        imageComponent.SetNativeSize();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Image imageComponent = GarbageBtn.GetComponent<Image>();
        imageComponent.sprite = GarbageSprite[1];
        imageComponent.SetNativeSize();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Image imageComponent = GarbageBtn.GetComponent<Image>();
        imageComponent.sprite = GarbageSprite[0];
        imageComponent.SetNativeSize();
    }

    public void Garbage()
    {
        cafeGameInstance.TotalClickedFruits.Clear();

        for (int i = 0; i < cafeGameInstance.TotalFruitImage.Count; i++)
        {
            Destroy(cafeGameInstance.TotalFruitImage[i]);
        }
        cafeGameInstance.TotalFruitImage.Clear();

        for (int i = 0; i < cafeGameInstance.TotalFruitName.Count; i++)
        {
            Destroy(cafeGameInstance.TotalFruitName[i]);
        }
        cafeGameInstance.TotalFruitName.Clear();
    }
}