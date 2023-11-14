﻿using Code.Units;

namespace Code.Combat
{
    public sealed class AlliesWaveCollection : BasePresentorWaveCollection<IPresenter>
    {
        public IPresenter Castle;

        public void AddCastle(IPresenter presenter)
        {
            Castle = presenter;
            presenter.OnBeingKilled += OnCastleDestroyed;
        }

        private void OnCastleDestroyed(IPresenter presenter, IUnitView view)
        {
            EndNight(isVictory: false);
            presenter.OnBeingKilled -= OnCastleDestroyed;
        }
    }
}