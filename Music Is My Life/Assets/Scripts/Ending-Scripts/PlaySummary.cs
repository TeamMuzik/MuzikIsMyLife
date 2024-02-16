using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlaySummary : MonoBehaviour
{
    public FunitureController funitureController;
    public GameObject creditPanel;
    public TMP_Text endingName;
    public TMP_Text playerName;
    public TMP_Text behaviorText;
    public TMP_Text statusText;
    public TMP_Text roomText;

    private int cafeCount = 0;
    private int officeCount = 0;
    private int factoryCount = 0;
    private int beggingCount = 0;
    private int coverCount = 0;
    private int jjirasiCount = 0;

    void Start()
    {
        funitureController.UpdateFurnitures();
        DayBehavior();
        creditPanel.SetActive(false);
        EndingCollectionManager.UnlockAndSaveEnding(PlayerPrefs.GetString("Ending"));
        endingName.text = $"\"{PlayerPrefs.GetString("EndingName")}\"";

        playerName.text = "플레이어: " + PlayerPrefs.GetString("PlayerName"); // "의 2주 결과";
        behaviorText.text = "카페 알바: " + cafeCount + "일"
            + "\n사무실 알바: " + officeCount + "일"
            + "\n공장 알바: " + factoryCount + "일"
            + "\n구걸: " + beggingCount + "일"
            + "\n\n커버: " + coverCount + "일" + " (구독자: " + PlayerPrefs.GetInt("Subscribers") + "명)"
            + "\n찌라시: " + jjirasiCount + "일" + " (총 클릭 수: " + PlayerPrefs.GetInt("JjirasiClick") + "회)";
        statusText.text = "돈: " + PlayerPrefs.GetInt("Money") + "만원"
            + "\n나의 명성: " + PlayerPrefs.GetInt("MyFame")
            + "\n야옹의 명성: " + PlayerPrefs.GetInt("BandFame");
        roomText.text = PlayerPrefs.GetString("PlayerName") + "의 방";
    }

    private void DayBehavior()
    {
        int endDday = PlayerPrefs.GetInt("EndDday");
        for (int d = 1; d < endDday; d++)
        {
            int behavior = PlayerPrefs.GetInt("Day" + d + "_Behavior");
            Debug.Log("Day" + d + "_Behavior: " + behavior);
            switch (behavior)
            {
                case 0:
                    cafeCount++;
                    break;
                case 1:
                    officeCount++;
                    break;
                case 2:
                    factoryCount++;
                    break;
                case 3:
                    beggingCount++;
                    break;
                case 4:
                    coverCount++;
                    break;
                case 5:
                    jjirasiCount++;
                    break;
                default:
                    break;
            }
        }
    }
}
