using Profile;
using Tool;
using UnityEngine;

namespace Features.Fight
{
    internal class StartFightController : BaseController
    {
        private readonly ResourcePath _resourcePath = new ResourcePath("Prefabs/Fight/StartFightView");

        private readonly StartFightView _view;
        private readonly ProfilePlayer _profilePlayer;


        public StartFightController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;
            _view = LoadView(placeForUi);
            SubscribeView();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            UnsubscribeView();
        }


        private StartFightView LoadView(Transform placeForUi)
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_resourcePath);
            GameObject objectView = Object.Instantiate(prefab, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<StartFightView>();
        }

        private void SubscribeView() =>
            _view.StartFightButton.onClick.AddListener(StartFight);

        private void UnsubscribeView() =>
            _view.StartFightButton.onClick.RemoveListener(StartFight);

        private void StartFight() =>
            _profilePlayer.CurrentState.Value = GameState.Fight;
    }
}
