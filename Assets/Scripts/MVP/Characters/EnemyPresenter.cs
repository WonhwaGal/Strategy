using Code.Strategy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Units
{
    public class EnemyPresenter : UnitPresenter
    {
        private EnemyView _enemyView;
        private readonly CombatService _combatService;

        public EnemyPresenter(UnitView view, UnitModel model, IUnitStrategy moveStrategy) : 
            base(view, model, moveStrategy)
        {
            _combatService = ServiceLocator.Container.RequestFor<CombatService>();
            _combatService.OnLocatingCollider += CheckCollider;
            SetEnemy(view);
        }

        private void SetEnemy(UnitView view)
        {
            _enemyView = (EnemyView)view;
            _enemyView.MaxSliderValue = _model.HP;
            _enemyView.ChangeHPSlider(_model.HP);
            _enemyView.OnReceiveDamage += ReceiveDamage;
        }

        private void CheckCollider(GameObject obj)
        {
            if (_view.gameObject.Equals(obj))
                ReceiveDamage();
        }

        private void ReceiveDamage()
        {
            _model.HP -= _model.Damage;
            _enemyView.ChangeHPSlider(_model.HP);
        }

        public override void Dispose()
        {
            base.Dispose();
            _combatService.OnLocatingCollider -= CheckCollider;
            _enemyView.OnReceiveDamage -= ReceiveDamage;
        }
    }
}