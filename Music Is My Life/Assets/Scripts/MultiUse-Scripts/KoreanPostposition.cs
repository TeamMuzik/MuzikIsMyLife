using UnityEngine;

public class KoreanPostposition : MonoBehaviour
{
    public static string SubjectCaseMarkerEunNeun(string input)
    {
        char lastChar = input[input.Length - 1];
        int jongseong = (lastChar - 0xAC00) % 28;
        Debug.Log("lastChar: " + lastChar);
        Debug.Log("jongseong: " + jongseong);
        if (jongseong != 0)
            return "은";
        return "는";
    }
}