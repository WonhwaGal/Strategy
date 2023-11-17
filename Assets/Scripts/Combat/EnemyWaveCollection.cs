using Code.Units;
using UnityEngine;

namespace Code.Combat
{
    public sealed class EnemyWaveCollection : BaseWaveCollection<IPresenter>
    {
        protected override void OnAddToCollection(IPresenter presenter) 
            => presenter.OnBeingKilled += OnEnemyKilled;

        protected override void OnRemoveFromCollection()
        {
            if (_participants.Count == 0)
                EndNight(isVictory: true);
        }

        private void OnEnemyKilled(IPresenter presenter, IUnitView view)
        {
            RemoveFromCollection(view.GameObject);
            presenter.OnBeingKilled -= OnEnemyKilled;
        }
    }
}