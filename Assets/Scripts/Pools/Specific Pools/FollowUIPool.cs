using Code.Pools;
using Code.ScriptableObjects;
using Code.UI;

public sealed class FollowUIPool : MultiPool<UIType, FollowUIView>
{
    private readonly FollowUIList _prefabs;
    private readonly UIPoolRoot _root;

    public FollowUIPool(FollowUIList uiList)
    {
        _prefabs = uiList;
        _root = ServiceLocator.Container.RequestFor<UIService>().GameCanvas.PoolRoot;
    }

    protected override FollowUIView GetPrefab(UIType type) => _prefabs.FindPrefab(type);

    public override void OnSpawned(FollowUIView result, ISpawnableType data = null) 
        => result.transform.SetParent(_root.Transform);
}