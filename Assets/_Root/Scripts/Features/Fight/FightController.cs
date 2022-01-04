using System;
using Profile;
using TMPro;
using Tool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.Fight
{
    internal class FightController : BaseController
    {
        private readonly ResourcePath _resourcePath = new ResourcePath("Prefabs/Fight/FightView");
        private readonly ProfilePlayer _profilePlayer;
        private readonly FightView _view;
        private readonly Enemy _enemy;

        private DataPlayer _money;
        private DataPlayer _heath;
        private DataPlayer _power;
        private DataPlayer _crime;

        private int _allCountMoneyPlayer;
        private int _allCountHealthPlayer;
        private int _allCountPowerPlayer;
        private int _allCountCrimePlayer;


        public FightController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;
            _view = LoadView(placeForUi);

            _enemy = new Enemy("Enemy Flappy");

            _money = CreateDataPlayer(DataType.Money);
            _heath = CreateDataPlayer(DataType.Health);
            _power = CreateDataPlayer(DataType.Power);
            _crime = CreateDataPlayer(DataType.Crime);

            Subscribe();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            DisposeDataPlayer(ref _money);
            DisposeDataPlayer(ref _heath);
            DisposeDataPlayer(ref _power);
            DisposeDataPlayer(ref _crime);

            Unsubscribe();
        }


        private FightView LoadView(Transform placeForUi)
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_resourcePath);
            GameObject objectView = Object.Instantiate(prefab, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<FightView>();
        }

        private DataPlayer CreateDataPlayer(DataType dataType)
        {
            DataPlayer dataPlayer = new DataPlayer(dataType);
            dataPlayer.Attach(_enemy);

            return dataPlayer;
        }

        private void DisposeDataPlayer(ref DataPlayer dataPlayer)
        {
            dataPlayer.Detach(_enemy);
            dataPlayer = null;
        }


        private void Subscribe()
        {
            _view.AddMoneyButton.onClick.AddListener(IncreaseMoney);
            _view.MinusMoneyButton.onClick.AddListener(DecreaseMoney);

            _view.AddHealthButton.onClick.AddListener(IncreaseHealth);
            _view.MinusHealthButton.onClick.AddListener(DecreaseHealth);

            _view.AddPowerButton.onClick.AddListener(IncreasePower);
            _view.MinusPowerButton.onClick.AddListener(DecreasePower);

            _view.AddCrimeButton.onClick.AddListener(IncreaseCrime);
            _view.MinusCrimeButton.onClick.AddListener(DecreaseCrime);

            _view.FightButton.onClick.AddListener(Fight);
            _view.EscapeButton.onClick.AddListener(Escape);
        }

        private void Unsubscribe()
        {
            _view.AddMoneyButton.onClick.RemoveAllListeners();
            _view.MinusMoneyButton.onClick.RemoveAllListeners();

            _view.AddHealthButton.onClick.RemoveAllListeners();
            _view.MinusHealthButton.onClick.RemoveAllListeners();

            _view.AddPowerButton.onClick.RemoveAllListeners();
            _view.MinusPowerButton.onClick.RemoveAllListeners();

            _view.AddCrimeButton.onClick.RemoveAllListeners();
            _view.MinusCrimeButton.onClick.RemoveAllListeners();

            _view.FightButton.onClick.RemoveAllListeners();
            _view.EscapeButton.onClick.RemoveAllListeners();
        }


        private void IncreaseMoney() => IncreaseValue(ref _allCountMoneyPlayer, DataType.Money);
        private void DecreaseMoney() => DecreaseValue(ref _allCountMoneyPlayer, DataType.Money);

        private void IncreaseHealth() => IncreaseValue(ref _allCountHealthPlayer, DataType.Health);
        private void DecreaseHealth() => DecreaseValue(ref _allCountHealthPlayer, DataType.Health);

        private void IncreasePower() => IncreaseValue(ref _allCountPowerPlayer, DataType.Power);
        private void DecreasePower() => DecreaseValue(ref _allCountPowerPlayer, DataType.Power);

        private void IncreaseCrime() => IncreaseValue(ref _allCountCrimePlayer, DataType.Crime);
        private void DecreaseCrime() => DecreaseValue(ref _allCountCrimePlayer, DataType.Crime);

        private void IncreaseValue(ref int value, DataType dataType) => AddToValue(ref value, 1, dataType);
        private void DecreaseValue(ref int value, DataType dataType) => AddToValue(ref value, -1, dataType);

        private void AddToValue(ref int value, int addition, DataType dataType)
        {
            value += addition;
            UpdateEscapeButtonVisibility();
            ChangeDataWindow(value, dataType);
        }


        private void ChangeDataWindow(int countChangeData, DataType dataType)
        {
            DataPlayer dataPlayer = GetDataPlayer(dataType);
            TMP_Text textComponent = GetTextComponent(dataType);
            string text = $"Player {dataType:F} {countChangeData}";

            dataPlayer.Value = countChangeData;
            textComponent.text = text;

            int enemyPower = _enemy.CalcPower();
            _view.CountPowerEnemyText.text = $"Enemy Power {enemyPower}";
        }

        private TMP_Text GetTextComponent(DataType dataType) =>
            dataType switch
            {
                DataType.Money => _view.CountMoneyText,
                DataType.Health => _view.CountHealthText,
                DataType.Power => _view.CountPowerText,
                DataType.Crime => _view.CountCrimeText,
                _ => throw new ArgumentException($"Wrong {nameof(DataType)}")
            };

        private DataPlayer GetDataPlayer(DataType dataType) =>
            dataType switch
            {
                DataType.Money => _money,
                DataType.Health => _heath,
                DataType.Power => _power,
                DataType.Crime => _crime,
                _ => throw new ArgumentException($"Wrong {nameof(DataType)}")
            };

        private void UpdateEscapeButtonVisibility()
        {
            const int minCrimeToUse = 0;
            const int maxCrimeToUse = 2;
            const int minCrimeToShow = 0;
            const int maxCrimeToShow = 5;

            bool canUse = minCrimeToUse <= _allCountCrimePlayer && _allCountCrimePlayer <= maxCrimeToUse;
            bool canShow = minCrimeToShow <= _allCountCrimePlayer && _allCountCrimePlayer <= maxCrimeToShow;

            _view.EscapeButton.interactable = canUse;
            _view.EscapeButton.gameObject.SetActive(canShow);
        }


        private void Fight()
        {
            int enemyPower = _enemy.CalcPower();
            bool isVictory = _allCountPowerPlayer >= enemyPower;

            string color = isVictory ? "#07FF00" : "#FF0000";
            string message = isVictory ? "Win" : "Lose";

            Debug.Log($"<color={color}>{message}!!!</color>");

            Close();
        }

        private void Escape()
        {
            string color = "#FFB202";
            string message = "Escaped";

            Debug.Log($"<color={color}>{message}!!!</color>");

            Close();
        }

        private void Close() => _profilePlayer.CurrentState.Value = GameState.Game;
    }
}
