using UnityEngine;
using Code.Construction;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(RuinList), menuName = "Ruins/RuinList")]
    public class RuinList : PrefabSO<RuinView>
    {
    }
}