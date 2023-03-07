using UnityEngine;
using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;

public sealed class UnitAttack : CommandExecutorBase<IAttackCommand>, ISelectable
{
    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public Sprite Icon => _icon;

    [SerializeField] private Outline _outline;

    [SerializeField] private float _maxHealth = 1000;
    [SerializeField] private Sprite _icon;

    private float _health = 500;

    private void Awake()
    {
        CommandType = CommandExecuterType.Attack;
        _outline.enabled = false;
    }

    public void UpdateSelection(bool b)
    {
        _outline.enabled = b;
    }

    public override void ExecuteSpecificCommand(IAttackCommand command)
    {
        
    }
}
