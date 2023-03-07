using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;

public sealed class UnitMove : CommandExecutorBase<IMoveCommand>
{
    private void Awake()
    {
        CommandType = CommandExecuterType.Move;
    }
    public override void ExecuteSpecificCommand(IMoveCommand command)
    {
        
    }
}
