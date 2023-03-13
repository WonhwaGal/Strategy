using Utils;
using System;
using Zenject;
using Abstractions;
using UserControlSystem.CommandsRealization;
using Abstractions.Commands.CommandsInterfaces;


namespace UserControlSystem
{
    public sealed class AttackCommandCommandCreator : CommandCreatorBase<IAttackCommand>
    {
        [Inject] private AssetsContext _context;
        private Action<IAttackCommand> _creationCallback;

        [Inject]
        private void Init(AttackableValue target) => target.OnSelected += onNewTarget;

        protected override void ClassSpecificCommandCreation(Action<IAttackCommand> creationCallback)
        {
            _creationCallback = creationCallback;
        }


        private void onNewTarget(IAttackable targetObject)
        {
            _creationCallback?.Invoke(_context.Inject(new AttackCommand(targetObject)));
            _creationCallback = null;
        }


        public override void ProcessCancel()
        {
            base.ProcessCancel();
            _creationCallback = null;
        }
    }
}