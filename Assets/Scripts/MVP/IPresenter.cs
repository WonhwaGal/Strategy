using System;
using Code.Units;

public interface IPresenter: IDisposable
{
    event Action<IPresenter, IUnitView> OnBeingKilled;
    void ChangeStage(GameMode stage);
    void ReceiveDamage(int damage);
}