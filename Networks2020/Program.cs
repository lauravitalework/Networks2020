using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Networks2020
{
    class Program
    {

        public static int subjectIdColumn=3;
        public static int partnerIdColumn =4;
        public static int partnerStatusColumn =12;
        public static int subjectStatusColumn =13;
        public static int subjectTypeColumn =14;
        public static int partnerTypeColumn =15;
        public static int dateColumn =0;
        public static Dictionary<string, string> colorAssignments;
         

        static void fromXmlToCsv(String szFileName)
        {
            using (TextWriter csvNetwork = new StreamWriter(szFileName.Replace(".xml", "xml.csv")))
            {
                csvNetwork.WriteLine("TYPE,NAME,NODE1,NODE2,VALUE");
                using (StreamReader sr = new StreamReader(szFileName))
                {//"E://CLASSROOMS_AS_OF_FEB2019//LADYBUGS1//SYNC/PAIRACTIVITY_0_LADYBUGS1_3_3_2017_4_2_2019_419449978_nodect_edgect_v2.gml"
                 //  "E://CLASSROOMS_AS_OF_FEB2019//LADYBUGS1//SYNC/PAIRACTIVITY_0_LADYBUGS1_3_3_20174_2_2019_419449978_nodect_edgect_v2.gml"
                    string[] splitNode = new string[] { "node" };
                    string[] splitEdge = new string[] { "edge" };
                    String[] line = sr.ReadToEnd().Split(splitNode, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 1; i < line.Length - 1; i++)
                    {
                        int label_index_start = line[i].IndexOf("label");
                        String label = line[i].Substring(label_index_start + 5).Trim();
                        label = label.Substring(0, label.Length - 1).Trim();
                        label = label.Substring(1, label.Length - 2);
                        int size_index_start = line[i].IndexOf("w");
                        int fill_index_start = line[i].IndexOf("fill");
                        String size = line[i].Substring(size_index_start + 1, fill_index_start - size_index_start - 1).Trim();
                        csvNetwork.WriteLine("NODE," + label + "," + label + ",," + size);

                    }
                    String rest = line[line.Length - 1];
                    String[] line2 = rest.Split(splitEdge, StringSplitOptions.RemoveEmptyEntries);
                    //Console.WriteLine(line2[0]);

                    for (int i = 1; i < line2.Length; i++)
                    {
                        //rawEdges.Add(line2[i]);
                        int source_index_start = line2[i].IndexOf("source");
                        int target_index_start = line2[i].IndexOf("target");
                        int target_index_end = line2[i].IndexOf("label");
                        int width_index_start = line2[i].IndexOf("width");
                        String source = line2[i].Substring(source_index_start + 6, target_index_start - source_index_start - 7).Trim();
                        String target = line2[i].Substring(target_index_start + 6, target_index_end - target_index_start - 6).Trim();
                        String width = line2[i].Substring(width_index_start + 5).Trim();
                        width = width.Split(']')[0];
                        width = width.Split(']')[0];

                        csvNetwork.WriteLine("EDGE," + source + "|" + target + "," + source + "," + target + "," + width);
                    }
                }

            }

        }
        static void getPride1819_LEAPAM()
        {
            getPride1819_LEAPAM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, 42);
        }
        static void getPride1819_LEAPPM()
        {
            getPride1819_LEAPPM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, 42);
        }
        static void getPride1819_REVMAM()
        {
            getPride1819_REVMAM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, 42);
        }

        static void getPride1819_LEAPAM(Boolean prune, Boolean includeTeachers, Boolean scale, double nodeMult, double edgeMult, List<int> colsNodes, List<int> colsEdges, List<int> colsNodesT, List<int> colsEdgesT, int divCol)
        {
        
        

        Network pride1819Network = new Network(//dates,
                //"G://CLASSROOMS_OLD//PRIDE_LEAP//PRIDE_LEAP_AM//SYNC//PAIRACTIVITY_ALL_5PRIDE_LEAP_AM_5_31_2020_1136973128ALL.CSV",
                "G://CLASSROOMS_OLD//PRIDE_LEAP//PRIDE_LEAP_AM//SYNCT1HACK//PAIRACTIVITY_ALL_5PRIDE_LEAP_AM_7_2_2020_779521743ALL.CSV",
                "G://CLASSROOMS_OLD//PRIDE_LEAP//PRIDE_LEAP_AM//MAPPING_PRIDE_LEAP_AM_BASE.CSV",
                 colsNodes.Count>0? colsNodes: new List<int>() { 27, 28 },//19 },//27, 28 },//19},   // 19 PairBlockTalking   26  26
                 colsEdges.Count > 0 ? colsEdges : new List<int>() { 27, 28 },// 19 },//27, 28 },//19},
                 colsNodesT.Count > 0 ? colsNodesT : new List<int>() { 28 },
                 colsEdgesT.Count > 0 ? colsEdgesT : new List<int>() { 28 }, 
                 subjectIdColumn,
                 partnerIdColumn,
                 partnerStatusColumn,
                 subjectStatusColumn,
                 subjectTypeColumn,
                 partnerTypeColumn,
                 dateColumn,
                 divCol,
                 "PRIDE_LEAP_AM",
                 "1819",
                 colorAssignments,
                 includeTeachers);
            pride1819Network.setMappings();
            pride1819Network.fromPairActivityFile();
            pride1819Network.toXMLandCSV(nodeMult, edgeMult, true); //(1500, 120, true);
        }
        static void getPride1819_LEAPPM(Boolean prune, Boolean includeTeachers, Boolean scale, double nodeMult, double edgeMult, List<int> colsNodes, List<int> colsEdges, List<int> colsNodesT, List<int> colsEdgesT, int divCol)
        {
          
            Network pride1819Network = new Network(//dates,
              "G://CLASSROOMS_OLD//PRIDE_LEAP//PRIDE_LEAP_PM//SYNC//PAIRACTIVITY_ALL_5PRIDE_LEAP_PM_9_9_2019_1340573660ALL.CSV",
              "G://CLASSROOMS_OLD//PRIDE_LEAP//PRIDE_LEAP_PM//MAPPING_PRIDE_LEAP_PM_BASE.CSV",
               colsNodes.Count > 0 ? colsNodes : new List<int>() { 19 },// 26, 27 },//19},   // 19 PairBlockTalking   26  26
               colsEdges.Count > 0 ? colsEdges : new List<int>() { 19 }, //26, 27 },//19},
               colsNodesT.Count > 0 ? colsNodesT : new List<int>() { 28 },
                 colsEdgesT.Count > 0 ? colsEdgesT : new List<int>() { 28 },
                 subjectIdColumn,
                 partnerIdColumn,
                 partnerStatusColumn,
                 subjectStatusColumn,
                 subjectTypeColumn,
                 partnerTypeColumn,
                 dateColumn,
                 divCol,
               "PRIDE_LEAP_PM",
               "1819",
               colorAssignments,
                 includeTeachers);
            pride1819Network.setMappings();
            pride1819Network.fromPairActivityFile();
            pride1819Network.toXMLandCSV(nodeMult, edgeMult, true); //1500 * 1.666, 120, true);// pride1819Network.toXMLandCSV(1500, 120, true);*1.666


        }
        static void getPride1819_REVMAM(Boolean prune, Boolean includeTeachers, Boolean scale, double nodeMult, double edgeMult, List<int> colsNodes, List<int> colsEdges, List<int> colsNodesT, List<int> colsEdgesT, int divCol)
        {
             
            Network pride1819Network = new Network(//dates,
                "G://CLASSROOMS_OLD//PRIDE_REVM//PRIDE_REVM_AM//SYNC//PAIRACTIVITY_ALL_5PRIDE_REVM_AM_5_19_2020_1455294013ALL.CSV",
                "G://CLASSROOMS_OLD//PRIDE_REVM//PRIDE_REVM_AM//MAPPING_PRIDE_REVM_AM_BASE.CSV",
                 colsNodes.Count > 0 ? colsNodes : new List<int>() { 27, 28 }, //27, 28 },//19},   // 19 PairBlockTalking   26  26
                 colsEdges.Count > 0 ? colsEdges : new List<int>() { 27, 28 },// 27, 28 },//19},
                 colsNodesT.Count > 0 ? colsNodesT : new List<int>() { 28 },
                 colsEdgesT.Count > 0 ? colsEdgesT : new List<int>() { 28 },
                 subjectIdColumn,
                 partnerIdColumn,
                 partnerStatusColumn,
                 subjectStatusColumn,
                 subjectTypeColumn,
                 partnerTypeColumn,
                 dateColumn,
                 divCol,
                 "PRIDE_REVM_AM",
                 "1819",
                 colorAssignments,
                 includeTeachers);
            pride1819Network.setMappings();
            pride1819Network.fromPairActivityFile();
            pride1819Network.toXMLandCSV(nodeMult, edgeMult, true);
        }
        static void getPride1819()
        {
            getPride1819(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, 42);
        }
        static void getPride1819(Boolean prune, Boolean includeTeachers, Boolean scale, double nodeMult, double edgeMult, List<int> colsNodes, List<int> colsEdges, List<int> colsNodesT, List<int> colsEdgesT, int divCol)
        {
            getPride1819_LEAPAM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, divCol);
            getPride1819_LEAPPM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, divCol);
            getPride1819_REVMAM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, divCol);
        }
        static void getPride1920_LEAPAM()
        {
             getPride1920_LEAPAM(true, false, false, 2500, 350, new List<int> { 26, 27 }, new List<int> { 26, 27 }, new List<int> { }, new List<int> { }, 42);
        }
        static void getPride1920_LEAPPM()
        {
            getPride1920_LEAPPM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, 42);
        }
        static void getPride1920_REVMAM()
        {
            getPride1920_REVMAM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, 42);
        }
        static void getPride1920_LEAPAM(Boolean prune, Boolean includeTeachers, Boolean scale, double nodeMult, double edgeMult, List<int> colsNodes, List<int> colsEdges, List<int> colsNodesT, List<int> colsEdgesT, int divCol)
        {
           
            Network prideNetwork = new Network(//dates,
                "G://CLASSROOMS1920//LEAP_1920//LEAP_AM_1920//SYNC//PAIRACTIVITY_ALL_5LEAP_AM_1920_4_17_2020_1173190353ALL.CSV",
                "G://CLASSROOMS1920//LEAP_1920//LEAP_AM_1920//MAPPING_LEAP_AM_1920_BASE.CSV",
                 colsNodes.Count > 0 ? colsNodes : new List<int>() { 26, 27 },// 26, 27 },   // 18 },//26,27 },   // 18 PairBlockTalking   26  26
                 colsEdges.Count > 0 ? colsEdges : new List<int>() { 26, 27 },//26, 27 },   //18 },//26, 27 },
                 colsNodesT.Count > 0 ? colsNodesT : new List<int>() { 28 },
                 colsEdgesT.Count > 0 ? colsEdgesT : new List<int>() { 28 },
                 subjectIdColumn,
                 partnerIdColumn,
                 partnerStatusColumn,
                 subjectStatusColumn,
                 subjectTypeColumn,
                 partnerTypeColumn,
                 dateColumn,
                 divCol,
                 "LEAP_AM_1920",
                 "1920",
                 colorAssignments,
                 includeTeachers);
            

             
            prideNetwork.setMappings();
            prideNetwork.fromPairActivityFile();
            prideNetwork.toXMLandCSV(nodeMult, edgeMult, true); //1500*1.666, 120, true); //prideNetwork.toXMLandCSV(1500, 120, true);
        }
        static void getPride1920_LEAPPM(Boolean prune, Boolean includeTeachers, Boolean scale, double nodeMult, double edgeMult, List<int> colsNodes, List<int> colsEdges, List<int> colsNodesT, List<int> colsEdgesT, int divCol)
        {
            //PAIRACTIVITY_ALL_5LEAP_PM_1920_7_1_2020_1686857863ALL   PAIRACTIVITY_ALL_4LEAP_PM_1920_4_17_2020_1346184507ALL
            Network prideNetwork = new Network(//dates,
                "G://CLASSROOMS1920//LEAP_1920//LEAP_PM_1920//SYNC//PAIRACTIVITY_ALL_5LEAP_PM_1920_7_1_2020_1686857863ALL.CSV",
                "G://CLASSROOMS1920//LEAP_1920//LEAP_PM_1920//MAPPING_LEAP_PM_1920_BASE.CSV",
                 colsNodes.Count > 0 ? colsNodes : new List<int>() { 26, 27 },   // 18 },//26,27 },   // 18 PairBlockTalking   26  26
                 colsEdges.Count > 0 ? colsEdges : new List<int>() { 26, 27 },   //18 },//26, 27 },
                 colsNodesT.Count > 0 ? colsNodesT : new List<int>() { 28 },
                 colsEdgesT.Count > 0 ? colsEdgesT : new List<int>() { 28 },
                 subjectIdColumn,
                 partnerIdColumn,
                 partnerStatusColumn,
                 subjectStatusColumn,
                 subjectTypeColumn,
                 partnerTypeColumn,
                 dateColumn,
                 divCol,
                 "LEAP_PM_1920",
                 "1920",
                 colorAssignments,
                 includeTeachers);
            prideNetwork.setMappings();
            prideNetwork.fromPairActivityFile();
            prideNetwork.toXMLandCSV(nodeMult, edgeMult, true); //1500 * 1.666, 120, true); ////prideNetwork.toXMLandCSV(1500, 120, true);


        }
        static void getPride1920_REVMAM(Boolean prune, Boolean includeTeachers, Boolean scale, double nodeMult, double edgeMult, List<int> colsNodes, List<int> colsEdges, List<int> colsNodesT, List<int> colsEdgesT, int divCol)
        {
            Network prideNetwork = new Network(//dates,
                "G://CLASSROOMS1920//REVM_1920//REVM_AM_1920//SYNC//PAIRACTIVITY_ALL_4REVM_AM_1920_6_24_2020_590519601ALL.CSV",
                "G://CLASSROOMS1920//REVM_1920//REVM_AM_1920//MAPPING_REVM_AM_1920_BASE.CSV",
                 colsNodes.Count > 0 ? colsNodes : new List<int>() { 27, 28 },   // 18 },//26,27 },   // 18 PairBlockTalking   26  26
                 colsEdges.Count > 0 ? colsEdges : new List<int>() { 27, 28 },   //18 },//26, 27 },
                 colsNodesT.Count > 0 ? colsNodesT : new List<int>() { 28 },
                 colsEdgesT.Count > 0 ? colsEdgesT : new List<int>() { 28 },
                 subjectIdColumn,
                 partnerIdColumn,
                 partnerStatusColumn,
                 subjectStatusColumn,
                 subjectTypeColumn,
                 partnerTypeColumn,
                 dateColumn,
                 divCol,
                 "REVM_AM_1920",
                 "1920",
                 colorAssignments,
                 includeTeachers);

            prideNetwork.setMappings();
            prideNetwork.fromPairActivityFile();
            prideNetwork.toXMLandCSV(nodeMult, edgeMult, true);
        }
        static void getPride1920(Boolean prune, Boolean includeTeachers, Boolean scale, double nodeMult, double edgeMult, List<int> colsNodes, List<int> colsEdges, List<int> colsNodesT, List<int> colsEdgesT, int divCol)
        {
            getPride1920_LEAPAM(true, false, false, 2500, 350, new List<int> { 27, 26 }, new List<int> { 27, 26 }, new List<int> { }, new List<int> { }, divCol);
            getPride1920_LEAPPM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, divCol);
            getPride1920_REVMAM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, divCol);


        }
        static void Main(string[] args)
        {
            colorAssignments = new Dictionary<string, string>();
            colorAssignments.Add("TYPICAL", "#ff3030");
            colorAssignments.Add("TD", "#ff3030");
            colorAssignments.Add("DD", "#4485F4");
            colorAssignments.Add("ASD", "#4485F4");//"#0000ff");
            colorAssignments.Add("AUTISM", "#4485F4");//"#0000ff");
            colorAssignments.Add("DELAY", "#ffff00");
            colorAssignments.Add("OTHERS", "#ffff00");

            getPride1819_LEAPAM(true, true, true, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { 29 }, new List<int> { 29 }, 42);
            getPride1920_LEAPAM(true, true, true, 2500, 350, new List<int> { 26, 27 }, new List<int> { 26, 27 }, new List<int> { 28 }, new List<int> { 28 }, 41);

            getPride1920_LEAPPM(true, false, false, 1500, 120, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, 42);

             
            getPride1819_LEAPAM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> {}, new List<int> {}, 42);
            getPride1819_LEAPPM(true, false, false, 2500, 350, new List<int> { 26, 27 }, new List<int> { 26, 27 }, new List<int> { }, new List<int> { }, 41);
            getPride1819_REVMAM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, 42);

            getPride1920_LEAPAM(true, false, false, 2500, 350, new List<int> { 26, 27 }, new List<int> { 26, 27 }, new List<int> {}, new List<int> {}, 41);
            getPride1920_LEAPPM(true, false, false, 2500, 350, new List<int> { 26, 27 }, new List<int> { 26, 27 }, new List<int> { }, new List<int> { }, 41);
            getPride1920_REVMAM(true, false, false, 2500, 350, new List<int> { 27, 28 }, new List<int> { 27, 28 }, new List<int> { }, new List<int> { }, 42);

        }
    }
}
