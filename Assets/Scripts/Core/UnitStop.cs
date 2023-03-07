using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;

public sealed class UnitStop : CommandExecutorBase<IStopCommand>
{
    private void Awake()
    {
        CommandType = CommandExecuterType.Stop;
    }
    public override void ExecuteSpecificCommand(IStopCommand command)
    {
        
    }
}
