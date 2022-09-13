using Assets.CodeBase.Datas;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.CodeBase.UI
{
    public class AbilityPurchaseScreen : MonoBehaviour
    {
        private const string MAX_LVL = "MAX LVL";

        [SerializeField] private BuyPurchaseView _buyMoveSpeedView;
        [SerializeField] private BuyPurchaseView _buyParkourSpeedView;
        [SerializeField] private BuyPurchaseView _buySneakerRechargeView;
        [SerializeField] private Button _missClickButton;

        private PlayerProgress _playerProgress;
        private Stats _stats;

        public event Action TapHappened;

        public void Construct(PlayerProgress playerProgress, Stats stats)
        {
            _playerProgress = playerProgress;
            _stats = stats;

            _missClickButton.onClick.AddListener(() => TapHappened?.Invoke());

            _buyMoveSpeedView.Construct(PurchaseType.MoveSpeed);
            _buyParkourSpeedView.Construct(PurchaseType.ParcourSpeed);
            _buySneakerRechargeView.Construct(PurchaseType.SneakerRecharge);

            _buyMoveSpeedView.Clicked += PurchaseBought;
            _buyParkourSpeedView.Clicked += PurchaseBought;
            _buySneakerRechargeView.Clicked += PurchaseBought;
        }

        public void Show()
        {
            gameObject.SetActive(true);

            var ms = _stats.MoveSpeedMultiplier;
            ViewPurchase(ms, _buyMoveSpeedView);

            var ps = _stats.ParkourSpeedMultiplier;
            ViewPurchase(ps, _buyParkourSpeedView);

            var sr = _stats.SneakerRecharge;
            ViewPurchase(sr, _buySneakerRechargeView);
        }

        public void Hide() => gameObject.SetActive(false);

        private void PurchaseBought(PurchaseType purchaseType)
        {
            switch (purchaseType)
            {
                case PurchaseType.MoveSpeed:
                    IncreaseAbility(_stats.MoveSpeedMultiplier);
                    break;
                case PurchaseType.ParcourSpeed:
                    IncreaseAbility(_stats.ParkourSpeedMultiplier);
                    break;
                case PurchaseType.SneakerRecharge:
                    IncreaseAbility(_stats.SneakerRecharge);
                    break;
                case PurchaseType.None:
                default:
                    throw new NotImplementedException($"Not implemented for {purchaseType}");
            }

            Show();
        }

        private void IncreaseAbility(Ability ability)
        {
            _playerProgress.PlayerLoot.Spend(ability.NextLevelCost);
            ability.Level++;
        }

        private void ViewPurchase(Ability ability, BuyPurchaseView purchaseView)
        {
            int money = _playerProgress.PlayerLoot.Collected;
            purchaseView.View(
                state: ability.MaxLevelReached == false && ability.NextLevelCost <= money,
                nextLevel: ability.MaxLevelReached ? MAX_LVL : $"LVL {ability.NextLevel}",
                cost: ability.MaxLevelReached ? MAX_LVL : ability.NextLevelCost.ToString(),
                name: ability.Name
                );
        }
    }

    public enum PurchaseType
    {
        None,
        MoveSpeed,
        ParcourSpeed,
        SneakerRecharge,
    }
}