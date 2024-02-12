using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreProductManager : MonoBehaviour
{
    public TMP_Text playerMoney; // 플레이어가 보유 중인 돈
    public TMP_Text playerStress;
    public GameObject[] audioProducts; // 모든 상품
    public GameObject[] goodsProducts; // 모든 상품

    void Start()
    {
        LoadProductsData(audioProducts);
        LoadGoodsProductsData(goodsProducts);
    }

    private void Update()
    {
        playerMoney.text = "잔고: " + PlayerPrefs.GetInt("Money") + "만원"; // 내 잔고 업데이트
        playerStress.text = "스트레스: " + PlayerPrefs.GetInt("Stress"); // 내 스트레스 업데이트 
    }

    public void LoadProductsData(GameObject[] allProductObj)
    {
        foreach (GameObject slotObj in allProductObj)
        {
            StoreProduct product = slotObj.GetComponentInChildren<StoreProduct>();
            // 가구 상태 세팅
            product.LoadFurnitureStatus();
            // 버튼 세팅
            Button button = slotObj.GetComponentInChildren<Button>();
            TextMeshProUGUI buttonText = slotObj.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();
            // Debug.Log("상품명: " + product.Category_Index + " | 가격: " + product.price);
            if (product.IsOwned) // 보유 중임
            {
                buttonText.text = "SOLD OUT";
                button.interactable = false;
            }
            else
            {
                buttonText.text = product.price + "만원";
            }
            buttonText.color = ColorUtility.TryParseHtmlString("#323232", out Color color) ? color : Color.black;
        }
    }

    public void LoadGoodsProductsData(GameObject[] allProductObj)
    {
        foreach (GameObject slotObj in allProductObj)
        {
            GoodsProduct product = slotObj.GetComponentInChildren<GoodsProduct>();
            // index, 개수 세팅
            product.index = PlayerPrefs.GetInt(product.category + "_CURRENT");
            product.description.text = "남은 개수: " + (product.totalCount - product.index) + "개";
            // 버튼 세팅
            Button button = slotObj.GetComponentInChildren<Button>();
            TextMeshProUGUI buttonText = slotObj.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();
            if (product.index >= product.totalCount) // 모두 판매 완료
            {
                buttonText.text = "SOLD OUT";
                button.interactable = false;
            }
            else
            {
                buttonText.text = product.price + "만원";
                button.interactable = true;
            }
            buttonText.color = ColorUtility.TryParseHtmlString("#323232", out Color color) ? color : Color.black;
        }
    }
}
