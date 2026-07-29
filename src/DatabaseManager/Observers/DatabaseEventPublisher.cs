public class DatabaseEventPublisher
{
    private List<IDatabaseEventObserver> _observers = new List<IDatabaseEventObserver>();

    public void AddListener(IDatabaseEventObserver listener) => this._observers.Add(listener);

    public void RemoveListener(IDatabaseEventObserver listener) => this._observers.Remove(listener);
}
