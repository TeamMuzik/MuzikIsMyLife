using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        initializeGamedata();
    }

    public void initializeGamedata()
    {
        PlayerPrefs.DeleteAll(); // 초기화 시 기존 데이터 모두 삭제
        PlayerPrefs.SetInt("Money", 0); // 돈
        PlayerPrefs.SetInt("Fame", 0); // 명성
        PlayerPrefs.SetInt("Stress", 0); // 스트레스
        PlayerPrefs.SetInt("Confidence", 0); // 뻔뻔지수
        PlayerPrefs.SetString("Date", "2024/01/01"); // 날짜
        PlayerPrefs.SetInt("Dday", 14); // 디데이
    }
    public void QuitGame() // 게임 종료 버튼이 생긴다면 사용
    {
        Application.Quit();
    }

    public void ResetGame() // 리셋 버튼이 생긴다면 사용
    {
        initializeGamedata();
    }
}
