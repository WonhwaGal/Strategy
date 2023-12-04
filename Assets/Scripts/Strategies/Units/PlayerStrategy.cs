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
        private UnitAnimator _animator;
        private Vector3 _targetPos;
        private float _currentInterval = 0;
        private RechargePanel _rechargePanel;
        protected bool _stopActions;
        private bool _activateAbility;
        private bool _isNight;

        public PlayerStrategy()
        {
            _input = ServiceLocator.Container.RequestFor<IInputService>();
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
        }

        public void Init(IUnitPresenter presenter)
        {
            presenter.View.NavAgent.speed = presenter.Model.Speed;
            _animator = new UnitAnimator(presenter.View.Animator);
            presenter.Model.OnKilled += OnDying;
        }

        public void Execute(IUnitPresenter presenter, float delta)
        {
            if (_stopActions)
                return;

            Move(presenter.Model, presenter.View);
            if (_isNight)
            {
                CountDown(presenter.Model, delta);
                UseAbility(presenter);
            }
        }

        public void SwitchStrategy(IUnitPresenter presenter, GameMode mode)
        {
            if (mode == GameMode.IsNight)
            {
                PrepareForCombat(presenter);
                return;
            }
            else
            {
                var area = ((PlayerUnit)presenter.View).ControlUnitArea;
                area.SetActive(mode == GameMode.IsUnitControl);
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

            _animator.AnimateMovement(view.NavAgent.hasPath);
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

        private void ActivateAbility() => _activateAbility = _isNight;

        private void UseAbility(IUnitPresenter presenter)
        {
            if (!_rechargePanel.IsReady || !_activateAbility)
            {
                _activateAbility = false;
                return;
            }

            _animator.AnimateAreaAttack(((PlayerUnit)presenter.View).AreaAttack);
            Attack(presenter.Model, AttackType.AreaSword);
            _rechargePanel.IsReady = false;
        }

        private void Attack(IModel model, AttackType attack)
        {
            if(_combatService.CheckForTargets(model, attack))
                _animator.AnimateAttack();
        }

        private void PrepareForCombat(IUnitPresenter presenter)
        {
            _isNight = true;
            WaveLocator.ParticipateInCombat(PrefabType.Player, presenter.View.gameObject, presenter);
            _rechargePanel = ((PlayerPresenter)presenter).SetUpRechargePanel();
            _input.OnPressSpace += ActivateAbility;
        }

        private void OnDying()
        {
            _stopActions = true;
            _animator.AnimateDeath();
        }

        public void Dispose() => _input.OnPressSpace -= ActivateAbility;
    }
}