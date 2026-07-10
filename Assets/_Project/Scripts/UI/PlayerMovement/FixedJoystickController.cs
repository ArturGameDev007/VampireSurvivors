using UnityEngine;

namespace _Project.Scripts.UI.PlayerMovement
{
    public class FixedJoystickController : MonoBehaviour, IFixedJoystickController
    {
        [field: SerializeField] public FixedJoystick Joystick { get; private set; }
    }
}