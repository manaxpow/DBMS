public interface ISchemaObject
{
    int Id { get; }
    string Name { get; }

    SchemaObjectType ObjectType { get; }
    void Drop();
    void Accept(ISchemaVisitor visitor);
    ISchemaObject Clone();
}
