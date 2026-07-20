using System;

public interface IServerComponent
{
    void Initialize(object config);
    void Shutdown();
}
