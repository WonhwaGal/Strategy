using UnityEngine;
using Code.Pools;
using Code.ScriptableObjects;
using Code.UI;

public class Launcher : MonoBehaviour
{
    [SerializeField] private RuinList _ruinsList;
    [SerializeField] private FollowUIList _hpBarList;

    void Start()
    {
        ServiceLocator.Container.Register(new RuinMultiPool(_ruinsList));
        ServiceLocator.Container.Register(new FollowUIPool(_hpBarList));
    }
}