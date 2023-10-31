using Code.Units;

namespace Code.Input
{

    using UnityEngine;

    public class KeyboardInput: IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        private Vector3 _input;

        public Vector3 GetInput() => new (Input.GetAxis(Horizontal), 0, Input.GetAxis(Vertical));
    }
}