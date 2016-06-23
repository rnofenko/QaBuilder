using Qa.Core.Excel;
using Qa.Core.Parsers;
using Xunit;

namespace Qa.Tests
{
    public class FileDateParserTests
    {
        private readonly FileDateFormatter _parser;

        public FileDateParserTests()
        {
            _parser = new FileDateFormatter();
        }

        [Fact]
        public void Parse_Month()
        {
            var result =_parser.Format("qirweyrtuwe_201603");
            Assert.Equal("March 2016", result);
        }

        [Fact]
        public void Parse_Week()
        {
            var result = _parser.Format("qirweyrtuwe_20160307");
            Assert.Equal("March 7, 2016", result);
        }

        [Fact]
        public void Parse_ArgusConsumer()
        {
            var result = _parser.Format("2015-07 Account Attributes (Corrected Overdraft Protection Flag).csv");
            Assert.Equal("July 2015", result);
        }
    }
}