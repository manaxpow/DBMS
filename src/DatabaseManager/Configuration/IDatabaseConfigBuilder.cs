using System;

public interface IDatabaseConfigBuilder
{
    IDatabaseConfigBuilder SetName(string name);

    IDatabaseConfigBuilder SetPageSize(int size);

    IDatabaseConfigBuilder EnableLogging(string logPath);

    IDatabaseConfigBuilder EnableEncryption();

    IDatabaseConfigBuilder SetMaxConnections(int maxConnections);

    DatabaseConfiguration Build();
}
