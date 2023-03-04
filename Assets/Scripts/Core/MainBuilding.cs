using UnityEngine;


public class MainBuilding : MonoBehaviour, IUnitProducer, ISelectable
{
    public float Health => _health;
    public float MaxHealth => _maxHealth;
    public Sprite Icon => _icon;

    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private Transform _unitsParent;
    [SerializeField] private Outline _outline;


    [SerializeField] private float _maxHealth = 1000;
    [SerializeField] private Sprite _icon;

    private float _health = 1000;
    
 
    private void Awake()
    {
        _outline.enabled = false;
    }
    public void ProduceUnit()
    {
        Instantiate(_unitPrefab, 
                    new Vector3(Random.Range(1, 19), 0, Random.Range(1, 19)), 
                    Quaternion.identity, 
                    _unitsParent);
    }
    public void UpdateSelection(bool b)
    {
        _outline.enabled = b;
    }
}
