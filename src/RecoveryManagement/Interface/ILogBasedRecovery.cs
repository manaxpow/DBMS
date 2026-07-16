using System;

public interface ILogBasedRecovery
{
    RecoveryAnalysisPhase PerformAnalysis();
    void PerformRedo();
    void PerformUndo();
}
