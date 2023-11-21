
namespace Code.Input
{
    using System;
    using UnityEngine;

    public sealed class KeyboardInput: IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";

        public event Action OnPressSpace;
        public event Action<bool> OnPressCtrl;

        public Vector3 GetInput()
        {
            if(Input.GetKeyUp(KeyCode.Space))
                OnPressSpace?.Invoke();

            CheckCtrl();

            return new(Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical));
        }

        private void CheckCtrl()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
                OnPressCtrl?.Invoke(true);
            if(Input.GetKeyUp(KeyCode.LeftControl))
                OnPressCtrl?.Invoke(false);
        }
    }
}