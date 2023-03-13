using Utils;
using System;
using Zenject;
using Abstractions.Commands.CommandsInterfaces;


namespace UserControlSystem
{
    internal class StopCommandCommandCreator: CommandCreatorBase<IStopCommand>
    {
        [Inject] private AssetsContext _context;
        protected override void ClassSpecificCommandCreation(Action<IStopCommand> creationCallback)
        {
            creationCallback?.Invoke(_context.Inject(new StopCommand()));
        }
    }
}