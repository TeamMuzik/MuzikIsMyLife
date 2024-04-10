using UnityEditor;
using UnityEngine;
public class AddMoneyTest : MonoBehaviour
{
    public StatusController statusController;

    public void AddMoneyForTest() {
        StatusChanger.EarnMoney(50);
        statusController.UpdateStatus();
    }
}