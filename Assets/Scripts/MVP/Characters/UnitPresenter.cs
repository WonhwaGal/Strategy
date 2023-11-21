using System;
using Code.Strategy;
using Code.UI;
using UnityEngine;

namespace Code.Units
{
    public abstract class UnitPresenter : IUnitPresenter
    {
        protected readonly UnitView _view;
        protected readonly UnitModel _model;
        protected IUnitStrategy _strategy;
        protected HPBar _hpBar;

        public UnitPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy, HPBar hpBar)
        {
            _view = view;
            _model = model;
            _strategy = moveStrategy;
            _hpBar = SetUpHPBar(hpBar);

            _model.OnKilled += Die;
            _view.OnViewDestroyed += Die;
            _view.OnUpdate += Update;
            _view.OnReceiveDamage += ReceiveDamage;
        }
        UnitView IUnitPresenter.View => _view;
        UnitModel IUnitPresenter.Model => _model;
        IStrategy IUnitPresenter.Strategy { get { return _strategy; } set { _strategy = (IUnitStrategy)value; } }


        public event Action<IPresenter, IUnitView> OnBeingKilled;

        public void PlaceUnit(Vector3 pos) => _view.transform.position = pos;
        public HPBar SetUpHPBar(HPBar hpBar) => hpBar.SetUpSlider(_model.HP, _model.Transform);
        public void OnGameModeChange(GameMode mode) => _strategy.SwitchStrategy(this, mode);

        public void ReceiveDamage(int damage)
        {
            _hpBar.gameObject.SetActive(true);
            _hpBar.SetHPValue(_model.HP -= damage);
        }

        protected void Update(float deltaTime) => _strategy.Execute(this, deltaTime);

        protected void Die() => OnBeingKilled?.Invoke(this, _view);

        public virtual void Dispose()
        {
            _hpBar.Despawn();
            _model.OnKilled -= Die;
            _view.OnViewDestroyed -= Die;
            _view.OnUpdate -= Update;
            _view.OnReceiveDamage -= ReceiveDamage;
            _model.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}