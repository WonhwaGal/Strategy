using System;
using Code.Combat;
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

        public UnitPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy)
        {
            _view = view;
            _model = model;
            _strategy = moveStrategy;

            _model.OnKilled += Die;
            _view.OnViewDestroyed += Die;
            _view.OnUpdate += Update;
            _view.OnReceiveDamage += ReceiveDamage;
            ServiceLocator.Container.RequestFor<LevelService>()
                .OnChangingGameMode += OnGameModeChange;
        }
        UnitView IUnitPresenter.View => _view;
        UnitModel IUnitPresenter.Model => _model;
        IStrategy IUnitPresenter.Strategy { get { return _strategy; } set { _strategy = (IUnitStrategy)value; } }
        HPBar IUnitPresenter.HPBar => _hpBar;

        public event Action<IPresenter, IUnitView, bool> OnBeingKilled;

        public virtual void PlaceUnit(Vector3 pos) => _view.transform.position = pos;

        public void OnGameModeChange(GameMode mode) => _strategy.SwitchStrategy(this, mode);

        public void ReceiveDamage(int damage)
        {
            _hpBar.gameObject.SetActive(true);
            _hpBar.SetHPValue(_model.HP -= damage);
        }

        public void SetUpHPBar(UIType uiType)
        {
            if (_hpBar == null)
            {
                var hpPool = ServiceLocator.Container.RequestFor<FollowUIPool>();
                _hpBar = (HPBar)hpPool.Spawn(uiType);
                hpPool.OnSpawned(_hpBar);
            }
            _hpBar.SetUpSlider(_model.HP, _model.Transform);
        }

        protected void Update(float deltaTime) => _strategy.Execute(this, deltaTime);

        protected void Die(bool destroyView) => OnBeingKilled?.Invoke(this, _view, destroyView);

        public virtual void Dispose()
        {
            if (_hpBar != null)
                _hpBar.Despawn();

            _model.OnKilled -= Die;
            _view.OnViewDestroyed -= Die;
            _view.OnUpdate -= Update;
            _view.OnReceiveDamage -= ReceiveDamage;
            _model.Dispose();
            ServiceLocator.Container.RequestFor<LevelService>()
                .OnChangingGameMode -= OnGameModeChange;
            GC.SuppressFinalize(this);
        }
    }
}