using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace Networks2020
{
    class Node
    {
        public String szOriginalId = "";
        public int numId = 0;
        public double value = 0;
        public double size = 0;
        public String szColor = "#574569";
        public int numCollectionDates = 0;


        public Node(String szIdS)
        {
            szOriginalId = szIdS;
            numId = Convert.ToInt16(Regex.Match(szOriginalId, @"\d+\.*\d*").Value);
            numId = Convert.ToInt16(isTeacher(szOriginalId) ? 200 + numId : isLab(szOriginalId) ? 300 + numId : numId);
        }

        public Boolean isTeacher(String szid1)
        {
            int id1 = Convert.ToInt16(Regex.Match(szid1, @"\d+\.*\d*").Value);
            return szid1.Contains("TEACHER") || szid1.Contains("T" + id1.ToString());
        }
        public Boolean isLab(String szid1)
        {
            int id1 = Convert.ToInt16(Regex.Match(szid1, @"\d+\.*\d*").Value);
            return szid1.Contains("LAB") || szid1.Contains("L" + id1.ToString());
        }
        public String toXml()
        {
            return "node\n\t[\n\t\troot_index\t-" + numId + "\n\t\tid\t-" + numId
                              + "\n\t\tgraphics\n\t\t[\n\t\t\tx\t" + 0 + "\n\t\t\ty\t" + 0
                              + "\n\t\t\th\t" + size + "\n\t\t\tw\t" + size + "\n\t\t\tfill\t\""
                              + szColor + "\"\n\t\t\ttype\t\"ellipse\"\n\t\t\toutline\t\"" + "#000000"
                              + "\"\n\t\t\toutline_width\t" + "2.0" + "\n\t\t]\n\t\tlabel\t\""
                              + szOriginalId + "\"\n\t]";

        }
        public String toXml(double nodeMult)
        {
            return "node\n\t[\n\t\troot_index\t-" + numId + "\n\t\tid\t-" + numId
                              + "\n\t\tgraphics\n\t\t[\n\t\t\tx\t" + 0 + "\n\t\t\ty\t" + 0
                              + "\n\t\t\th\t" + (size * nodeMult) + "\n\t\t\tw\t" + (size * nodeMult) + "\n\t\t\tfill\t\""
                              + szColor + "\"\n\t\t\ttype\t\"ellipse\"\n\t\t\toutline\t\"" + "#000000"
                              + "\"\n\t\t\toutline_width\t" + "2.0" + "\n\t\t]\n\t\tlabel\t\""
                              + szOriginalId + "\"\n\t]";

        }
    }
}
