using System;
using Xunit;
using DBMS.DatabaseManager;

namespace DBMS.UnitTests.DatabaseManager
{
    public class StatisticsManagerTests
    {
        [Fact]
        public void UpdateStatistics_WhenDataChanges_ShouldRefreshStatistics()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void EstimateSelectivity_WhenStatisticsExist_ShouldReturnEstimate()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void EstimateSelectivity_WhenStatisticsAreMissing_ShouldUseFallback()
        {
            throw new NotImplementedException();
        }

    }
}

