using System;
using System.IO;

namespace DBMS.StorageEngine.IntegrationTests.FileManagement.TestSupport;

internal sealed class TemporaryDirectory : IDisposable
{
    public string Path { get; }

    public TemporaryDirectory()
    {
        Path = System.IO.Path.Combine(
            System.IO.Path.GetTempPath(),
            "DBMS.StorageEngine.Tests",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(Path);
    }

    public void Dispose()
    {
        if (Directory.Exists(Path))
        {
            try
            {
                Directory.Delete(Path, recursive: true);
            }
            catch
            {
                // Tests might fail to clean up if handles aren't closed,
                // but we should report cleanup failure if the framework permits.
                // For IDisposable in xUnit, exceptions here bubble up and fail the test.
                throw new IOException($"Failed to delete temporary directory: {Path}. Ensure all handles are closed.");
            }
        }
    }
}
