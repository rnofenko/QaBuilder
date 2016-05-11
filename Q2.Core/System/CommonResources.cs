using System.Drawing;
using Q2.Core.Properties;

namespace Q2.Core.System
{
    public class CommonResources
    {
        public static Image Logo()
        {
            return (Image)Resources.ResourceManager.GetObject("SantanderLogo2");
        }
    }
}
