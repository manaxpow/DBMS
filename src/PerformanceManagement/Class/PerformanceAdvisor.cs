using System;using System.Collections.Generic;

public class PerformanceAdvisor : IPerformanceAdvisor
{
    private object _engine;

    public List<PerformanceRecommendation> AnalyzeWorkload()
    {
        return default;
    }

    public List<MissingIndexRecommendationRule> SuggestIndexes()
    {
        return default;
    }

    public void EvaluateRules()
    {
    }
}
