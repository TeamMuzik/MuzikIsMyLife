using System.Collections.Generic;
using UnityEngine;

public static class SpriteUtils
{
    private const string SpriteKeyPrefix = "SavedSprite_";

    public static void SaveSprite(int day, string spriteName)
    {
        if (string.IsNullOrEmpty(spriteName))
        {
            Debug.LogError("저장하려는 스프라이트 이름이 null이거나 비어 있습니다.");
            return;
        }

        string key = SpriteKeyPrefix + day;
        PlayerPrefs.SetString(key, spriteName);
        PlayerPrefs.Save();

        string savedValue = PlayerPrefs.GetString(key, "저장된 값 없음");
        Debug.Log($"스프라이트 저장됨: {spriteName}, Day: {day}, Key: {key}, 확인된 값: {savedValue}");
    }

    public static List<string> GetAllSavedSprites()
    {
        List<string> spriteNames = new List<string>();

        for (int day = 1; day <= 30; day++)
        {
            string key = SpriteKeyPrefix + day;
            if (PlayerPrefs.HasKey(key))
            {
                string spriteName = PlayerPrefs.GetString(key);
                spriteNames.Add(spriteName);
                Debug.Log($"로드된 스프라이트 이름: {spriteName}, Day: {day}, Key: {key}");
            }
        }

        Debug.Log($"총 저장된 스프라이트 수: {spriteNames.Count}");
        return spriteNames;
    }

    public static void ResetAllSavedSprites()
    {
        for (int day = 1; day <= 30; day++)
        {
            string key = SpriteKeyPrefix + day;
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
            }
        }
        PlayerPrefs.Save();
        Debug.Log("모든 저장된 스프라이트가 초기화되었습니다.");
    }

    public static void DebugPlayerPrefsKeys()
    {
        for (int i = 1; i <= 30; i++) // 최대 30일까지 검사 (필요에 따라 범위 조정 가능)
        {
            string key = SpriteKeyPrefix + i;
            if (PlayerPrefs.HasKey(key))
            {
                string value = PlayerPrefs.GetString(key);
                Debug.Log($"Key: {key}, Value: {value}");
            }
        }
    }
}
