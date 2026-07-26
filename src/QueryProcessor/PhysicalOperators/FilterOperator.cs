using System;

public class FilterOperator : PhysicalOperator {
    public PhysicalOperator Child { get; }
    public Func<Row, bool> Predicate { get; }
    
    public FilterOperator(PhysicalOperator child, Func<Row, bool> predicate) {
        Child = child;
        Predicate = predicate;
    }

    public override void Open() { 
        Child.Open(); 
    }

    public override bool Next() { 
        while (Child.Next()) {
            if (Predicate(Child.GetCurrent())) {
                return true;
            }
        }
        return false;
    }

    public override Row GetCurrent() { 
        return Child.GetCurrent();
    }

    public override void Close() { 
        Child.Close(); 
    }
}
