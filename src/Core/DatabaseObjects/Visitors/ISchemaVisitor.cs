public interface ISchemaVisitor
{
    void Visit(Schema schema);

    void Visit(Table table);

    void Visit(View view);

    void Visit(StoredProcedure storedProcedure);
}
