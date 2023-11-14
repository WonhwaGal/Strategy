using System;
using UnityEngine;

namespace Code.Construction
{
    public interface ICombatService : IService
    {
        event Action<GameMode> OnChangeGameStage;
        event Action<PrefabType, GameObject, IPresenter> OnRegisterForCombat;

        void SwitchMode(GameMode mode);
        void RegisterForCombat(PrefabType type, GameObject go, IPresenter presenter);
    }
}