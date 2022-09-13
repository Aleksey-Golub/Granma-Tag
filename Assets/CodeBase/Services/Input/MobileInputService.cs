using UnityEngine;

namespace Assets.CodeBase.Services.Input
{
    public class MobileInputService : IInputService
    {
        private readonly Joystick _joystick;

        public MobileInputService(Joystick joystick)
        {
            _joystick = joystick;
        }

        public Vector2 RawAxis => new Vector2(_joystick.Horizontal, _joystick.Vertical);
    }
}