using Code.Units;
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ConstructionView : MonoBehaviour
{
    [SerializeField] private GameObject CastleView;

    public event Action OnTriggerIn;
    public event Action OnTriggerOut;

    private void OnEnable()
    {
        CastleView.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        CastleView.SetActive(other.GetComponent<UnitView>());
        OnTriggerIn?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        CastleView.SetActive(!other.GetComponent<UnitView>());
        OnTriggerOut?.Invoke();
    }

    private void OnDestroy()
    {
        OnTriggerIn = null;
        OnTriggerOut = null;
    }
}
