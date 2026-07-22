public class ConstraintContext
{
    public Row CandidateRow { get; }
    public Row? ExistingRow { get; }
    public Table Table { get; }
    public Schema Schema { get; }

    public ConstraintContext(Row candidateRow, Table table, Schema schema, Row? existingRow = null)
    {
        CandidateRow = candidateRow;
        Table = table;
        Schema = schema;
        ExistingRow = existingRow;
    }
}
