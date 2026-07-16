using System;using System.Collections.Generic;

public interface IPerformanceAdvisor
{
    List<PerformanceRecommendation> AnalyzeWorkload();
    List<MissingIndexRecommendationRule> SuggestIndexes();
}
