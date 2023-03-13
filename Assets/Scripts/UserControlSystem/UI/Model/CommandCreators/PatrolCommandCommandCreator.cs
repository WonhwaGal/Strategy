using Utils;
using System;
using Zenject;
using UserControlSystem.CommandsRealization;
using Abstractions.Commands.CommandsInterfaces;


namespace UserControlSystem
{
    internal class PatrolCommandCommandCreator : CommandCreatorBase<IPatrolCommand>
    {
        [Inject] private AssetsContext _context;
        protected override void ClassSpecificCommandCreation(Action<IPatrolCommand> creationCallback)
        {
            creationCallback?.Invoke(_context.Inject(new PatrolCommand()));
        }
    }
}