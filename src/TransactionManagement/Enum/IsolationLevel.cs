using System;

public enum IsolationLevel { 
    ReadUncommitted, 
    ReadCommitted, 
    RepeatableRead, 
    Serializable 
}
