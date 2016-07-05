using Qa.Core.System;
using Xunit;

namespace Qa.Tests
{
    public class FileMaskFilterTests
    {
        [Fact]
        public void Can_find_by_extension()
        {
            var result = new FileMaskFilter().Filter("*.txt", new[] {"heelo.txt", "hell.csv", "txt.hell" });
            Assert.Equal(1, result.Count);
            Assert.Equal("heelo.txt", result[0]);
        }

        [Fact]
        public void Can_find_by_part_of_name()
        {
            var result = new FileMaskFilter().Filter("ACCT*.*", new[] { "ACCT_201812.txt", "TRNX_201812.txt", "ATM_TRNX_201112.txt" });
            Assert.Equal(1, result.Count);
            Assert.Equal("ACCT_201812.txt", result[0]);
        }

        [Fact]
        public void Can_find_by_start_of_name()
        {
            var result = new FileMaskFilter().Filter("TRNX*.*", new[] { "ACCT_201812.txt", "TRNX_201812.txt", "ATM_TRNX_201112.txt" });
            Assert.Equal(1, result.Count);
            Assert.Equal("TRNX_201812.txt", result[0]);

            result = new FileMaskFilter().Filter("TRNX*.*", new[] { "ACCT_201812.txt", "ATM_TRNX_201112.txt" });
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void Filter_should_work_when_filename_has_multiple_dots()
        {
            var result = new FileMaskFilter().Filter("*.txt", new[] { "SOV.SBB.201507.txt" });
            Assert.Equal(1, result.Count);
        }
    }
}
