using System;
using Code.Strategy;
using UnityEngine;

namespace Code.Units
{
    public abstract class UnitPresenter : IUnitPresenter
    {
        protected readonly UnitView _view;
        protected readonly UnitModel _model;
        protected IUnitStrategy _strategy;
        protected readonly HPBar _hpBar;

        public UnitPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy)
        {
            _view = view;
            _model = model;
            _strategy = moveStrategy;
            _hpBar = view.HPBar;
            _hpBar.SetMaxValue(_model.HP);
            _model.OnKilled += Die;
            _view.OnUpdate += Update;
            _view.OnReceiveDamage += ReceiveDamage;
            _view.OnViewDestroyed += Destroy;
        }
        UnitView IUnitPresenter.View => _view;
        UnitModel IUnitPresenter.Model => _model;
        IStrategy IUnitPresenter.Strategy { get { return _strategy; } set { _strategy = (IUnitStrategy)value; } }

        public event Action<IPresenter, IUnitView> OnBeingKilled;
        public event Action<UnitPresenter> OnRequestDestroy;

        public void PlaceUnit(Vector3 pos) => _view.transform.position = pos;
        public void OnGameModeChange(GameMode mode) => _strategy.SwitchStrategy(this, mode);

        public void ReceiveDamage(int damage) 
            => _hpBar.ChangeHPSlider(_model.HP -= damage);

        protected void Update(float deltaTime) 
            => _strategy.Execute(this, deltaTime);

        protected void Die()
        {
            OnBeingKilled?.Invoke(this, _view);
            _model.HP = (int)_hpBar.MaxSliderValue;
        }

        private void Destroy() => OnRequestDestroy?.Invoke(this);

        public virtual void Dispose()
        {
            _view.OnUpdate -= Update;
            _model.OnKilled -= Die;
            _view.OnReceiveDamage -= ReceiveDamage;
            _view.OnViewDestroyed += Destroy;
            GC.SuppressFinalize(this);
        }
    }
}