using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.Utils
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(Collapse))]
    [CanEditMultipleObjects]
    public class CollapseEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Collapse unrollMask = (Collapse)target;
            if (GUILayout.Button("Enlarge"))
            {
                unrollMask.Enlarge();
            }
            if (GUILayout.Button("Collapse Down"))
            {
                unrollMask.CollapseDown();
            } 
        }
        
    }
#endif

    
    [RequireComponent(typeof(RectTransform))]
    public class Collapse : MonoBehaviour
    {

        public delegate void OnAnimationFinished();
        
        [SerializeField]
        float duration;

        [SerializeField]
        AnimationCurve curve;

        [SerializeField] RectTransform rectTransform;
        Coroutine moveCoroutine;
        
        [SerializeField] private bool enlargeOnAwake = false;
        [SerializeField] private bool scaleToZeroOnAwake = true;
        
        public float EnlargeTargetScale = 1f;

        private void Awake()
        {
            if (scaleToZeroOnAwake)
            {
                rectTransform.localScale = Vector3.zero;
            }
            if (enlargeOnAwake) {
                Enlarge();
            }
        }

        public void CollapseDown(OnAnimationFinished callback = null) {
            if (IsAlreadyTarget(0)) return;
            _clearCoroutine();
            gameObject.SetActive(true);
            StartCoroutine(_move(duration, EnlargeTargetScale, 0, callback));
        }

        public void Enlarge(OnAnimationFinished callback = null) {
            //if (IsAlreadyTarget(1)) return;
            _clearCoroutine();
            gameObject.SetActive(true);
            StartCoroutine(_move(duration, 0, EnlargeTargetScale, callback));
        }

        public void CollapseNow() {
            _clearCoroutine();
            rectTransform.localScale = new Vector3(0f, 0f, 0f);
        }

        private bool IsAlreadyTarget(float target) {
            return rectTransform.localScale == new Vector3(target, target, target);
        }

        private void _clearCoroutine() {
            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
            }
        }

        private IEnumerator _move(float duration, float initialScale, float targetScale, OnAnimationFinished callback = null) {
            float elapsed = 0f;

            while (true) {
                float progress = curve.Evaluate(elapsed / duration);
                elapsed += Time.deltaTime;
                float scaleValue = Mathf.Lerp(initialScale, targetScale, progress);
                rectTransform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
                if (progress >= 1f) {
                    callback?.Invoke();
                    yield break;
                }
                yield return null;
            }
        }
    }
}
