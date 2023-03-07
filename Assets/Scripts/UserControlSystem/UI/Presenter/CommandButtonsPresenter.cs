using Utils;
using System;
using UnityEngine;
using Abstractions;
using Abstractions.Commands;
using UserControlSystem.UI.View;
using System.Collections.Generic;
using UserControlSystem.CommandsRealization;
using Abstractions.Commands.CommandsInterfaces;


namespace UserControlSystem.UI.Presenter
{
    public sealed class CommandButtonsPresenter : MonoBehaviour
    {
        [SerializeField] private SelectableValue _selectable;
        [SerializeField] private CommandButtonsView _view;
        [SerializeField] private AssetsContext _context;

        private ISelectable _currentSelectable;
        private void Start()
        {
            _selectable.OnSelected += onSelected;
            onSelected(_selectable.CurrentValue);
            _view.OnClick += onButtonClick;
        }

        private void onSelected(ISelectable selectable)
        {
            if (_currentSelectable == selectable)
            {
                return;
            }

            _currentSelectable = selectable;
            _view.Clear();
            
            if (selectable != null)
            {
                var commandExecutors = new List<ICommandExecutor>();
                commandExecutors.AddRange((selectable as Component).GetComponentsInParent<ICommandExecutor>());
                _view.MakeLayout(commandExecutors);     // problematic line
            }
        }

        private void onButtonClick(ICommandExecutor commandExecutor)
        {
            switch (commandExecutor.CommandType)
            {
                case CommandExecuterType.ProduceUnit:
                    var unitProducer = commandExecutor as CommandExecutorBase<IProduceUnitCommand>;
                    if (unitProducer != null)
                    {
                        unitProducer.ExecuteSpecificCommand(_context.Inject(new ProduceUnitCommandHeir()));
                    }
                    return;
                case CommandExecuterType.Move:
                    var moveExecuter = commandExecutor as CommandExecutorBase<IMoveCommand>;
                    if (moveExecuter != null)
                    {
                        Debug.Log("Executing Move");
                    }
                    return;
                case CommandExecuterType.Attack:
                    var atackExecuter = commandExecutor as CommandExecutorBase<IAttackCommand>;
                    if (atackExecuter != null)
                    {
                        Debug.Log("Executing Attack");
                    }
                    return;
                case CommandExecuterType.Patrol:
                    var patrolExecuter = commandExecutor as CommandExecutorBase<IPatrolCommand>;
                    if (patrolExecuter != null)
                    {
                        Debug.Log("Executing Patrol");
                    }
                    return;
                case CommandExecuterType.Stop:
                    var holdpositionExecuter = commandExecutor as CommandExecutorBase<IStopCommand>;
                    if (holdpositionExecuter != null)
                    {
                        Debug.Log("Executing HoldPosition");
                    }
                    return;
                default:
                    throw new
                    ApplicationException($"{nameof(CommandButtonsPresenter)}.{nameof(onButtonClick)}: " +
                             $" Unknown type of commands executor: { commandExecutor.GetType().FullName }!");
            }
        }
    }

}
