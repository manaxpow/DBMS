using System;

public interface ISchemaObject
{
    int Id { get; }
    string Name { get; }

    SchemaObjectType ObjectType { get; }
    void Drop();
}
