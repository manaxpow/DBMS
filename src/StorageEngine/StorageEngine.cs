using System;



public class StorageEngine : IServerComponent
{
    public EngineState State { get; set; }

    public void Start(object configuration)
    {
        throw new NotImplementedException();
    }

    private bool ValidateConfiguration(object configuration)
    {
        throw new NotImplementedException();
    }

    private void SetState(object state)
    {
        throw new NotImplementedException();
    }

    public object ReadPage(object pageId)
    {
        throw new NotImplementedException();
    }

    public void WritePage(object pageId, object data)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public void DropTableStorage(int tableId)
    {
        throw new NotImplementedException();
    }
}
