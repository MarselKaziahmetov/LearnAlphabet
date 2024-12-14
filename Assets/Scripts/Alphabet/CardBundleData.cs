using UnityEngine;

namespace LearnAlphabet
{
    [CreateAssetMenu(fileName = "New CardBundleData", menuName = "Card Bundle Data")]
    public class CardBundleData : ScriptableObject
    {
        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        [SerializeField] private CardData[] _cardsData;

        public CardData[] CardsData => _cardsData;
        public int Rows => _rows;
        public int Columns => _columns;

        private void OnValidate()
        {
            ValidateCardsDataLength();
        }

        private void ValidateCardsDataLength()
        {
            var targetLength = _rows * _columns;

            if (_cardsData == null || _cardsData.Length != targetLength)
            {
                var newCardData = new CardData[targetLength];

                if (_cardsData != null)
                    for (int i = 0; i < Mathf.Min(_cardsData.Length, targetLength); i++)
                        newCardData[i] = _cardsData[i];

                _cardsData = newCardData;
            }
        }
    }
}