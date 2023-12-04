using UnityEngine;
using Code.Pools;

namespace Code.Construction
{
    public class RuinView : MonoBehaviour, ISpawnableType
    {
        [SerializeField] private PrefabType _prefabType;
        [SerializeField] private GameObject[] _stages;

        public PrefabType PrefabType => _prefabType;

        public void SetStage(int currentStage)
        {
            for(int i = 0; i < _stages.Length; i++)
                _stages[i].SetActive(i == currentStage - 1);
        }
    }
}