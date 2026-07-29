using System;

public class DatabaseConfigBuilder : IDatabaseConfigBuilder
{
    public IDatabaseConfigBuilder SetName(string name)
    {
        throw new NotImplementedException();
    }

    public IDatabaseConfigBuilder SetPageSize(int size)
    {
        throw new NotImplementedException();
    }

    public IDatabaseConfigBuilder EnableLogging(string logPath)
    {
        throw new NotImplementedException();
    }

    public IDatabaseConfigBuilder EnableEncryption()
    {
        throw new NotImplementedException();
    }

    public IDatabaseConfigBuilder SetMaxConnections(int maxConnections)
    {
        throw new NotImplementedException();
    }

    public DatabaseConfiguration Build()
    {
        throw new NotImplementedException();
    }
}
