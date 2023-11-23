using System;
using Code.Units;

public interface IPresenter: IDisposable
{
    event Action<IPresenter, IUnitView, bool> OnBeingKilled;
    void OnGameModeChange(GameMode mode);
    void ReceiveDamage(int damage);
}