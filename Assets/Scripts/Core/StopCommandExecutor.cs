using UnityEngine;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;

public sealed class StopCommandExecutor : CommandExecutorBase<IStopCommand>
{
    public override void ExecuteSpecificCommand(IStopCommand command)
    {
        Debug.Log($"{name} is going to STOP");
    }
}
