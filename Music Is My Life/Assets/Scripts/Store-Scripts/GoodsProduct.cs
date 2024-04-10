using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GoodsProduct : MonoBehaviour
{
    public string category;
    public int index; // 1~14, 1~6
    public int totalCount; // 총 개수
    public int price; // 가격
    public TMP_Text description;
    public Button button;

    private TextMeshProUGUI buttonText;

    public void LoadIndexStatus()
    {
        index = PlayerPrefs.GetInt($"{category}_CURRENT");
        description.text = "남은 개수: " + (totalCount - index) + "개";
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
            DecreaseStress(); // 스트레스 감소
            UpdateGoodsProductData(); // 굿즈 데이터 업데이트
            description.text = "남은 개수: " + (totalCount - index) + "개";
            if (index == totalCount)
            {
                SetButtonTextAndColor("구매 완료", "#00B028");
                button.interactable = false;
                StartCoroutine(MapSoldOutAfterDelay());
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
    private void UpdateGoodsProductData()
    {
        index++;
        PlayerPrefs.SetInt($"{category}_{index}_IsOwned", 1);
        PlayerPrefs.SetInt($"{category}_CURRENT", index);
    }

    IEnumerator MapPriceTextAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SetButtonTextAndColor(price.ToString() + "만원", "#323232");
    }

    IEnumerator MapSoldOutAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        SetButtonTextAndColor("SOLD OUT", "#323232");
    }

    private void SetButtonTextAndColor(string text, string colorCode)
    {
        if (buttonText != null)
        {
            buttonText.text = text; // 텍스트 변경
            buttonText.color = ColorUtility.TryParseHtmlString(colorCode, out Color color) ? color : Color.black; // 컬러 코드를 Color로 변환하여 적용
        }
    }

    private void DecreaseStress()
    {
        float p = Random.value;
        int stress;
        switch (price)
        {
            case 15:
                stress = -20;
                break;
            case 30:
                stress = -50;
                break;
            case 5:
                if (p < 0.7f)
                    stress = -5;
                else if (p < 0.9f)
                    stress = -10;
                else if (p < 0.97f)
                    stress = -20;
                else
                    stress = -50;
                break;
            default:
                throw new System.Exception("굿즈 상품 가격으로  15/30/5가 아닌 값이 입력되었습니다.");
        }
        StatusChanger.UpdateStress(stress); // 스트레스 업데이트
        Debug.Log("굿즈 구입 -> 스트레스 수치 업데이트 : " + stress);
    }
}
