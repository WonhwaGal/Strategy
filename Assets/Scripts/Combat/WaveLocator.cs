using System;
using System.Collections.Generic;
using UnityEngine;
using Code.Combat;
using Code.Tools;

public static class WaveLocator
{
    private readonly static Dictionary<string, BaseWaveCollection<IPresenter>> _collections = new();

    public static BaseWaveCollection<IPresenter> GetCollection(string key)
    {
        if(_collections.ContainsKey(key))
            return _collections[key];
        return null;
    }

    public static void SubscribeToWaveOver(Action<bool> method)
    {
        foreach (var collection in _collections.Values)
            collection.OnWaveOver += method;
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
        string collectionKey;

        if ((int)type >= Constants.EnemyThreshold)
        {
            collectionKey = Constants.Enemies;
            if (!_collections.ContainsKey(collectionKey))
                _collections.Add(collectionKey, new EnemyWaveCollection());
        }
        else if ((int)type < Constants.PlayerThreshold)
        {
            collectionKey = Constants.Buildings;
            if (!_collections.ContainsKey(collectionKey))
                _collections.Add(collectionKey, new BuildingWaveCollection());
        }
        else
        {
            collectionKey = Constants.Allies;
            if (!_collections.ContainsKey(collectionKey))
                _collections.Add(collectionKey, new AllyWaveCollection());
        }

        return collectionKey;
    }

    public static void Dispose()
    {
        foreach (var collection in _collections)
            collection.Value.Dispose();
        _collections.Clear();
    }
}