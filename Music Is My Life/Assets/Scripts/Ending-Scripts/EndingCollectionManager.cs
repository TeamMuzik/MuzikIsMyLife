using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EndingCollectionManager
{
    // 엔딩 잠금 해제 및 저장
    public static void UnlockAndSaveEnding(string sceneName)
  {
      string endingKey = sceneName.Replace("-", "");
      PlayerPrefs.SetInt(endingKey, 1);
      PlayerPrefs.Save(); // 변경사항 저장
      Debug.Log("엔딩 잠금 해제 및 저장: " + endingKey);
  }

    // 엔딩 잠금 해제 여부 확인
    public static bool IsEndingUnlocked(string sceneName)
{
    string endingKey = "Ending" + sceneName.Replace("-", "");
    bool isUnlocked = PlayerPrefs.GetInt(endingKey, 0) == 1;
    Debug.Log("엔딩 잠금 해제 여부 확인: " + endingKey + " - " + isUnlocked);
    return isUnlocked;
}

    // 잠금 해제된 모든 엔딩 가져오기
    public static List<string> GetAllUnlockedEndings()
    {
        List<string> allEndingKeys = new List<string> { "EndingExpedition", "EndingOpeningBand", "EndingConcertInKorea", "EndingNormal", "EndingNarak","EndingRich"};
        List<string> unlockedEndings = new List<string>();
        foreach (var key in allEndingKeys)
        {
            if (IsEndingUnlocked(key.Replace("Ending", "")))
            {
                unlockedEndings.Add(key);
            }
        }
        return unlockedEndings;
    }
}
