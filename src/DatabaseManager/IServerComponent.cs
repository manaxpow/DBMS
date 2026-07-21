using System;

public interface IServerComponent
{
    void Start(object config);
    void Stop();
}
