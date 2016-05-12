using System.Drawing;
using Qa.Core.Properties;

namespace Qa.Core.System
{
    public class CommonResources
    {
        public static Image Logo()
        {
            return (Image)Resources.ResourceManager.GetObject("SantanderLogo2");
        }
    }
}
