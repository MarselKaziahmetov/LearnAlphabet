using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LearnAlphabet
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _transparentSplash;
        [SerializeField] private CanvasGroup _fullSplash;
        [SerializeField] private Button _retryButton;

        public Action RetryClicked;

        private void OnEnable() => _retryButton.onClick.AddListener(Retry);
        private void OnDisable() => _retryButton.onClick.RemoveAllListeners();

        public void Finish()
        {
            _transparentSplash.DOFade(.8f, 2f);
            _retryButton.gameObject.SetActive(true);
        }

        private void Retry()
        {
            StartCoroutine(RetryCoroutine());
        }

        private IEnumerator RetryCoroutine()
        {
            _transparentSplash.alpha = 0;
            _retryButton.gameObject.SetActive(false);

            _fullSplash.DOFade(1, 1f);
            yield return new WaitForSeconds(1);

            _transparentSplash.alpha = 0;
            _retryButton.gameObject.SetActive(false);
            RetryClicked?.Invoke();

            _fullSplash.DOFade(0, 1f);
            yield return new WaitForSeconds(1);
        }
    }
}