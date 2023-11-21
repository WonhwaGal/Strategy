using UnityEngine;
using Code.Pools;
using Code.ScriptableObjects;

public class Launcher : MonoBehaviour
{
    [SerializeField] private RuinList _ruinsList;

    void Start()
    {
        ServiceLocator.Container.Register(new RuinMultiPool(_ruinsList));
    }
}