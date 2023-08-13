using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineInfo : MonoBehaviour
{
    public Image Icon;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI AmountText;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI IncomeTotal;
    public Button BuyButton;
    public int OreIndex;

    public void OnEnable()
    {
        BuyButton.onClick.AddListener(BuyMine);
    }
    public void OnDisable()
    {
        BuyButton.onClick.RemoveListener(BuyMine);
    }

    public void BuyMine()
    {
        GameController.Instance.BuyMine(OreIndex);
    }
}
