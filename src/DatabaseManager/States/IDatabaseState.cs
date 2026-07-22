public interface IDatabaseState
{
    void Open();
    void SetReadOnly();
    void Recovery();
    void Drop();
}
