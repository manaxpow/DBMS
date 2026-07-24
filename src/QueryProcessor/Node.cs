using System.Collections.Generic;

public enum ASTNodeType
{
    SelectStatement,
    InsertStatement,
    TableReference,
    ColumnReference
}

public class Node
{
    public ASTNodeType Type { get; set; }
    public List<Node> Children { get; set; } = new List<Node>();
}
