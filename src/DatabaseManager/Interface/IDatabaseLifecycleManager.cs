using System;

public interface IDatabaseLifecycleManager
{
    DatabaseId CreateDatabase(string name);
    void DropDatabase(DatabaseId dbId);
    void StartDatabase(DatabaseId dbId);
    void StopDatabase(DatabaseId dbId);
}
