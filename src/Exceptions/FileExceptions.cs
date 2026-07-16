using System;

namespace DBMS.StorageEngine.FileManagement.Exceptions;

public class FileAlreadyExistsException : Exception { public FileAlreadyExistsException(string msg) : base(msg) {} }
public class FileCreationException : Exception { public FileCreationException(string msg, Exception inner) : base(msg, inner) {} }
public class FileNotOpenException : Exception { public FileNotOpenException(string msg) : base(msg) {} }
public class FileCloseException : Exception { public FileCloseException(string msg, Exception inner) : base(msg, inner) {} }
public class FileTruncationException : Exception { public FileTruncationException(string msg) : base(msg) {} }
public class MaximumFileSizeExceededException : Exception { public MaximumFileSizeExceededException(string msg) : base(msg) {} }
public class FileResizeException : Exception { public FileResizeException(string msg, Exception inner) : base(msg, inner) {} }
public class FileNotFoundException : Exception { public FileNotFoundException(string msg) : base(msg) {} }
public class InvalidFileFormatException : Exception { public InvalidFileFormatException(string msg) : base(msg) {} }
public class FileOpenException : Exception { public FileOpenException(string msg, Exception inner) : base(msg, inner) {} }
public class FileDeleteException : Exception { public FileDeleteException(string msg, Exception inner) : base(msg, inner) {} }
public class FileInUseException : Exception { public FileInUseException(string msg) : base(msg) {} }
public class LockConflictException : Exception { public LockConflictException(string msg) : base(msg) {} }
public class UnsupportedFileVersionException : Exception { public UnsupportedFileVersionException(string msg) : base(msg) {} }
public class CorruptedFileMetadataException : Exception { public CorruptedFileMetadataException(string msg) : base(msg) {} }
public class IncompletePageReadException : Exception { public IncompletePageReadException(string msg) : base(msg) {} }
public class ReadFailureException : Exception { public ReadFailureException(string msg) : base(msg) {} public ReadFailureException(string msg, Exception inner) : base(msg, inner) {} }
public class IncompletePageWriteException : Exception { public IncompletePageWriteException(string msg) : base(msg) {} }
public class NoFreeExtentException : Exception { public NoFreeExtentException(string msg) : base(msg) {} }
public class ExtentAllocationException : Exception { public ExtentAllocationException(string msg, Exception inner) : base(msg, inner) {} }
public class ExtentInUseException : Exception { public ExtentInUseException(string msg) : base(msg) {} }
public class InvalidExtentException : Exception { public InvalidExtentException(string msg) : base(msg) {} }
public class ExtentAlreadyFreeException : Exception { public ExtentAlreadyFreeException(string msg) : base(msg) {} }

public class ReadOnlyFileException : Exception { public ReadOnlyFileException(string msg) : base(msg) {} }
public class WriteFailureException : Exception { public WriteFailureException(string msg) : base(msg) {} public WriteFailureException(string msg, Exception inner) : base(msg, inner) {} }
public class FileSyncException : Exception { public FileSyncException(string msg, Exception inner) : base(msg, inner) {} }
public class InvalidPageIdException : Exception { public InvalidPageIdException(string msg) : base(msg) {} }
