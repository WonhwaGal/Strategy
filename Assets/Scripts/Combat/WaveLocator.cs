using System;
using System.Collections.Generic;
using UnityEngine;
using Code.Combat;

public static class WaveLocator
{
    private readonly static Dictionary<string, BaseWaveCollection<IPresenter>> _collections = new();
    private const int EnemyThreshold = 30;
    private const int PlayerThreshold = 25;
    private const string Enemy = "Enemy";
    private const string Building = "Building";
    private const string Ally = "Ally";

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

        if ((int)type >= EnemyThreshold)
        {
            collectionKey = Enemy;
            if (!_collections.ContainsKey(collectionKey))
                _collections.Add(collectionKey, new EnemyWaveCollection());
        }
        else if ((int)type < PlayerThreshold)
        {
            collectionKey = Building;
            if (!_collections.ContainsKey(collectionKey))
                _collections.Add(collectionKey, new BuildingWaveCollection());
        }
        else
        {
            collectionKey = Ally;
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