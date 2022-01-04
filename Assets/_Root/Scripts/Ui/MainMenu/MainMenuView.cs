using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Ui
{
    public class MainMenuView : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string _productId;

        [Header("Buttons")]
        [SerializeField] private Button _buttonStart;
        [SerializeField] private Button _buttonSettings;
        [SerializeField] private Button _buttonShed;
        [SerializeField] private Button _buttonReward;
        [SerializeField] private Button _buttonBuy;
        [SerializeField] private Button _buttonDailyReward;
        [SerializeField] private Button _exitGame;


        public void Init(UnityAction startGame, UnityAction settings, UnityAction shed,
            UnityAction reward, UnityAction<string> buy, UnityAction dailyReward, UnityAction exitGame)
        {
            _buttonStart.onClick.AddListener(startGame);
            _buttonSettings.onClick.AddListener(settings);
            _buttonShed.onClick.AddListener(shed);
            _buttonReward.onClick.AddListener(reward);
            _buttonBuy.onClick.AddListener(() => buy(_productId));
            _buttonDailyReward.onClick.AddListener(dailyReward);
            _exitGame.onClick.AddListener(exitGame);
        }

        public void OnDestroy()
        {
            _buttonStart.onClick.RemoveAllListeners();
            _buttonSettings.onClick.RemoveAllListeners();
            _buttonShed.onClick.RemoveAllListeners();
            _buttonReward.onClick.RemoveAllListeners();
            _buttonBuy.onClick.RemoveAllListeners();
            _buttonDailyReward.onClick.RemoveAllListeners();
            _exitGame.onClick.RemoveAllListeners();
        }
    }
}
