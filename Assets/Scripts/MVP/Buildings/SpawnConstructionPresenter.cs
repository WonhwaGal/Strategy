using Code.Strategy;
using Code.UI;
using System;
using UnityEngine;

namespace Code.Construction
{
    public sealed class SpawnConstructionPresenter : ConstructionPresenter
    {
        public SpawnConstructionPresenter(ConstructionView view, ConstructionModel model, 
            IConstructionStrategy strategy) : base(view, model, strategy)
        {
        }

        public event Action<Transform[]> OnBuildingWithUnits;

        protected override void OnReactToUpgrade(BuildActionType action, bool selfBuild)
        {
            if (action != BuildActionType.Build || !selfBuild)
                return;

            if(_model.PrefabType == PrefabType.Barracks)
            {
                var view = (SpawnConstructionView)_view;
                OnBuildingWithUnits?.Invoke(view.AllySpawnPoints);
            }
        }
    }
}