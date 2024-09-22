using UnityEditor;
using UnityEngine;
public class CheatController : MonoBehaviour
{
    public StatusController statusController;

    public void AddMoneyForTest()
    {
        StatusChanger.EarnMoney(50);
        statusController.UpdateStatus();
    }

    public void AddMyFameForTest()
    {
        StatusChanger.UpdateMyFame(30);
        statusController.UpdateStatus();
    }

    public void AddBandFameForTest()
    {
        StatusChanger.UpdateBandFame(50);
        statusController.UpdateStatus();
    }
}