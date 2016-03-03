namespace Qa.Core.Excel
{
    public struct Pos
    {
        public int Row;

        public int Column;

        public Pos Clone()
        {
            return new Pos {Row = Row, Column = Column};
        }
    }
}