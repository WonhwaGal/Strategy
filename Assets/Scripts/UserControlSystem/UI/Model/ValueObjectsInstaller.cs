using Zenject;
using UserControlSystem;

public class ValueObjectsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<Vector3Value>().AsSingle().NonLazy();
        Container.Bind<AttackableValue>().AsSingle().NonLazy();
        Container.Bind<SelectableValue>().AsSingle().NonLazy();
    }
}
