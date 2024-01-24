using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public TMP_Text playerName;

    void Start()
    {
        PlayerNameText();
    }

    public void PlayerNameText()
    {
        playerName.text = PlayerPrefs.GetString("PlayerName");
    }
}
