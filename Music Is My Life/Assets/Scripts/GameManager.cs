using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        InitializeGamedata();
    }

    public void InitializeGamedata()
    {
        PlayerPrefs.DeleteAll(); // 초기화 시 기존 데이터 모두 삭제
        PlayerPrefs.SetInt("Money", 0); // 돈
        PlayerPrefs.SetInt("MyFame", 0); // 내 명성
        PlayerPrefs.SetInt("BandFame", 0); // 야옹의 명성 (추후 교체 필요)
        PlayerPrefs.SetInt("Stress", 0); // 스트레스
        PlayerPrefs.SetInt("Confidence", 0); // 뻔뻔지수
        PlayerPrefs.SetString("Date", "2023/12/31"); // 날짜 (하루 넘기고 시작할 예정)
        PlayerPrefs.SetInt("Dday", 15); // 디데이 (1 빼고 시작할 예정)
        
        // Cover Game
        PlayerPrefs.SetInt("Subscribers", 0);
    }
    public void QuitGame() // 게임 종료 버튼이 생긴다면 사용
    {
        Application.Quit();
    }

    public void ResetGame() // 리셋 버튼이 생긴다면 사용
    {
        InitializeGamedata();
    }
}
