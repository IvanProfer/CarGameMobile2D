using UnityEngine;

namespace Features.Fight
{
    internal interface IEnemy
    {
        void Update(DataPlayer dataPlayer);
    }

    internal class Enemy : IEnemy
    {
        private const float KMoney = 5f;
        private const float KHealth = 7f;
        private const float KPower = 5f;
        private const float KCrime = 10f;
        private const float KSummary = 0.2f;
        private const float MaxHealthPlayer = 20;

        private readonly string _name;

        private int _moneyPlayer;
        private int _healthPlayer;
        private int _powerPlayer;
        private int _crimePlayer;


        public Enemy(string name) =>
            _name = name;


        public void Update(DataPlayer dataPlayer)
        {
            switch (dataPlayer.DataType)
            {
                case DataType.Money:
                    _moneyPlayer = dataPlayer.Value;
                    break;

                case DataType.Health:
                    _healthPlayer = dataPlayer.Value;
                    break;

                case DataType.Power:
                    _powerPlayer = dataPlayer.Value;
                    break;

                case DataType.Crime:
                    _crimePlayer = dataPlayer.Value;
                    break;
            }

            Debug.Log($"Notified {_name} change to {dataPlayer}");
        }

        public int CalcPower()
        {
            float moneyRatio = _moneyPlayer / KMoney;
            float healthRatio = _healthPlayer / KHealth;
            float powerRatio = _powerPlayer / KPower;
            float crimeRatio = _crimePlayer / KCrime;
            float summaryRatio = moneyRatio + healthRatio + powerRatio + crimeRatio;

            return (int)(summaryRatio * KSummary * MaxHealthPlayer);
        }
    }
}

