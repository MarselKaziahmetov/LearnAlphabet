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
        [SerializeField] private TextMeshProUGUI _questText;
        [SerializeField] private Transform _cardsBackplate;
        [SerializeField] private Transform _cardsHolder;
        [SerializeField] private float _offset;
        [SerializeField] private UIView _UiView;

        private List<CardView> _currentActiveCards = new List<CardView>();
        private CardView _selectedCard;
        private int _curentBundle = 0;
        private string _correctAnswerId;

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

            ChooseRandomQuest(_cardBundleData[_curentBundle]);
            UpdateQuestText();
            SetHolderScale(_cardBundleData[_curentBundle].Rows, _cardBundleData[_curentBundle].Columns);
            PutCardOnPlaces(_cardBundleData[_curentBundle]);

            _curentBundle++;
        }

        private void ClearPreviousBundle()
        {
            foreach (var card in _currentActiveCards)
                Destroy(card.gameObject);

            _currentActiveCards.Clear();
        }

        private void ChooseRandomQuest(CardBundleData cardBundle)
        {
            var randomValue = Random.Range(0, cardBundle.CardsData.Length);

            _correctAnswerId = cardBundle.CardsData[randomValue].Id;
        }

        private void UpdateQuestText()
        {
            _questText.text = $"Find {_correctAnswerId}";
        }

        private void SetHolderScale(int rowsCount, int columnsCount)
        {
            var xLength = _cardViewPrefab.transform.localScale.x * columnsCount + _offset * (columnsCount + 1);
            var yLength = _cardViewPrefab.transform.localScale.y * rowsCount + _offset * (rowsCount + 1);
            _cardsBackplate.localScale = new Vector3(xLength, yLength, 1);
        }

        private void PutCardOnPlaces(CardBundleData cardBundle)
        {
            if (_currentActiveCards.Count == 0) return;

            var rows = cardBundle.Rows;
            var columns = cardBundle.Columns;

            var cardWidth = _cardViewPrefab.transform.localScale.x;
            var cardHeight = _cardViewPrefab.transform.localScale.y;

            var totalWidth = columns * cardWidth + (columns - 1) * _offset;
            var totalHeight = rows * cardHeight + (rows - 1) * _offset;

            var startX = -totalWidth / 2 + cardWidth / 2;
            var startY = totalHeight / 2 - cardHeight / 2;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var index = row * columns + col;
                    if (index >= _currentActiveCards.Count) break;

                    var card = _currentActiveCards[index];
                    var xPos = startX + col * (cardWidth + _offset);
                    var yPos = startY - row * (cardHeight + _offset);

                    card.transform.localPosition = new Vector3(xPos, yPos, 0);
                }
            }
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
            _selectedCard = cardView;

            if (cardId == _correctAnswerId)
            {
                foreach(var card in _currentActiveCards)
                    card.Tapped -= OnAnswered;

                _selectedCard.CorrectAnimation();

                Invoke(nameof(LoadNextBundle), 2f);
            }
            else 
            {
                _selectedCard.UncorrectAnimation();
            }
        }
    }
}