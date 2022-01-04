using System;
using UnityEngine;
using JetBrains.Annotations;
using JoostenProductions;

namespace Features.AbilitySystem.Abilities
{
    internal class JumpAbility : IAbility
    {
        private const float StartTime = 0f;

        private readonly IAbilityItem _abilityItem;

        private float _time;
        private bool _isActive;
        private Transform _transformCache;
        private float _startHeight;
        private float _targetHeight;
        private float _jumpDuration;

        private enum JumpState { Direct, Reverse }
        private JumpState _jumpState;


        public JumpAbility([NotNull] IAbilityItem abilityItem) =>
            _abilityItem = abilityItem ?? throw new ArgumentNullException(nameof(abilityItem));


        public void Apply(IAbilityActivator activator)
        {
            if (_isActive)
                return;

            StartAbility(activator);
        }

        private void StartAbility(IAbilityActivator activator)
        {
            _isActive = true;
            _time = StartTime;
            _transformCache = activator.ViewGameObject.transform;
            _startHeight = _transformCache.position.y;
            _targetHeight = _startHeight + activator.JumpHeight;
            _jumpDuration = _abilityItem.Value;
            _jumpState = JumpState.Direct;

            UpdateManager.SubscribeToUpdate(OnUpdateAbility);
        }

        private void FinishAbility()
        {
            _isActive = false;
            _time = default;
            _transformCache = default;
            _startHeight = default;
            _targetHeight = default;
            _jumpDuration = default;
            _jumpState = default;

            UpdateManager.UnsubscribeFromUpdate(OnUpdateAbility);
        }

        private void OnUpdateAbility()
        {
            UpdateTime();
            UpdatePosition();
            UpdateState();
        }


        private void UpdateTime()
        {
            switch (_jumpState)
            {
                case JumpState.Direct: IncreaseTime();
                    break;

                case JumpState.Reverse: DecreaseTime();
                    break;
            }
        }

        private void IncreaseTime() =>
            _time += Time.deltaTime;

        private void DecreaseTime() =>
            _time -= Time.deltaTime;


        private void UpdatePosition()
        {
            float currentHeight = CalcCurrentHeight();
            _transformCache.position = CalcCurrentPosition(currentHeight);
        }

        private float CalcCurrentHeight()
        {
            float ratio = _time / _jumpDuration;
            return Mathf.Lerp(_startHeight, _targetHeight, ratio);
        }

        private Vector3 CalcCurrentPosition(float height)
        {
            Vector3 position = _transformCache.position;
            position.y = height;
            return position;
        }


        private void UpdateState()
        {
            if (IsJumpPeak())
                _jumpState = JumpState.Reverse;

            if (IsJumpFinished())
                FinishAbility();
        }

        private bool IsJumpPeak() =>
            _jumpState == JumpState.Direct &&
            _time >= _jumpDuration;

        private bool IsJumpFinished() =>
            _jumpState == JumpState.Reverse &&
            _time <= StartTime;
    }
}
