using DG.Tweening;
using System;
using UnityEngine;

namespace LearnAlphabet
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _cardSprite;
        private SpriteRenderer _cardBackground;
        private CardData _cardData;

        public Action<string, CardView> Tapped;

        private void Awake()
        {
            _cardBackground = GetComponent<SpriteRenderer>();
            SetRandomBackground();
        }

        private void OnMouseDown()
        {
            Tapped?.Invoke(_cardData.Id, this);
        }

        public void CorrectAnimation()
        {
            transform.DOScale(transform.localScale * 1.2f, 2).SetEase(Ease.OutBounce);
        }

        public void UncorrectAnimation()
        {
            transform.DOShakePosition(1, new Vector3(0.5f, 0, 0)).SetEase(Ease.OutBounce);
        }

        public void InitCardView(CardData newCard)
        {
            _cardData = newCard;

            _cardSprite.sprite = _cardData.Sprite;
        }

        private void SetRandomBackground()
        {
            Color randomColor = new Color(
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f),
                UnityEngine.Random.Range(0f, 1f)  
            );

            _cardBackground.color = randomColor;
        }
    }
}