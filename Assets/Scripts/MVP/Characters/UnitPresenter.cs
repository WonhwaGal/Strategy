using Code.Strategy;
using System;

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
        }

        public virtual void Dispose()
        {
        }
    }
}