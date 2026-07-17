using System;
using Xunit;
using DBMS.StorageEngine;

namespace DBMS.UnitTests.StorageEngine
{
    public class BufferPoolTests
    {
        [Fact]
        public void FetchPage_WhenPageIsBuffered_ShouldReturnExistingFrame()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void FetchPage_WhenSpaceIsAvailable_ShouldLoadPage()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void FetchPage_WhenAllFramesArePinned_ShouldThrow()
        {
            throw new NotImplementedException();
        }

    }
}

