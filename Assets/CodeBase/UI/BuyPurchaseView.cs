using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.CodeBase.UI
{
    public class BuyPurchaseView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _levelCostText;
        [SerializeField] private TextMeshProUGUI _nameText;
        
        private PurchaseType _purchaseType;

        public event Action<PurchaseType> Clicked;

        public void Construct(PurchaseType purchaseType)
        {
            _purchaseType = purchaseType;

            _button.onClick.AddListener(() => Clicked?.Invoke(_purchaseType));
        }

        public void View(bool state, string nextLevel, string cost, string name)
        {
            _button.interactable = state;
            _levelText.text = nextLevel;
            _levelCostText.text = cost;
            _nameText.text = name;
        }
    }
}
