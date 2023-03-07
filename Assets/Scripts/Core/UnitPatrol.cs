using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;

public sealed class UnitPatrol : CommandExecutorBase<IPatrolCommand>
{
    private void Awake()
    {
        CommandType = CommandExecuterType.Patrol;
    }
    public override void ExecuteSpecificCommand(IPatrolCommand command)
    {
        
    }
}
