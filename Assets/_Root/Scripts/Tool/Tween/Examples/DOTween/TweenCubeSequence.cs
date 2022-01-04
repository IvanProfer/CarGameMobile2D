using DG.Tweening;
using UnityEngine;

namespace Tool.Tween.Examples.DOTween
{
    internal class TweenCubeSequence : MonoBehaviour
    {
        [SerializeField] private float _duration;

        [Header("Moving")]
        [SerializeField] private int _countMoveLoops;
        [SerializeField] private Ease _moveEase;
        [SerializeField] private Vector3 _endPosition;

        [Header("Scaling")]
        [SerializeField] private float _scaleDelay;
        [SerializeField] private Vector3 _endScale;

        [Header("Jumping")]
        [SerializeField] private float _jumpDelay;
        [SerializeField] private Vector3 _jumpPosition;
        [SerializeField] private float _jumpPower;
        [SerializeField] private int _jumpCount;


        [ContextMenu(nameof(Play))]
        public void Play()
        {
            Sequence sequence = DG.Tweening.DOTween.Sequence();

            sequence.Append(transform.DOMove(_endPosition, _duration).SetLoops(_countMoveLoops).SetEase(_moveEase));
            sequence.Insert(_scaleDelay, transform.DOScale(_endScale, _duration));
            sequence.Insert(_jumpDelay, transform.DOJump(_jumpPosition, _jumpPower, _jumpCount, _duration));
        }
    }
}
