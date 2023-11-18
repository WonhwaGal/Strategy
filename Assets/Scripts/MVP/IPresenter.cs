using System;
using Code.Units;

public interface IPresenter: IDisposable
{
    event Action<IPresenter, IUnitView> OnBeingKilled;
    void OnGameModeChange(GameMode mode);
    void ReceiveDamage(int damage);
}