using _Project.Scripts.UI.CharacterMovementController;
using Fusion;

namespace _Project.Scripts.Infrastructure.Player
{
    public struct InputController : INetworkInput
    {
        public float HorizontalInput { get; private set; }
        public float VerticalInput { get; private set; }

        public void GetJoystick(IFixedJoystickController controller)
        {
            if (controller != null && controller.Joystick != null)
            {
                HorizontalInput = controller.Joystick.Horizontal;
                VerticalInput = controller.Joystick.Vertical;
            }
            else
            {
                HorizontalInput = 0f;
                VerticalInput = 0f;
            }
        }
    }
}