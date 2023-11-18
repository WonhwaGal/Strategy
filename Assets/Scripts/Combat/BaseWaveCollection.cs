using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Combat
{
    public abstract class BaseWaveCollection<T> : IDisposable where T : IPresenter
    {
        protected Dictionary<GameObject, T> _participants = new();

        public event Action <bool> OnWaveOver;

        public void AddToCollection(GameObject gameObj, T presenter)
        {
            Debug.Log(gameObj.name + " added");
            _participants.Add(gameObj, presenter);
            OnAddToCollection(presenter);
        }

        public void RemoveFromCollection(GameObject gameObj)
        {
            _participants.Remove(gameObj);
            OnRemoveFromCollection();
        }

        public void ApplyDamage(GameObject gameObj, int damage)
        {
            if (_participants.TryGetValue(gameObj, out T presenter))
                presenter.ReceiveDamage(damage);
            else
                Debug.Log($"Participant {gameObj.name} not found");
        }

        public IPresenter FindParticipant(GameObject gameObj) => _participants[gameObj];

        protected virtual void OnAddToCollection(T presenter) { }
        protected virtual void OnRemoveFromCollection() { }

        protected void EndNight(bool isVictory) => OnWaveOver?.Invoke(isVictory);

        public void Dispose()
        {
            _participants.Clear();
            OnWaveOver = null;
            GC.SuppressFinalize(this);
        }
    }
}