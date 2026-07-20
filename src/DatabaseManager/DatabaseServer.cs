using System;
using System.Collections.Generic;

public class DatabaseServer
{
    private object _config;
    private IReadOnlyList<object> _components;
    private bool _isRunning;

    public void Start(object config)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}
