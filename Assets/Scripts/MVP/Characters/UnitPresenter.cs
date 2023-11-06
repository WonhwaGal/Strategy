using System;
using UnityEngine;
using Code.Strategy;

namespace Code.Units
{
    public class UnitPresenter : IDisposable  // WAS ABSTRACT
    {
        protected readonly UnitView _view;
        protected readonly UnitModel _model;
        protected readonly IUnitStrategy _strategy;
        private readonly CombatService _combatService;

        public UnitPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy)
        {
            _view = view;
            _model = model;
            _strategy = moveStrategy;
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
            _view.OnUpdate += Update;
            _combatService.OnLocatingCollider += CheckCollider;
        }

        protected void Update(float deltaTime)
        {
            _strategy.Execute(_model, _view, deltaTime);
        }

        protected void CheckCollider(GameObject obj)
        {
            if (_view.gameObject.Equals(obj))
                Debug.Log($"{_view.name} : it is me");
            else
                Debug.Log($"{_view.name} : it is NOT me");
        }

        public virtual void Dispose()
        {
            _view.OnUpdate -= Update;
            _combatService.OnLocatingCollider -= CheckCollider;
        }
    }
}