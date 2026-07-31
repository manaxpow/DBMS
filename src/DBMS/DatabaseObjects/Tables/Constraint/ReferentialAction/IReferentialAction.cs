using System;

public interface IReferentialAction
{
    void Execute(Row parentRow, Table childTable);
}
