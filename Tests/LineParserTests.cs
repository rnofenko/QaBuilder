using Qa.Core.Parsers;
using Xunit;

namespace Qa.Tests
{
    public class LineParserTests
    {
        [Fact]
        public void SimpleCsv()
        {
            var result = new CsvParser(",", "\"").Parse("apple,juice");
            Assert.Equal(new[] { "apple", "juice" }, result);
        }

        [Fact]
        public void EmptyFieldAtTheEnd()
        {
            var result = new CsvParser(",", "\"").Parse("apple,juice,");
            Assert.Equal(new[] { "apple", "juice", "" }, result);
        }

        [Fact]
        public void EmptyString()
        {
            var result = new CsvParser(",", "\"").Parse("");
            Assert.Equal(new[] { "" }, result);
        }

        [Fact]
        public void ManyFields()
        {
            var result = new CsvParser("|", "\"").Parse("C-F000878815-0000|F000878814|04013000000963||030332030|300|124|0000001|R|2298.91|2298.91|19000228|0401||Y|N||||||0.00030|N|Y|N|Y||||||71||6|N|Y||||N||B||||||");
            Assert.Equal(48, result.Length);
        }
        
        [Fact]
        public void Csv_with_quotes()
        {
            var result = new CsvParser(",", "\"").Parse("\"apple\",\"juice\"");
            Assert.Equal(new[] { "apple", "juice" }, result);
        }

        [Fact]
        public void Csv_with_quotes_and_without_quotes()
        {
            var result = new CsvParser(",", "\"").Parse("\"apple\",juice");
            Assert.Equal(new[] { "apple", "juice" }, result);
        }

        [Fact]
        public void Csv_with_quotes_and_with_comma()
        {
            var result = new CsvParser(",", "\"").Parse("\"apple,pineapple\",\"juice\"");
            Assert.Equal(new[] { "apple,pineapple", "juice" }, result);
        }

        [Fact]
        public void Csv_with_quotes_and_without_quotes_and_with_comma()
        {
            var result = new CsvParser(",", "\"").Parse("\"apple,pineapple\",grape,\"juice\"");
            Assert.Equal(new[] {"apple,pineapple", "grape", "juice"}, result);
        }

        [Fact]
        public void Line_with_pipe_delimiter()
        {
            var result = new CsvParser("|", "\"").Parse("\"apple|pineapple\"|grape|\"juice\"");
            Assert.Equal(new[] { "apple|pineapple", "grape", "juice" }, result);
        }

        [Fact]
        public void Line_with_semicolon_delimiter()
        {
            var result = new CsvParser(";", "\"").Parse("\"apple;pineapple\";grape;\"juice\"");
            Assert.Equal(new[] { "apple;pineapple", "grape", "juice" }, result);
        }

        [Fact]
        public void Csv_with_single_quotes_and_without_quotes_and_with_comma()
        {
            var result = new CsvParser(",", "'").Parse("'apple,pineapple',grape,'juice'");
            Assert.Equal(new[] { "apple,pineapple", "grape", "juice" }, result);
        }

        [Fact]
        public void Csv_with_single_quote_inside()
        {
            var result = new CsvParser(",", "\"").Parse("\"Tom's apple\",Tom's grape");
            Assert.Equal(new[] { "Tom's apple", "Tom's grape" }, result);
        }


        [Fact]
        public void Csv_with_single_quote_inside_and_single_quote_as_a_text_qualifier()
        {
            var result = new CsvParser(",", "'").Parse("'Tom\\'s apple',Tom grape");
            Assert.Equal(new[] { "Tom's apple", "Tom's grape" }, result);
        }
    }
}
