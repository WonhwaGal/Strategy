using UnityEngine;
using Code.Pools;
using Code.ScriptableObjects;
using Code.UI;

public class Launcher : MonoBehaviour
{
    [SerializeField] private RuinList _ruinsList;
    [SerializeField] private HPBarList _hpBarList;
    [SerializeField] private UIPoolRoot _uiPoolRoot;

    void Start()
    {
        ServiceLocator.Container.Register(new RuinMultiPool(_ruinsList));
        ServiceLocator.Container.Register(new HPBarPool(_hpBarList, _uiPoolRoot));
    }
}