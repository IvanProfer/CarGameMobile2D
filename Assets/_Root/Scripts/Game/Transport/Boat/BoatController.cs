using Tool;
using UnityEngine;

namespace Game.Transport.Boat
{
    internal class BoatController : TransportController
    {
        private readonly ResourcePath _viewPath = new ResourcePath("Prefabs/Boat");
        private readonly BoatView _view;

        public override GameObject ViewGameObject => _view.gameObject;


        public BoatController(TransportModel model) : base(model) =>
            _view = LoadView();


        private BoatView LoadView()
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_viewPath);
            GameObject objectView = Object.Instantiate(prefab);
            AddGameObject(objectView);

            return objectView.GetComponent<BoatView>();
        }
    }
}
