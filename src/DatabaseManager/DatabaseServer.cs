using System;
using System.Collections.Generic;
using DBMS.Exceptions;

public class DatabaseServer
{
    // private object? _config;
    private readonly IReadOnlyList<IServerComponent> _components;

    public DatabaseServer(IReadOnlyList<IServerComponent> components)
    {
        this._components = components ?? new List<IServerComponent>();
    }

    public bool IsRunning { get; private set; }

    public void Start(object config)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}
