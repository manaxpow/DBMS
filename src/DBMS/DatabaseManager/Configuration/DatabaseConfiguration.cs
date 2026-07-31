using System;

public class DatabaseConfiguration
{
    public string DatabaseName { get; set; } = null!;

    public int PageSize { get; set; }

    public string LogPath { get; set; } = null!;

    public bool EncryptionEnabled { get; set; }

    public int MaxConnections { get; set; }

    public void PrintConfiguration()
    {
        throw new NotImplementedException();
    }
}
