using Fusion;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.Player
{
    public struct InputController : INetworkInput
    {
        // private const string HORIZONTAL = "Horizontal";
        // private const string VERTICAL = "Vertical";

        public float HorizontalInput;
        public float VerticalInput;
        //
        // public void UpdateHorizontalInput()
        // {
        //     HorizontalInput = Input.GetAxis(HORIZONTAL);
        // }
        //
        // public void UpdateVerticalInput()
        // {
        //     VerticalInput = Input.GetAxis(VERTICAL);
        // }
    }
}