using System;
using System.Collections.Generic;
using UnityEngine;
using Code.Combat;
using Code.Tools;

public static class WaveLocator
{
    private readonly static Dictionary<string, BaseWaveCollection<IPresenter>> _collections = new();

    public static event Action<bool> OnWaveEnd;

    public static BaseWaveCollection<IPresenter> GetCollection(string key)
    {
        if(_collections.ContainsKey(key))
            return _collections[key];
        return null;
    }

    public static void ParticipateInCombat(PrefabType type, GameObject go, IPresenter presenter)
    {
        var currentCollection = _collections[ReceiveCollectionKey(type)];

        if (type != PrefabType.Castle)
            currentCollection.AddToCollection(go, presenter);
        else
            ((BuildingWaveCollection)currentCollection).AddCastle(presenter);
    }

    private static string ReceiveCollectionKey(PrefabType type)
    {
        string key;

        if ((int)type >= Constants.EnemyThreshold)
            key = CheckCollection(Constants.Enemies, new EnemyWaveCollection());
        else if ((int)type < Constants.PlayerThreshold)
            key = CheckCollection(Constants.Buildings, new BuildingWaveCollection());
        else
            key = CheckCollection(Constants.Allies, new AllyWaveCollection());

        return key;
    }

    private static string CheckCollection(string key, BaseWaveCollection<IPresenter> collection)
    {
        if (!_collections.ContainsKey(key))
        {
            _collections.Add(key, collection);
            _collections[key].OnWaveOver += EndWave;
        }
        return key;
    }

    private static void EndWave(bool isVictory)
    {
        ClearCollections();
        OnWaveEnd?.Invoke(isVictory);
    }

    public static void Dispose()
    {
        ClearCollections();
        OnWaveEnd = null;
    }

    private static void ClearCollections()
    {
        foreach (var collection in _collections)
            collection.Value.Dispose();
        _collections.Clear();
    }
}