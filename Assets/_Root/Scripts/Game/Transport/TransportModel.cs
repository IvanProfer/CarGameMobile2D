using Features.Shed.Upgrade;

namespace Game.Transport
{
    internal class TransportModel : IUpgradable
    {
        private readonly float _defaultSpeed;
        private readonly float _defaultJumpHeight;

        public readonly TransportType Type;

        public float Speed { get; set; }
        public float JumpHeight { get; set; }


        public TransportModel(float speed, float jumpHeight, TransportType type)
        {
            _defaultSpeed = speed;
            _defaultJumpHeight = jumpHeight;

            Speed = speed;
            JumpHeight = jumpHeight;
            Type = type;
        }

        public void Restore()
        {
            Speed = _defaultSpeed;
            JumpHeight = _defaultJumpHeight;
        }
    }
}
