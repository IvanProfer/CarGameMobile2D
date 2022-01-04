using System.Collections;
using UnityEngine;

namespace Tool.Tween.Examples
{
    internal class MathfLerpComponent : MonoBehaviour
    {
        private const float MinValue = 0;
        private const float MaxValue = 100;

        [SerializeField, Range(MinValue, MaxValue)] private float _range;
        [SerializeField] private float _duration;

        private Coroutine _coroutine;


        [ContextMenu(nameof(Play))]
        public void Play()
        {
            Stop();
            _coroutine = StartCoroutine(Playing());
        }

        [ContextMenu(nameof(Stop))]
        public void Stop()
        {
            if (_coroutine == null)
                return;

            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        [ContextMenu(nameof(Reset))]
        public void Reset()
        {
            Stop();
            _range = MinValue;
        }

        private IEnumerator Playing()
        {
            for (float t = 0; t < _duration; t += Time.deltaTime)
            {
                float ratio = CalcRatio(t);
                _range = Mathf.Lerp(MinValue, MaxValue, ratio);
                yield return null;
            }

            _range = MaxValue;
        }

        private float CalcRatio(float time)
        {
            if (_duration == 0)
                return 0;

            return time / _duration;
        }
    }
}