using System;

namespace Code.Construction
{
    public interface IReactToDaytimeSwitch : IService
    {
        event Action<GameMode> OnGameModeChange;

        void SwitchMode(GameMode mode);
    }
}