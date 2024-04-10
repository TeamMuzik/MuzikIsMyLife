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
        LoadAudioProductsData(audioProducts);
        LoadGoodsProductsData(goodsProducts);
    }

    private void Update()
    {
        playerMoney.text = "잔고: " + PlayerPrefs.GetInt("Money") + "만원"; // 내 잔고 업데이트
        playerStress.text = "스트레스: " + PlayerPrefs.GetInt("Stress"); // 내 스트레스 업데이트 
    }

    public void LoadAudioProductsData(GameObject[] allProductObj)
    {
        foreach (GameObject slotObj in allProductObj)
        {
            AudioProduct product = slotObj.GetComponentInChildren<AudioProduct>();
            // 가구 상태 세팅
            product.LoadOwnedStatus();
            // 버튼 세팅
            Button button = slotObj.GetComponentInChildren<Button>();
            TextMeshProUGUI buttonText = slotObj.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>();
            // Debug.Log("상품명: " + product.Category_Index + " | 가격: " + product.price);
            if (product.isOwned == false) // 보유 중x
            {
                buttonText.text = product.price + "만원";
            }
            else if (product.category.Equals("GUITAR")) // 일단은 기타만 장착 변경 가능
            {
                buttonText.text = "장착하기";
            }
            else
            {
                buttonText.text = "SOLD OUT";
                button.interactable = false;
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
            product.LoadIndexStatus();
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
