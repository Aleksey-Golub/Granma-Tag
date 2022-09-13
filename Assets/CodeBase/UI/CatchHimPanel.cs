using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

namespace Assets.CodeBase.UI
{
    public class CatchHimPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _text;
        [SerializeField] private TextMeshProUGUI _stageName;

        public IEnumerator ShowFor(string sceneName, GameMode gameMode, int enemiesCount)
        {
            _stageName.text = $"zone {sceneName}";
            SetCatchText(gameMode, enemiesCount);
            gameObject.SetActive(true);

            yield return StartCoroutine(Animate());
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator Animate()
        {
            _text.localScale = Vector3.one;
            _canvasGroup.alpha = 1f;

            _text.DOScale(1.1f, 1f);
            yield return new WaitForSeconds(1f);
            _text.DOScale(0.9f, 1f);
            _canvasGroup.DOFade(0f, 1f);
            yield return new WaitForSeconds(0.5f);
        }

        private void SetCatchText(GameMode gameMode, int enemiesCount)
        {
            if (enemiesCount > 1)
            {
                _text.GetComponent<TextMeshProUGUI>().text = gameMode switch
                {
                    GameMode.GranmaTagBully => "Catch Them!",
                    GameMode.BullyEscapesFromGranma => "Run From Them!",
                    GameMode.None => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            }
            else if(enemiesCount == 1)
            {
                _text.GetComponent<TextMeshProUGUI>().text = gameMode switch
                {
                    GameMode.GranmaTagBully => "Catch Him!",
                    GameMode.BullyEscapesFromGranma => "Run From Her!",
                    GameMode.None => throw new NotImplementedException(),
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
                throw new ArgumentOutOfRangeException($"{enemiesCount} have to be >= 1");
            }
        }
    }
}