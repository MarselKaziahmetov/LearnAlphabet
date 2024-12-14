using System.Collections.Generic;
using UnityEngine;

namespace LearnAlphabet
{
    public class CardBundleView : MonoBehaviour
    {
        [SerializeField] private float _offset;

        private float _cardLegthX;
        private float _cardLegthY;

        public void SetHolderScale(int rowsCount, int columnsCount, float cardLegthX, float cardLegthY)
        {
            _cardLegthX = cardLegthX;
            _cardLegthY = cardLegthY;

            var xLength = _cardLegthX * columnsCount + _offset * (columnsCount + 1);
            var yLength = _cardLegthY * rowsCount + _offset * (rowsCount + 1);
            transform.localScale = new Vector3(xLength, yLength, 1);
        }

        public void PutCardOnPlaces(CardBundleData cardBundle, List<CardView> currentActiveCards)
        {
            if (currentActiveCards.Count == 0) return;

            var rows = cardBundle.Rows;
            var columns = cardBundle.Columns;

            var cardWidth = _cardLegthX;
            var cardHeight = _cardLegthY;

            var totalWidth = columns * cardWidth + (columns - 1) * _offset;
            var totalHeight = rows * cardHeight + (rows - 1) * _offset;

            var startX = -totalWidth / 2 + cardWidth / 2;
            var startY = totalHeight / 2 - cardHeight / 2;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var index = row * columns + col;
                    if (index >= currentActiveCards.Count) 
                        break;

                    var card = currentActiveCards[index];
                    var xPos = startX + col * (cardWidth + _offset);
                    var yPos = startY - row * (cardHeight + _offset);

                    card.transform.localPosition = new Vector3(xPos, yPos, 0);
                }
            }
        }
    }
}