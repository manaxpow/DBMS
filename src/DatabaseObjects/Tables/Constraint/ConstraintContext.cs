public class ConstraintContext(Row candidateRow, Table table, Schema schema, Row? existingRow = null)
{
    public Row CandidateRow { get; } = candidateRow;

    public Row? ExistingRow { get; } = existingRow;

    public Table Table { get; } = table;

    public Schema Schema { get; } = schema;
}
