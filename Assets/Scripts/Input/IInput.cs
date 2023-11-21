using System;
using UnityEngine;

namespace Code.Input
{
    public interface IInputService : IService
    {
        event Action OnPressSpace;
        event Action<bool> OnPressCtrl;
        Vector3 GetInput();
    }
}