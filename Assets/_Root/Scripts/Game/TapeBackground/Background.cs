using UnityEngine;

namespace Game.TapeBackground
{
    internal class Background : MonoBehaviour
    {
        [SerializeField] private float _leftBorder;
        [SerializeField] private float _rightBorder;
        [SerializeField] private float _relativeSpeedRate;


        public void Move(float value)
        {
            Vector3 position = transform.position;
            position += Vector3.right * value * _relativeSpeedRate;

            if (position.x <= _leftBorder)
                position.x = _rightBorder - (_leftBorder - position.x);

            else if (position.x >= _rightBorder)
                position.x = _leftBorder + (_rightBorder - position.x);

            transform.position = position;
        }
    }
}
