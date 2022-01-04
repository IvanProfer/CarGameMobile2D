using Tool;
using System;
using System.Collections.Generic;
using Profile;
using Services;
using UnityEngine;
using Game.InputLogic;
using Game.TapeBackground;
using Game.Transport;
using Game.Transport.Boat;
using Game.Transport.Car;
using Features.AbilitySystem;
using Features.AbilitySystem.Abilities;

namespace Game
{
    internal class GameController : BaseController
    {
        private readonly ProfilePlayer _profilePlayer;
        private readonly SubscriptionProperty<float> _leftMoveDiff;
        private readonly SubscriptionProperty<float> _rightMoveDiff;

        private readonly TapeBackgroundController _tapeBackgroundController;
        private readonly InputGameController _inputGameController;
        private readonly TransportController _transportController;
        private readonly IAbilitiesController _abilitiesController;


        public GameController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;
            _leftMoveDiff = new SubscriptionProperty<float>();
            _rightMoveDiff = new SubscriptionProperty<float>();

            _tapeBackgroundController = CreateTapeBackground();
            _inputGameController = CreateInputGameController();
            _transportController = CreateTransportController();
            _abilitiesController = CreateAbilitiesController(placeForUi);

            ServiceLocator.Analytics.SendGameStarted();
        }


        private TapeBackgroundController CreateTapeBackground()
        {
            var tapeBackgroundController = new TapeBackgroundController(_leftMoveDiff, _rightMoveDiff);
            AddController(tapeBackgroundController);

            return tapeBackgroundController;
        }

        private InputGameController CreateInputGameController()
        {
            var inputGameController = new InputGameController(_leftMoveDiff, _rightMoveDiff, _profilePlayer.CurrentTransport);
            AddController(inputGameController);

            return inputGameController;
        }

        private TransportController CreateTransportController()
        {
            TransportController transportController;
            TransportModel transportModel = _profilePlayer.CurrentTransport;

            switch (_profilePlayer.CurrentTransport.Type)
            {
                case TransportType.Car:
                    transportController = new CarController(transportModel);
                    break;
                case TransportType.Boat:
                    transportController = new BoatController(transportModel);
                    break;
                default:
                    throw new ArgumentException(nameof(TransportType));
            }

            AddController(transportController);

            return transportController;
        }

        private IAbilitiesController CreateAbilitiesController(Transform placeForUi)
        {
            AbilityItemConfig[] abilityItemConfigs = LoadAbilityItemConfigs();
            var repository = CreateAbilitiesRepository(abilityItemConfigs);
            var view = LoadAbilitiesView(placeForUi);

            var abilitiesController = new AbilitiesController(view, repository, abilityItemConfigs, _transportController);
            AddController(abilitiesController);

            return abilitiesController;
        }

        private AbilityItemConfig[] LoadAbilityItemConfigs()
        {
            var path = new ResourcePath("Configs/Ability/AbilityItemConfigDataSource");
            return ContentDataSourceLoader.LoadAbilityItemConfigs(path);
        }

        private IAbilitiesRepository CreateAbilitiesRepository(IEnumerable<IAbilityItem> abilityItemConfigs)
        {
            var repository = new AbilitiesRepository(abilityItemConfigs);
            AddRepository(repository);

            return repository;
        }

        private IAbilitiesView LoadAbilitiesView(Transform placeForUi)
        {
            var path = new ResourcePath("Prefabs/Ability/AbilitiesView");

            GameObject prefab = ResourcesLoader.LoadPrefab(path);
            GameObject objectView = UnityEngine.Object.Instantiate(prefab, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<AbilitiesView>();
        }
    }
}
