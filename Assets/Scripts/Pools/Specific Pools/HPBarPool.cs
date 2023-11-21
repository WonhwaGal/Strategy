using UnityEngine;
using Code.Pools;
using Code.ScriptableObjects;
using Code.Units;
using Code.UI;

public sealed class HPBarPool : MultiPool<UIType, HPBar>
{
    private readonly HPBarList _prefabs;
    private readonly UIPoolRoot _root;

    public HPBarPool(HPBarList uiList, UIPoolRoot root)
    {
        _prefabs = uiList;
        _root = root;
    }

    protected override HPBar GetPrefab(UIType type) => _prefabs.FindPrefab(type);

    public override void OnSpawned(HPBar result, ISpawnableType data = null) 
        => result.transform.SetParent(_root.Transform);
}