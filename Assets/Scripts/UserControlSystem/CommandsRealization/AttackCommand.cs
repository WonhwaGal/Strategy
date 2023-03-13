using UnityEngine;
using Abstractions.Commands.CommandsInterfaces;
using Abstractions;

namespace UserControlSystem.CommandsRealization
{
    public sealed class AttackCommand : IAttackCommand
    {
        public IAttackable Target { get; }

        public AttackCommand(IAttackable target)
        {
            Target = target;
        }
    }
}