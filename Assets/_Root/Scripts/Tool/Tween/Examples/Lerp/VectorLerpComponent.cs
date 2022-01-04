using System.Collections;
using UnityEngine;

namespace Tool.Tween.Examples
{
    public enum RatioType
    {
        Linear,
        Quad,
        Root
    }

    internal class VectorLerpComponent : MonoBehaviour
    {
        [SerializeField] private RatioType _ratioType;
        [SerializeField] private float _duration;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;

        private Coroutine _coroutine;


        public void Play(Vector3 startPosition, Vector3 endPosition, float duration)
        {
            Stop();
            _coroutine = StartCoroutine(Playing(startPosition, endPosition, _duration));
        }

        [ContextMenu(nameof(Play))]
        public void Play()
        {
            Vector3 startPosition = _startPoint.position;
            Vector3 endPosition = _endPoint.position;

            Play(startPosition, endPosition, _duration);
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
            transform.position = _startPoint.position;
        }

        private IEnumerator Playing(Vector3 startPosition, Vector3 endPosition, float duration)
        {
            Transform transform = this.transform;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float ratio = CalcRatio(t, duration);
                transform.position = Vector3.Lerp(startPosition, endPosition, ratio);
                yield return null;
            }
        }

        private float CalcRatio(float time, float duration)
        {
            if (duration == 0)
                return 0;

            float ratio = time / duration;

            switch (_ratioType)
            {
                case RatioType.Linear: return ratio;
                case RatioType.Quad: return Mathf.Pow(ratio, 2);
                case RatioType.Root: return Mathf.Sqrt(ratio);
                default: return ratio;
            }
        }
    }
}
