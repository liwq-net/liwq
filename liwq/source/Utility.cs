using System;
using System.Xml.Linq;

namespace liwq
{
    public static class Utility
    {
        static Utility()
        {
            Random = new Random();
        }

        //---------------------------------------------------------------------

        public static Random Random { get; private set; }

        //---------------------------------------------------------------------

        public static string SafeReadString(this XElement element, string name)
        {
            XAttribute attribe = element.Attribute(name);
            if (attribe == null) return "";
            else return attribe.Value;
        }
        public static int SafeReadInt(this XElement element, string name)
        {
            XAttribute attribe = element.Attribute(name);
            if (attribe == null) return 0;
            else return int.Parse(attribe.Value);
        }

        //---------------------------------------------------------------------

    }
}
