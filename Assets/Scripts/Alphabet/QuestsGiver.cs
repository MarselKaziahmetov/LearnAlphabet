using TMPro;
using UnityEngine;

namespace LearnAlphabet
{
    public class QuestsGiver : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _questText;
        private string _correctAnswerId;

        public string CorrectAnswerId => _correctAnswerId;

        public void ChooseRandomQuest(CardBundleData cardBundle)
        {
            var randomValue = Random.Range(0, cardBundle.CardsData.Length);

            _correctAnswerId = cardBundle.CardsData[randomValue].Id;

            UpdateQuestText();
        }

        private void UpdateQuestText()
        {
            _questText.text = $"Find {_correctAnswerId}";
        }
    }
}
