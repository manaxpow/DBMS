public interface ITableBuilder
{
    Table Build();

    ITableBuilder SetName(string name);

    ITableBuilder AddColumn(Column column);

    ITableBuilder AddConstraint(Constraint constraint);

    ITableBuilder AddIndex(Index index);

    ITableBuilder AddPartition(Partition partition);
}
