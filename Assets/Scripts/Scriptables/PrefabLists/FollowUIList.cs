using Code.UI;
using UnityEngine;

namespace Code.ScriptableObjects
{
    [CreateAssetMenu(fileName = nameof(FollowUIList), menuName = "UI/FollowBarList")]
    public sealed class FollowUIList : UIPrefabSO<FollowUIView>
    {
    }
}