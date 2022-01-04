using UnityEngine;

namespace Tool.Tween.Examples
{
    internal static class GameObjectExtensions
    {
        public static void Move(this GameObject go, Vector3 startPosition, Vector3 endPosition, float duration)
        {
            if (go.TryGetComponent<VectorLerpComponent>(out var component) == false)
                return;

            component.Play(startPosition, endPosition, duration);
        }
    }
}
