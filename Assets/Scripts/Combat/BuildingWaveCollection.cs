using Code.Units;
using UnityEngine;

namespace Code.Combat
{
    public sealed class BuildingWaveCollection : BaseWaveCollection<IPresenter>
    {
        public IPresenter Castle;

        public void AddCastle(IPresenter presenter)
        {
            Castle = presenter;
            presenter.OnBeingKilled += OnCastleDestroyed;
        }

        public override IPresenter FindParticipant(GameObject gameObj)
        {
            if (_participants.ContainsKey(gameObj))
                return base.FindParticipant(gameObj);
            else
                return FindCastle();
        }

        public IPresenter FindCastle() => Castle;

        private void OnCastleDestroyed(IPresenter presenter, IUnitView view, bool destroyView)
        {
            EndNight(isVictory: false);
            presenter.OnBeingKilled -= OnCastleDestroyed;
        }
    }
}