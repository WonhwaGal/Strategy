using Code.Units;
using UnityEngine;

namespace Code.Combat
{
    public sealed class BuildingWaveCollection : BaseWaveCollection<IPresenter>
    {
        public IPresenter Castle;

        public void AddCastle(IPresenter presenter)
        {
            Debug.Log("added castle");
            Castle = presenter;
            presenter.OnBeingKilled += OnCastleDestroyed;
        }

        public IPresenter FindCastle() => Castle;

        private void OnCastleDestroyed(IPresenter presenter, IUnitView view)
        {
            EndNight(isVictory: false);
            presenter.OnBeingKilled -= OnCastleDestroyed;
        }
    }
}