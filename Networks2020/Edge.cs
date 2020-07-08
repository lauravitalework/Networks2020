using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace Networks2020
{
    class Edge
    {
        public String szId = "";
        public String szId1 = "";
        public String szId2 = "";

        public int numId1 = 0;
        public int numId2 = 0;

        public double value = 0;
        public double size = 0;

        public int numCollectionDates = 0;
        public bool isEdgeWithTeacher = false;

        public Edge(String szid1, String szid2)
        {
            // Regex.Match("anything 876.8 anything", @"\d+\.*\d*").Value
            int id1 = Convert.ToInt16(Regex.Match(szid1, @"\d+\.*\d*").Value);
            int id2 = Convert.ToInt16(Regex.Match(szid2, @"\d+\.*\d*").Value);


            Boolean isLab1 = (szid1.Contains("LAB") || szid1.Contains("L" + id1.ToString()));
            Boolean isTeacher1 = (szid1.Contains("TEACHER") || szid1.Contains("T" + id1.ToString()));
            Boolean isLab2 = (szid2.Contains("LAB") || szid2.Contains("L" + id2.ToString()));
            Boolean isTeacher2 = (szid2.Contains("TEACHER") || szid2.Contains("T" + id2.ToString()));
            isEdgeWithTeacher = isTeacher1 || isTeacher2;

            if (id1 != id2)
            {
                if (id1 < id2)
                {
                    szId1 = szid1;
                    szId2 = szid2;
                    numId1 = isTeacher1 ? 200 + id1 : isLab1 ? 300 + id1 : id1;
                    numId2 = id2;
                }
                else
                {
                    szId1 = szid2;
                    szId2 = szid1;
                    numId1 = isTeacher2 ? 200 + id2 : isLab2 ? 300 + id2 : id2; ;
                    numId2 = isTeacher1 ? 200 + id1 : isLab1 ? 300 + id1 : id1;
                }
            }
            else
            {
                if ((!isLab1) &&
                    (!isTeacher1))
                {
                    szId1 = szid1;
                    szId2 = szid2;

                    numId1 = isTeacher1 ? 200 + id1 : isLab1 ? 300 + id1 : id1;
                    numId2 = isTeacher2 ? 200 + id2 : isLab2 ? 300 + id2 : id2; ;
                }
                else if ((!isLab2) &&
                    (!isTeacher2))
                {
                    szId1 = szid2;
                    szId2 = szid1;

                    numId1 = isTeacher2 ? 200 + id2 : isLab2 ? 300 + id2 : id2; ;
                    numId2 = isTeacher1 ? 200 + id1 : isLab1 ? 300 + id1 : id1;
                }
                else if (isTeacher1)
                {
                    szId1 = szid1;
                    szId2 = szid2;

                    numId1 = isTeacher1 ? 200 + id1 : isLab1 ? 300 + id1 : id1;
                    numId2 = isTeacher2 ? 200 + id2 : isLab2 ? 300 + id2 : id2;
                }
                else
                {
                    szId1 = szid2;
                    szId2 = szid1;

                    numId1 = isTeacher2 ? 200 + id2 : isLab2 ? 300 + id2 : id2;
                    numId2 = isTeacher1 ? 200 + id1 : isLab1 ? 300 + id1 : id1;
                }


            }

            szId = szId1 + "|" + szId2;
        }
        public String toXml()
        {
            return "edge\n\t[\n\t\tsource\t-" + numId1 + "\n\t\ttarget\t-" + numId2 + "\n\t\tlabel\t\" \"\n\t\tgraphics [\n\t\t\twidth\t"
                + size + "\n\t\t]\n\t]";

        }
        public String toXml(double edgeMult)
        {
            return "edge\n\t[\n\t\tsource\t-" + numId1 + "\n\t\ttarget\t-" + numId2 + "\n\t\tlabel\t\" \"\n\t\tgraphics [\n\t\t\twidth\t"
                + (size * edgeMult) + "\n\t\t]\n\t]";

        }
    }
}
