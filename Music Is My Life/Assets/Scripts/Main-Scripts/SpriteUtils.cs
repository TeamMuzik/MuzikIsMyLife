using System.Collections.Generic;
using UnityEngine;

public static class SpriteUtils
{
    // 스프라이트 저장
    public static void SaveSprite(int day, Sprite sprite)
    {
        string spriteName = sprite.name;
        PlayerPrefs.SetString($"DisplayedSprite_{day}", spriteName);
        PlayerPrefs.Save();
        Debug.Log($"Saved Sprite: Day {day}, Name: {spriteName}"); // 디버그 로그 추가
    }

    // 저장된 스프라이트 불러오기
    public static Sprite LoadSprite(string spriteName)
    {
        Debug.Log($"Attempting to load sprite: {spriteName}"); // 디버그 로그 추가
        return Resources.Load<Sprite>(spriteName);
    }

    // 저장된 모든 스프라이트 이름 불러오기
    public static List<string> GetAllSavedSprites()
    {
        List<string> spriteNames = new List<string>();
        int day = 1;
        while (PlayerPrefs.HasKey($"DisplayedSprite_{day}"))
        {
            string spriteName = PlayerPrefs.GetString($"DisplayedSprite_{day}");
            spriteNames.Add(spriteName);
            Debug.Log($"Loaded Sprite: Day {day}, Name: {spriteName}"); // 디버그 로그 추가
            day++;
        }
        return spriteNames;
    }

    // 저장된 모든 스프라이트 데이터 리셋
    public static void ResetAllSavedSprites()
    {
        int day = 1;
        while (PlayerPrefs.HasKey($"DisplayedSprite_{day}"))
        {
            PlayerPrefs.DeleteKey($"DisplayedSprite_{day}");
            day++;
        }
        PlayerPrefs.Save();
        Debug.Log("Reset all saved sprites."); // 디버그 로그 추가
    }
}
