using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.Units
{
    public class UnitAnimator : IDisposable
    {
        private readonly Animator _animator;
        private static readonly int s_Move = Animator.StringToHash("Move");
        private static readonly int s_Attack = Animator.StringToHash("Attack");
        private static readonly int s_Die = Animator.StringToHash("Die");
        private static readonly int s_AreaAttack = Animator.StringToHash("AreaAttack");
        private const string DeathClip = "Death";
        private const int DefaultLength = 850;
        private int _dieClipLength;

        public UnitAnimator(Animator animator)
        {
            _animator = animator;
            GetDieClipLength();
        }

        public event Action<bool> OnDyingAnimated;

        public void AnimateMovement(bool isMoving) => _animator.SetBool(s_Move, isMoving);

        public void AnimateAttack() => _animator.SetTrigger(s_Attack);

        public async void AnimateAreaAttack(GameObject go)
        {
            go.SetActive(true);
            _animator.SetTrigger(s_AreaAttack);
            await Task.Delay(DefaultLength);
            go.SetActive(false);
        }

        public async void AnimateDeath()
        {
            _animator.SetTrigger(s_Die);
            await Task.Delay(_dieClipLength);
            OnDyingAnimated?.Invoke(false);
        }

        private int GetDieClipLength()
        {
            AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name == DeathClip)
                    return _dieClipLength = (int)(clips[i].length * DefaultLength);
            }
            return DefaultLength;
        }

        public void Dispose() => OnDyingAnimated = null;
    }
}