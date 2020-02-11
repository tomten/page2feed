using System;
using Page2Feed.Core.Util;
using Xunit;

namespace Page2Feed.Tests.Core
{

    public class UnitTest1
    {

        [Fact]
        public void Hex()
        {
            var arrange = new byte[] { 0, 6, 0xa, 6, 0 };
            var act = arrange.Hex();
            Assert.Equal("00060a0600", act);
        }

    }

}
