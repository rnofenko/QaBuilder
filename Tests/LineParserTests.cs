﻿using Qa.Core.Collectors;
using Xunit;

namespace Qa.Tests
{
    public class LineParserTests
    {
        [Fact]
        public void SimpleCsv()
        {
            var result = new LineParser(",", "\"").Parse("apple,juice");
            Assert.Equal(new[] { "apple", "juice" }, result);
        }

        [Fact]
        public void Csv_with_quotes()
        {
            var result = new LineParser(",", "\"").Parse("\"apple\",\"juice\"");
            Assert.Equal(new[] { "apple", "juice" }, result);
        }

        [Fact]
        public void Csv_with_quotes_and_without_quotes()
        {
            var result = new LineParser(",", "\"").Parse("\"apple\",juice");
            Assert.Equal(new[] { "apple", "juice" }, result);
        }

        [Fact]
        public void Csv_with_quotes_and_with_comma()
        {
            var result = new LineParser(",", "\"").Parse("\"apple,pineapple\",\"juice\"");
            Assert.Equal(new[] { "apple,pineapple", "juice" }, result);
        }

        [Fact]
        public void Csv_with_quotes_and_without_quotes_and_with_comma()
        {
            var result = new LineParser(",", "\"").Parse("\"apple,pineapple\",grape,\"juice\"");
            Assert.Equal(new[] {"apple,pineapple", "grape", "juice"}, result);
        }

        [Fact]
        public void Line_with_pipe_delimiter()
        {
            var result = new LineParser("|", "\"").Parse("\"apple|pineapple\"|grape|\"juice\"");
            Assert.Equal(new[] { "apple|pineapple", "grape", "juice" }, result);
        }

        [Fact]
        public void Line_with_semicolon_delimiter()
        {
            var result = new LineParser(";", "\"").Parse("\"apple;pineapple\";grape;\"juice\"");
            Assert.Equal(new[] { "apple;pineapple", "grape", "juice" }, result);
        }

        [Fact]
        public void Csv_with_single_quotes_and_without_quotes_and_with_comma()
        {
            var result = new LineParser(",", "'").Parse("'apple,pineapple',grape,'juice'");
            Assert.Equal(new[] { "apple,pineapple", "grape", "juice" }, result);
        }

        [Fact]
        public void Csv_with_single_quote_inside()
        {
            var result = new LineParser(",", "\"").Parse("\"Tom's apple\",Tom's grape");
            Assert.Equal(new[] { "Tom's apple", "Tom's grape" }, result);
        }


        [Fact]
        public void Csv_with_single_quote_inside_and_single_quote_as_a_text_qualifier()
        {
            var result = new LineParser(",", "'").Parse("'Tom\\'s apple',Tom grape");
            Assert.Equal(new[] { "Tom's apple", "Tom's grape" }, result);
        }
    }
}