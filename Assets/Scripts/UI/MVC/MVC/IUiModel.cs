namespace Code.MVC
{
    public interface IUiModel
    {
        virtual void Receive() { }
        virtual void Update() { }
    }
}