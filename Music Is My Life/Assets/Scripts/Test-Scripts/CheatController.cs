using UnityEditor;
using UnityEngine;
public class CheatController : MonoBehaviour
{
    public StatusController statusController;

    public void AddMoneyForTest()
    {
        StatusChanger.EarnMoney(30);
        statusController.UpdateStatus();
    }

    public void AddMyFameForTest()
    {
        StatusChanger.UpdateMyFame(30);
        statusController.UpdateStatus();
    }

    public void AddBandFameForTest()
    {
        StatusChanger.UpdateBandFame(30);
        statusController.UpdateStatus();
    }
}