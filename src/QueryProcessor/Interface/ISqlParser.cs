using System;

public interface ISqlParser
{
    SqlStatement Parse(string sql);
}
