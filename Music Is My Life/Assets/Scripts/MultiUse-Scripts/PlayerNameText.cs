using UnityEngine;
using TMPro;

public class PlayerNameText : MonoBehaviour
{
    public TMP_Text playerName;

    void Start()
    {
        GetPlayerNameText();
    }

    public void GetPlayerNameText()
    {
        playerName.text = PlayerPrefs.GetString("PlayerName");
    }
}
