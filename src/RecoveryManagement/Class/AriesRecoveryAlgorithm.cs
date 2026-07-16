using System;

public class AriesRecoveryAlgorithm : ILogBasedRecovery
{
    private object _trt;
    private object _dpt;

    public RecoveryAnalysisPhase PerformAnalysis()
    {
        return default;
    }

    public void PerformRedo()
    {
    }

    public void PerformUndo()
    {
    }
}
