public class DatabaseEventPublisher
{
    private List<IDatabaseEventObserver> _observers = new List<IDatabaseEventObserver>();
    public void AddListener(IDatabaseEventObserver listener) => _observers.Add(listener);
    public void RemoveListener(IDatabaseEventObserver listener) => _observers.Remove(listener);
}
