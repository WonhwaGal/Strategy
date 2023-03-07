using UnityEngine;
using Abstractions;
using Abstractions.Commands;
using Abstractions.Commands.CommandsInterfaces;

public class Unit : CommandExecutorBase<ICommand>, ISelectable
{
    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public Sprite Icon => _icon;

    [SerializeField] private Outline _outline;

    [SerializeField] private float _maxHealth = 1000;
    [SerializeField] private Sprite _icon;

    private float _health = 1000;

    private void Awake()
    {
        _outline.enabled = false;
    }

    public void UpdateSelection(bool b)
    {
        _outline.enabled = b;
    }

    public override void ExecuteSpecificCommand(ICommand command)
    {
        throw new System.NotImplementedException();
    }
}
