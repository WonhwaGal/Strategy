using UnityEngine;
using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;

public sealed class AttackCommandExecutor : CommandExecutorBase<IAttackCommand>, ISelectable
{
    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public Sprite Icon => _icon;

    [SerializeField] private Outline _outline;

    [SerializeField] private float _maxHealth = 500;
    [SerializeField] private Sprite _icon;

    private float _health = 500;

    private void Awake()
    {
        _outline.enabled = false;
    }

    public void UpdateSelection(bool b)
    {
        _outline.enabled = b;
    }

    public override void ExecuteSpecificCommand(IAttackCommand command)
    {
        Debug.Log($"{name} is going to attack to {command.Target}");
    }
}
