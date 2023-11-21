using Code.UI;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(HPBarList), menuName = "UI/HPBarList")]
    public sealed class HPBarList : UIPrefabSO<HPBar>
    {
    }
}