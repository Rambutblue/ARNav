using System.Collections;
using UnityEngine;

namespace Scripts.Util {
    public class Fade : MonoBehaviour {

        public delegate void Callback();
    
        [SerializeField]
        CanvasGroup canvasGroup;

        [SerializeField]
        float lowAlpha, highAlpha;

        [SerializeField]
        AnimationCurve curve;

        [SerializeField]
        float duration;

        Coroutine fadeCoroutine;

        public float CurrentAlpha
        {
            get => canvasGroup.alpha;
            set => canvasGroup.alpha = value;
        }

        public void FadeOut(Callback callback = null)
        {
            _clearCoroutine();
            gameObject.SetActive(true);
            fadeCoroutine = StartCoroutine(_fade(highAlpha, lowAlpha, duration,false, callback));
        }

        public void FadeIn(Callback callback = null)
        {
            _clearCoroutine();
            gameObject.SetActive(true);
            fadeCoroutine = StartCoroutine(_fade(lowAlpha, highAlpha, duration,true, callback));
        }

        private void _clearCoroutine()
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
        }

        private IEnumerator _fade(float initialAlpha, float targetAlpha, float duration, bool status, Callback callback)
        {
            float elapsed = 0f;
            while (true)
            {
                float progress = curve.Evaluate(elapsed / duration);
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, progress);
                if (progress >= 1f)
                {
                    gameObject.SetActive(status);
                    callback?.Invoke();
                    yield break;
                }
                yield return null;
            }
        }
    }
}