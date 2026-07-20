using System;
using System.Collections.Generic;
using DBMS.Exceptions;

public class DatabaseServer
{
    private object? _config;
    private readonly IReadOnlyList<IServerComponent> _components;
    private bool _isRunning;

    public DatabaseServer(IReadOnlyList<IServerComponent> components)
    {
        _components = components ?? new List<IServerComponent>();
    }

    public void Start(object config)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public bool IsRunning => _isRunning;
}
