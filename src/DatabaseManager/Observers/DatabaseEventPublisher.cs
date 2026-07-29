public class DatabaseEventPublisher
{
    private List<IDatabaseEventObserver> observers = new List<IDatabaseEventObserver>();

    public void AddListener(IDatabaseEventObserver listener) => this.observers.Add(listener);

    public void RemoveListener(IDatabaseEventObserver listener) => this.observers.Remove(listener);
}
