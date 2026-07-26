using System;

public class PhysicalOperatorFactory : OperatorFactory {
    public override PhysicalOperator CreateOperator(LogicalNode node) {
        return node switch {
            LogicalTableScan scan => new TableScanOperator(scan.TableName),
            LogicalIndexScan _ => new IndexScanOperator(),
            LogicalHashJoin _ => new HashJoinOperator(),
            LogicalNestedLoopJoin _ => new NestedLoopJoinOperator(),
            LogicalSort _ => new SortOperator(),
            _ => throw new NotSupportedException("Unsupported logical node: " + node.GetType().Name)
        };
    }
}
