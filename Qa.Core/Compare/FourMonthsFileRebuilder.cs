namespace Qa.Core.Compare
{
    public class FourMonthsFileRebuilder
    {
        public void Rebuild(ComparePacket packet)
        {
            if (packet.CompareMethod != CompareFilesMethod.FourMonths)
            {
                return;
            }

            //
        }
    }
}
