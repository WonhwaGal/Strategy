using System;

public class ValueObject<T>
{
    public T CurrentValue { get; private set; }

    public Action<T> OnSelected;
    public void SetValue(T value)
    {
        CurrentValue = value;
        OnSelected?.Invoke(value);

        OnSetValue(value);
    }
    public virtual void OnSetValue(T value) { }
}
