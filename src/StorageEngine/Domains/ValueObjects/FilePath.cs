using System;

namespace DBMS.StorageEngine.FileManagement.Domain;

public readonly record struct FilePath
{
    public string Value { get; }

    public FilePath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("FilePath cannot be null or whitespace.", nameof(value));
        }

        Value = value;
    }

    public override string ToString() => Value;

    public static implicit operator string(FilePath path) => path.Value;
    public static implicit operator FilePath(string path) => new FilePath(path);
}
