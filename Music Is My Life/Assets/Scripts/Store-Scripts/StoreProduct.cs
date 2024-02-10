using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class StoreProduct : Furniture // Furniture을 상속받음
{
    public int price; // 가격
    public bool replaceable;
    public Button button;

    private TextMeshProUGUI buttonText;

    public void BuyProduct()
    {
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (StatusChanger.SpendMoney(price)) // 물건 구매 시도
        {
            if (replaceable)
            {
                ReplaceProduct();
            }
            IsOwned = true; // 구매 완료
            IsEquipped = true; // 장착 완료

            if (buttonText != null)
            {
                buttonText.text = "구매 완료"; // 텍스트를 변경하여 구매 완료로 설정
                button.interactable = false;
            }
        }
        else
        {
            SetButtonTextAndColor("잔고 부족", "#FF262C"); // 텍스트를 변경, 글자를 빨간색으로
            Debug.Log("구매할 수 없습니다.");
            StartCoroutine(MapPriceTextAfterDelay());
        }
    }
    IEnumerator MapPriceTextAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1초 대기

        // 1초 후에 가격으로 텍스트를 변경하고 검은색으로 설정
        SetButtonTextAndColor(price.ToString()+"만원", "#323232");
    }
    private void SetButtonTextAndColor(string text, string colorCode)
    {
        if (buttonText != null)
        {
            buttonText.text = text; // 텍스트 변경
            buttonText.color = ColorUtility.TryParseHtmlString(colorCode, out Color color) ? color : Color.black; // 컬러 코드를 Color로 변환하여 적용
        }
    }

    public void ReplaceProduct()
    {
        string[] CI = Category_Index.Split("_");
        string category = CI[0];
        int index = int.Parse(CI[1]);
        int previousIndex = PlayerPrefs.GetInt(category + "_CURRENT");
        PlayerPrefs.SetInt(category + "_" + previousIndex + "_IsEquipped", 0);
        PlayerPrefs.SetInt(category + "_CURRENT", index);
    }
}
