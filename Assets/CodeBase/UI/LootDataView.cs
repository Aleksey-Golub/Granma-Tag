using UnityEngine;
using TMPro;
using Assets.CodeBase.Datas;

namespace Assets.CodeBase.UI
{
    public class LootDataView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _softCurrencyText;

        private PlayerLootData _lootData;

        public void Construct(PlayerLootData lootData)
        {
            _lootData = lootData;
            _lootData.Changed += OnLootDataChanged;

            OnLootDataChanged();
        }

        private void OnLootDataChanged()
        {
            _softCurrencyText.text = _lootData.Collected.ToString();
        }
    }
}
