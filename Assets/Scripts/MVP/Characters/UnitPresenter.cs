using System;
using UnityEngine;
using Code.Strategy;

namespace Code.Units
{
    public abstract class UnitPresenter : IDisposable
    {
        protected readonly UnitView _view;
        protected readonly UnitModel _model;
        protected readonly IUnitStrategy _strategy;

        public UnitPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy)
        {
            _view = view;
            _model = model;
            _strategy = moveStrategy;
            _view.OnUpdate += Update;
        }

        protected void Update(float deltaTime)
        {
            _strategy.Execute(_model, _view, deltaTime);
        }

        public virtual void Dispose()
        {
            _view.OnUpdate -= Update;
        }
    }
}