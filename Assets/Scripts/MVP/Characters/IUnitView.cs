﻿using System;

namespace Code.Units
{
    public interface IUnitView
    {
        event Action<float> OnUpdate;
    }
}