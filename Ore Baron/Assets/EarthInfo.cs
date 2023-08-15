using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EarthInfo : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DescriptionText;
    public TextMeshProUGUI PriceText;
    public Button BuyButton;
    public GameObject Lock;
    public GameObject CompletedIcon;
    public GameObject ButtonObject;

    public void OnEnable()
    {
        BuyButton.onClick.AddListener(BuyEarth);
    }
    public void OnDisable()
    {
        BuyButton.onClick.RemoveListener(BuyEarth);
    }

    public void BuyEarth()
    {
        GameController.Instance.BuyEarth();
    }
    public void Unlock()
    {
        Lock.SetActive(false);
    }
    public void Complete()
    {
        CompletedIcon.SetActive(true);
        ButtonObject.SetActive(false);
    }
}
