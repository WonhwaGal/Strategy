using System;
using Code.Strategy;
using UnityEngine;

namespace Code.Units
{
    public abstract class UnitPresenter : IDisposable
    {
        protected readonly UnitView _view;
        protected readonly UnitModel _model;
        protected readonly IUnitStrategy _strategy;
        protected readonly HPBar _hpBar;

        public event Action<UnitPresenter> OnBeingKilled;

        public UnitPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy)
        {
            _view = view;
            _model = model;
            _strategy = moveStrategy;
            _hpBar = view.HPBar;
            _hpBar.MaxSliderValue = _model.HP;
            _hpBar.ChangeHPSlider(_model.HP);
            _view.OnUpdate += Update;
            _view.OnReceiveDamage += ReceiveDamage;
            _model.OnDestroyed += Die;
        }

        protected void Update(float deltaTime)
        {
            _strategy.Execute(_model, _view, deltaTime);
        }

        protected void ReceiveDamage()
        {
            _model.HP -= _model.Damage;
            _hpBar.ChangeHPSlider(_model.HP);
        }

        protected void Die() => OnBeingKilled?.Invoke(this);

        public virtual void Dispose()
        {
            _view.OnUpdate -= Update;
            _view.OnReceiveDamage -= ReceiveDamage;
            _model.OnDestroyed -= Die;
        }
    }
}