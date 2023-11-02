using System;
using UnityEngine;

namespace Code.Input
{
    public interface IInputService : IService
    {
        event Action OnPressSpace;
        Vector3 GetInput();
    }
}