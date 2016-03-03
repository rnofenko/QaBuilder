using System.Collections.Generic;

namespace Qa.Excel
{
    public class InternalRow
    {
        
    }

    public class InternalSheet
    {
        private readonly List<InternalRow> _rows;

        public InternalSheet()
        {
            _rows = new List<InternalRow>();
            Pos = new Pos();
        }

        public Pos Pos { get; private set; }

        public int X
        {
            get { return Pos.X; }
            set { Pos.X = value; }
        }

        public int Y
        {
            get { return Pos.Y; }
            set { Pos.Y = value; }
        }

        public void Set(params string[] values)
        {
            SetWithShiftY(0, values);
        }

        public void SetWithShiftY(int shiftY, params string[] values)
        {
            var row = getRow(Y + shiftY);
        }

        private InternalRow getRow(int y)
        {
            while (_rows.Count<=y)
            {
                _rows.Add(new InternalRow());
            }
            return _rows[y];
        }
    }
}
