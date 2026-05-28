using TMPro;
using UnityEngine;

public class Menu_CoinZone : MonoBehaviour, IUpdateUIProperty
{
    [SerializeField] protected TextMeshProUGUI txtCoin;
    protected int TempCount = 0;
    private void Start()
    {
        //this.TempCount = Facade.Instance.ConfigManager.UserConfig.UserPropertyData.Gold;
    }
    public void IUpdateUIProperty()
    {
        //this.txtCoin.text = Facade.Instance.ConfigController.UserConfig.UserPropertyData.Gold.ToString();
    }

    public void IUpdateUIPropertyFake(int Number, int MaxNumber)
    {
        //int TempCount = Facade.Instance.ConfigController.UserConfig.UserPropertyData.Gold - MaxNumber + Number;
        //this.txtCoin.text = TempCount.ToString();
    }
}
