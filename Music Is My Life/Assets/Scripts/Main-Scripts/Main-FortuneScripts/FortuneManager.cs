using UnityEngine;
using TMPro;

public class FortuneManager : MonoBehaviour
{
    public TMP_Text fortuneMessage;
    public TMP_Text effectMessage;
    private int fortuneId;
    private bool chance;

    public void Start()
    {
        fortuneId = DayFortune.GetTodayFortuneId();
        if (fortuneId == 0)
        {
            chance = true;
            fortuneMessage.text = "";
            effectMessage.text = "";
        }
        else
        {
            chance = false;
            Debug.Log("오늘의 운세를 이미 보았습니다.");
            fortuneMessage.text = DayFortune.GetFortuneMessage(fortuneId);
            effectMessage.text = DayFortune.GetEffectMessage(fortuneId);
        }
    }

    public void PickRandomFortune()
    {
        if (chance == false)
        {
            Debug.Log("오늘의 운세를 이미 보았습니다.");
            return;
        }
        fortuneId = DayFortune.RandomDraw();
        fortuneMessage.text = DayFortune.GetFortuneMessage(fortuneId);
        effectMessage.text = DayFortune.GetEffectMessage(fortuneId);
        chance = false;
    }

}