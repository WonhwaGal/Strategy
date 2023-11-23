using UnityEngine;
using Code.Input;
using Code.Units;
using Code.Combat;

namespace Code.Strategy
{
    public class PlayerStrategy : IUnitStrategy
    {
        private readonly IInputService _input;
        private readonly CombatService _combatService;
        private Vector3 _targetPos;
        private float _currentInterval = 0;
        private bool _abilityActive;
        private bool _isNight;

        public PlayerStrategy()
        {
            _input = ServiceLocator.Container.RequestFor<IInputService>();
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
            _input.OnPressSpace += ActivateAbility;
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
             _isNight = mode == GameMode.IsNight;
            if(mode == GameMode.IsNight)
            {
                WaveLocator.ParticipateInCombat(PrefabType.Player, presenter.View.gameObject, presenter);
                presenter.SetUpHPBar(UIType.PlayerHP);
            }
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

        private void ActivateAbility() => _abilityActive = _isNight;

        private void UseAbility(UnitModel model)
        {
            if (!_abilityActive)
                return;

            Attack(model, AttackType.AreaSword);
            _abilityActive = false;
        }

        private void Attack(IModel model, AttackType attack) =>
            _combatService.CheckForTargets(model, attack);

        public void Dispose() => _input.OnPressSpace -= ActivateAbility;
    }
}