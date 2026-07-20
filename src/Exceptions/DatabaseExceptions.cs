using System;

namespace DBMS.Exceptions
{
    public class DatabaseException : Exception { }
    public class DatabaseAlreadyExistsException : DatabaseException { }
    public class DatabaseCreationException : DatabaseException { }
    public class DatabaseNotFoundException : DatabaseException { }
    public class ObjectAlreadyExistsException : DatabaseException { }
    public class ObjectNotFoundException : DatabaseException { }
    public class SchemaAlreadyExistsException : DatabaseException { }
    public class SchemaNotFoundException : DatabaseException { }
    public class SchemaReferencedException : DatabaseException { }
    public class ServerAlreadyRunningException : DatabaseException { }
    public class ServerNotRunningException : DatabaseException { }
    public class ComponentInitializationException : DatabaseException { }
    public class ComponentShutdownException : DatabaseException { }
    public class StorageInitializationException : DatabaseException { }
    public class FlushFailureException : DatabaseException { }
}
