using UnityEngine;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;

public sealed class PatrolCommandExecutor : CommandExecutorBase<IPatrolCommand>
{
    public override void ExecuteSpecificCommand(IPatrolCommand command)
    {
        Debug.Log($"{name} is going to PATROL");
    }
}
