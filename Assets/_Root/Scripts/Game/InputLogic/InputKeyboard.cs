using JoostenProductions;
using UnityEngine;

namespace Game.InputLogic
{
    internal class InputKeyboard : BaseInputView
    {
        [SerializeField] private float _inputMultiplier = 0.01f;


        private void Start() =>
            UpdateManager.SubscribeToUpdate(Move);

        private void OnDestroy() =>
            UpdateManager.UnsubscribeFromUpdate(Move);


        private void Move()
        {
            float moveValue = _inputMultiplier * _speed;

            if (Input.GetKey(KeyCode.LeftArrow))
                OnLeftMove(moveValue);

            if (Input.GetKey(KeyCode.RightArrow))
                OnRightMove(moveValue);
        }
    }
}
