using Ui;
using Tool;
using Game;
using Profile;
using UnityEngine;
using System;
using System.Collections.Generic;
using Features.Fight;
using Features.Rewards;
using Features.Shed;
using Features.Shed.Upgrade;
using Features.Inventory;
using Features.Inventory.Items;
using Object = UnityEngine.Object;

internal class MainController : BaseController
{
    private readonly Transform _placeForUi;
    private readonly ProfilePlayer _profilePlayer;

    private readonly List<GameObject> _subObjects = new List<GameObject>();
    private readonly List<IDisposable> _subDisposables = new List<IDisposable>();

    private MainMenuController _mainMenuController;
    private SettingsMenuController _settingsMenuController;
    private ShedController _shedController;
    private RewardController _rewardController;
    private StartFightController _startFightController;
    private GameController _gameController;
    private FightController _fightController;


    public MainController(Transform placeForUi, ProfilePlayer profilePlayer)
    {
        _placeForUi = placeForUi;
        _profilePlayer = profilePlayer;

        profilePlayer.CurrentState.SubscribeOnChange(OnChangeGameState);
        OnChangeGameState(_profilePlayer.CurrentState.Value);
    }

    protected override void OnDispose()
    {
        DisposeControllers();
        DisposeSubInstances();
        _profilePlayer.CurrentState.UnSubscribeOnChange(OnChangeGameState);
    }


    private void OnChangeGameState(GameState state)
    {
        DisposeControllers();
        DisposeSubInstances();

        switch (state)
        {
            case GameState.Start:
                _mainMenuController = new MainMenuController(_placeForUi, _profilePlayer);
                break;
            case GameState.Settings:
                _settingsMenuController = new SettingsMenuController(_placeForUi, _profilePlayer);
                break;
            case GameState.Shed:
                _shedController = CreateShedController(_placeForUi);
                break;
            case GameState.DailyReward:
                _rewardController = new RewardController(_placeForUi, _profilePlayer);
                break;
            case GameState.Game:
                _gameController = new GameController(_placeForUi, _profilePlayer);
                _startFightController = new StartFightController(_placeForUi, _profilePlayer);
                break;
            case GameState.Fight:
                _fightController = new FightController(_placeForUi, _profilePlayer);
                break;
        }
    }

    private void DisposeControllers()
    {
        _mainMenuController?.Dispose();
        _settingsMenuController?.Dispose();
        _shedController?.Dispose();
        _rewardController?.Dispose();
        _startFightController?.Dispose();
        _gameController?.Dispose();
        _fightController?.Dispose();
    }

    private void DisposeSubInstances()
    {
        DisposeSubDisposables();
        DisposeSubObjects();
    }

    private void DisposeSubDisposables()
    {
        foreach (IDisposable disposable in _subDisposables)
            disposable.Dispose();

        _subDisposables.Clear();
    }

    private void DisposeSubObjects()
    {
        foreach (GameObject gameObject in _subObjects)
            Object.Destroy(gameObject);

        _subObjects.Clear();
    }


    private ShedController CreateShedController(Transform placeForUi)
    {
        InventoryController inventoryController = CreateInventoryController(placeForUi);
        UpgradeHandlersRepository shedRepository = CreateShedRepository();
        ShedView shedView = LoadShedView(placeForUi);

        return new ShedController
        (
            shedView,
            _profilePlayer,
            inventoryController,
            shedRepository
        );
    }

    private UpgradeHandlersRepository CreateShedRepository()
        {
            var path = new ResourcePath("Configs/Shed/UpgradeItemConfigDataSource");

            UpgradeItemConfig[] upgradeConfigs = ContentDataSourceLoader.LoadUpgradeItemConfigs(path);
            var repository = new UpgradeHandlersRepository(upgradeConfigs);
            _subDisposables.Add(repository);

            return repository;
        }

        private ShedView LoadShedView(Transform placeForUi)
        {
            var path = new ResourcePath("Prefabs/Shed/ShedView");

            GameObject prefab = ResourcesLoader.LoadPrefab(path);
            GameObject objectView = Object.Instantiate(prefab, placeForUi, false);
            _subObjects.Add(objectView);

            return objectView.GetComponent<ShedView>();
        }

        private InventoryController CreateInventoryController(Transform placeForUi)
        {
            IInventoryView inventoryView = LoadInventoryView(placeForUi);
            IInventoryModel inventoryModel = _profilePlayer.Inventory;
            IItemsRepository itemsRepository = CreateItemsRepository();

            var inventoryController = new InventoryController(inventoryView, inventoryModel, itemsRepository);
            _subDisposables.Add(inventoryController);

            return inventoryController;
        }

        private IInventoryView LoadInventoryView(Transform placeForUi)
        {
            var path = new ResourcePath("Prefabs/Inventory/InventoryView");

            GameObject prefab = ResourcesLoader.LoadPrefab(path);
            GameObject objectView = Object.Instantiate(prefab, placeForUi);
            _subObjects.Add(objectView);

            return objectView.GetComponent<InventoryView>();
        }

        private IItemsRepository CreateItemsRepository()
        {
            var path = new ResourcePath("Configs/Inventory/ItemConfigDataSource");

            ItemConfig[] itemConfigs = ContentDataSourceLoader.LoadItemConfigs(path);
            var repository = new ItemsRepository(itemConfigs);
            _subDisposables.Add(repository);

            return repository;
        }
}
