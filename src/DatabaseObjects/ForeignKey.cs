using System;
using System.Collections.Generic;

public enum ReferentialAction
{
    NoAction,
    Restrict,
    Cascade,
    SetNull,
    SetDefault
}

public class ForeignKey : Constraint
{
    public string ChildColumnName { get; set; }
    public string ReferencedTableName { get; set; }
    public string ReferencedColumnName { get; set; }
    public ReferentialAction OnDelete { get; set; }
    public ReferentialAction OnUpdate { get; set; }
    public bool IsNullable { get; set; }
    
    private Schema _schema;

    public ForeignKey(string referencedTableName)
    {
        ReferencedTableName = referencedTableName;
    }

    protected override bool Check(object value) => throw new NotImplementedException();

    public new bool Validate(object parentKey) => throw new NotImplementedException();
    public void DeleteParent(object parentKey) => throw new NotImplementedException();
    public void UpdateParent(object oldKey, object newKey) => throw new NotImplementedException();
    private IReadOnlyList<Row> GetReferencingRows(object parentKey) => throw new NotImplementedException();
}
