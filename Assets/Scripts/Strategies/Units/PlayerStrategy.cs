using System;
using UnityEngine;
using Code.Input;
using Code.Units;
using Code.Combat;
using Code.UI;

namespace Code.Strategy
{
    public class PlayerStrategy : IUnitStrategy, IDisposable
    {
        private readonly IInputService _input;
        private readonly CombatService _combatService;
        private Vector3 _targetPos;
        private float _currentInterval = 0;
        private RechargePanel _rechargePanel;
        private bool _activateAbility;
        private bool _isNight;

        public PlayerStrategy()
        {
            _input = ServiceLocator.Container.RequestFor<IInputService>();
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
        }

        public void Init(IUnitPresenter presenter) { }

        public void Execute(IUnitPresenter presenter, float delta)
        {
            Move(presenter.Model, presenter.View);
            if (_isNight)
            {
                CountDown(presenter.Model, delta);
                UseAbility(presenter.Model);
            }
        }

        public void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsNight)
            {
                PrepareForCombat(presenter);
                return;
            }

            if (mode == GameMode.IsUnitControl)
            {
                var area = ((PlayerUnit)presenter.View).ControlUnitArea;
                area.SetActive(!area.activeSelf);
            }
            if (presenter.HPBar != null)
                presenter.HPBar.gameObject.SetActive(false);
            _isNight = false;
            _input.OnPressSpace -= ActivateAbility;
        }

        private void Move(UnitModel model, UnitView view)
        {
            _targetPos = _input.GetInput() * model.Speed + view.transform.position;
            if (model.HP > 0)
                view.NavAgent.SetDestination(_targetPos);
        }

        private void CountDown(UnitModel model, float delta)
        {
            _currentInterval += delta;
            if (_currentInterval >= model.AttackInterval)
            {
                Attack(model, AttackType.Arrow);
                _currentInterval = 0;
            }
        }

        private void ActivateAbility()
        {
            if (_isNight)
                _activateAbility = true;
        }

        private void UseAbility(UnitModel model)
        {
            if (!_rechargePanel.IsReady || !_activateAbility)
            {
                _activateAbility = false;
                return;
            }

            Debug.Log("sword attack");
            Attack(model, AttackType.AreaSword);
            _rechargePanel.IsReady = false;
        }

        private void Attack(IModel model, AttackType attack) =>
            _combatService.CheckForTargets(model, attack);

        private void PrepareForCombat(IUnitPresenter presenter)
        {
            _isNight = true;
            WaveLocator.ParticipateInCombat(PrefabType.Player, presenter.View.gameObject, presenter);
            presenter.SetUpHPBar(UIType.PlayerHP);
            _rechargePanel = ((PlayerPresenter)presenter).SetUpRechargePanel();
            _input.OnPressSpace += ActivateAbility;
        }

        public void Dispose() => _input.OnPressSpace -= ActivateAbility;
    }
}