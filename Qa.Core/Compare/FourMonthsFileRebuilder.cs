using System.Linq;
using Qa.Core.Structure;

namespace Qa.Core.Compare
{
    public class FourMonthsFileRebuilder
    {
        private readonly FourMonthsFileDetector _fileDetector;

        public FourMonthsFileRebuilder()
        {
            _fileDetector = new FourMonthsFileDetector();
        }

        public void Rebuild(ComparePacket packet)
        {
            if (packet.CompareMethod != CompareFilesMethod.FourMonths)
            {
                return;
            }
            var files = packet.Files.Select(x => x.FileName).ToList();
            if (_fileDetector.FindTheCurrent(files) == null)
            {
                return;
            }
            check(packet, 1, "MoM Change", _fileDetector.FindMoM(files) == null);
            check(packet, 2, "YTD Change", _fileDetector.FindYtD(files) == null);
            check(packet, 3, "1 YR Change", _fileDetector.FindYearAgo(files) == null);
        }

        private void check(ComparePacket packet, int index, string name, bool itNeedsInsert)
        {
            if (itNeedsInsert)
            {
                insert(packet, index);
            }            
            packet.Files[index].FileName = name;
        }

        private void insert(ComparePacket packet, int index)
        {
            packet.Files.Insert(index, new FileInformation { Index = index });
            foreach (var groupedField in packet.GroupedFields)
            {
                groupedField.FileValues.Insert(index, new GroupedValuesList());
            }
            foreach (var numberField in packet.NumberFields)
            {
                numberField.Numbers.Insert(index, new CompareNumber(null, null));
            }
        }
    }
}
