

namespace Code.Input
{
    using System;
    using UnityEngine;

    public sealed class KeyboardInput: IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        public event Action OnPressSpace;

        public Vector3 GetInput()
        {
            if(Input.GetKeyUp(KeyCode.Space))
                OnPressSpace?.Invoke();

            return new(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical));
        }
    }
}