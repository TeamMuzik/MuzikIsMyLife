using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GoodsProduct : MonoBehaviour
{
    public string category;
    public int price; // 가격
    public int totalCount; // 총 개수
    public int index; // 1~14, 1~6
    public TMP_Text description;
    public Button button;

    private TextMeshProUGUI buttonText;

    public void Start()
    {
        index = PlayerPrefs.GetInt(category + "_CURRENT");
        description.text = "남은 개수: " + (totalCount - index) + "개";

        if (index >= totalCount)
        {
            buttonText.text = "SOLD OUT";
            button.interactable = false;
            return;
        }
    }

    public void BuyGoodsProduct()
    {
        if (index >= totalCount)
        {
            // throw new System.Exception("굿즈의 index가 totalCount 범위를 넘어갔습니다.");
            return;
        }
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

        if (StatusChanger.SpendMoney(price)) // 물건 구매 시도
        {
            UpdateGoodsProductData();
            description.text = "남은 개수: " + (totalCount - index) + "개";
            if (index == totalCount)
            {
                SetButtonTextAndColor("구매 완료", "#00B028");
                buttonText.text = "SOLD OUT";
                button.interactable = false;
            }
            else if (buttonText != null)
            {
                SetButtonTextAndColor("구매 완료", "#00B028"); // 텍스트를 변경하여 구매 완료로 설정
                StartCoroutine(MapPriceTextAfterDelay());
            }
        }
        else
        {
            SetButtonTextAndColor("잔고 부족", "#FF262C"); // 텍스트를 변경, 글자를 빨간색으로
            Debug.Log("구매할 수 없습니다.");
            StartCoroutine(MapPriceTextAfterDelay());
        }
    }

    // 구매에 대한 로직
    public void UpdateGoodsProductData()
    {
        index++;
        string Category_Index = category + "_" + index;
        PlayerPrefs.SetInt($"{Category_Index}_IsOwned", 1);
        PlayerPrefs.SetInt($"{Category_Index}_IsEquipped", 1);
        PlayerPrefs.SetInt(category + "_CURRENT", index);
    }

    public bool SaleStatus() // 판매 중인지
    {
        if (totalCount == index)
            return false;
        return true;
    }

    IEnumerator MapPriceTextAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SetButtonTextAndColor(price.ToString() + "만원", "#323232");
    }

    private void SetButtonTextAndColor(string text, string colorCode)
    {
        if (buttonText != null)
        {
            buttonText.text = text; // 텍스트 변경
            buttonText.color = ColorUtility.TryParseHtmlString(colorCode, out Color color) ? color : Color.black; // 컬러 코드를 Color로 변환하여 적용
        }
    }

}
