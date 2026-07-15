using System;
using System.IO;

namespace DBMS.StorageEngine.FileManagement.PhysicalStorage;

public class FileHandle : IDisposable
{
    private FileStream? _stream;
    private bool _isDisposed;

    internal FileHandle() { }

    internal FileHandle(FileStream stream)
    {
        _stream = stream ?? throw new ArgumentNullException(nameof(stream));
    }

    public virtual int ReadAtOffset(long offset, Memory<byte> destination)
    {
        if (_isDisposed) throw new ObjectDisposedException(nameof(FileHandle));
        _stream!.Position = offset;
        return _stream!.Read(destination.Span);
    }

    public virtual void WriteAtOffset(long offset, ReadOnlyMemory<byte> source)
    {
        if (_isDisposed) throw new ObjectDisposedException(nameof(FileHandle));
        _stream!.Position = offset;
        _stream!.Write(source.Span);
    }

    public void Flush()
    {
        if (_isDisposed) throw new ObjectDisposedException(nameof(FileHandle));
        _stream!.Flush(flushToDisk: true);
    }

    internal long GetSize()
    {
        if (_isDisposed) throw new ObjectDisposedException(nameof(FileHandle));
        return _stream!.Length;
    }

    internal void Resize(long newSize)
    {
        if (_isDisposed) throw new ObjectDisposedException(nameof(FileHandle));
        _stream!.SetLength(newSize);
    }
    
    public void Dispose()
    {
        if (!_isDisposed)
        {
            _stream?.Dispose();
            _stream = null;
            _isDisposed = true;
        }
    }
}
