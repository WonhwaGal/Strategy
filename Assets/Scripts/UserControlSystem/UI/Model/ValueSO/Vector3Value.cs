using System;
using UnityEngine;

namespace UserControlSystem
{
    public class Vector3Value : ValueObject<Vector3>
    {
        public Action<Vector3> OnNewValue;
        public override void OnSetValue(Vector3 value)
        {
            OnNewValue?.Invoke(value);
        }
    }
}