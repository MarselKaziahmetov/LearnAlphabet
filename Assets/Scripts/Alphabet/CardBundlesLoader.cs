using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LearnAlphabet
{
    public class CardBundlesLoader : MonoBehaviour
    {
        [SerializeField] private CardView _cardViewPrefab;
        [SerializeField] private CardBundleData[] _cardBundleData;
        [SerializeField] private CardBundleView _cardBundleView;
        [SerializeField] private QuestsGiver _questGiver;
        [SerializeField] private UIView _UiView;
        [SerializeField] private Transform _cardsHolder;

        private List<CardView> _currentActiveCards = new List<CardView>();
        private int _curentBundle = 0;

        private void Start()
        {
            LoadNextBundle();
            CardAppearenceAnimation();
        }

        private void OnEnable() => _UiView.RetryClicked += LoadNextBundle;
        private void OnDisable() => _UiView.RetryClicked -= LoadNextBundle;

        private void LoadNextBundle()
        {
            if (_curentBundle == _cardBundleData.Length) 
            {
                _UiView.Finish();
                _curentBundle = 0;
                return;
            }

            ClearPreviousBundle();

            foreach (var card in _cardBundleData[_curentBundle].CardsData)
            {
                var newcard = Instantiate(_cardViewPrefab, _cardsHolder);
                newcard.InitCardView(card);
                _currentActiveCards.Add(newcard);
                newcard.Tapped += OnAnswered;
            }

            _questGiver.ChooseRandomQuest(_cardBundleData[_curentBundle]);
            _cardBundleView.SetHolderScale(_cardBundleData[_curentBundle].Rows, _cardBundleData[_curentBundle].Columns, _cardViewPrefab.transform.localScale.x, _cardViewPrefab.transform.localScale.y);
            _cardBundleView.PutCardOnPlaces(_cardBundleData[_curentBundle], _currentActiveCards);

            _curentBundle++;
        }

        private void ClearPreviousBundle()
        {
            foreach (var card in _currentActiveCards)
                Destroy(card.gameObject);

            _currentActiveCards.Clear();
        }

        private void CardAppearenceAnimation()
        {
            foreach (var card in _currentActiveCards)
                card.transform.localScale = Vector3.zero;

            foreach (var card in _currentActiveCards)
                card.transform.DOScale(_cardViewPrefab.transform.localScale, 2).SetEase(Ease.OutBounce);
        }

        private void OnAnswered(string cardId, CardView cardView)
        {
            if (cardId == _questGiver.CorrectAnswerId)
            {
                foreach(var card in _currentActiveCards)
                    card.Tapped -= OnAnswered;

                cardView.CorrectAnimation();

                Invoke(nameof(LoadNextBundle), 2f);
            }
            else 
                cardView.UncorrectAnimation();
        }
    }
}