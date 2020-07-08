using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
namespace Networks2020
{
    class Network
    {
        //public List<DateTime> collectionDates = new List<DateTime>();
        public String szSourceFileName = "";
        public String szMappingFileName = "";
        public Boolean includeLabs = false;//NOT SUPPORTED YET
        public Boolean includeTeachers = false;

        public Boolean scaleTeachersApart = false;
        public Boolean scaleNodesAndEdges = false;
        public double scaleNodeValue = 380;
        public double scaleEdgeValue = 5;


        public List<int> nodeColumns = new List<int>();
        public List<int> edgeColumns = new List<int>();
        public List<int> nodeColumnsT = new List<int>();
        public List<int> edgeColumnsT = new List<int>();

        public int subjectIdColumn = 0;
        public int partnerIdColumn = 0;
        public int subjectStatusColumn = 0;
        public int partnerStatusColumn = 0;
        public int subjectTypeColumn = 14;
        public int partnerTypeColumn = 15;
        public int dateColumn = 0;
        public int divColumn = -1;
        public List<int> mapIdsColumn = new List<int>() { 3, 18 };
        public int mapTypeColumn = 17;
        public int mapDiagnosisColumn = 14;
        public int mapGenderColumn = 15;
        Dictionary<string, string> colorAssignments;
        String szColorMapType = "DIAGNOSIS";
        public String szDayVersion = (DateTime.Now.Month < 10 ? "0" : "") + DateTime.Now.Month + (DateTime.Now.Day < 10 ? "0" : "") + DateTime.Now.Day + DateTime.Now.Year + new Random().Next();
        public String szVersion = (DateTime.Now.Month<10?"0":"")+DateTime.Now.Month + (DateTime.Now.Day < 10 ? "0" : "") + DateTime.Now.Day +  DateTime.Now.Year + new Random().Next();
        public String className = "";
        public String classYear = "";
        public Dictionary<DateTime, DayNetwork> dayNetworks = new Dictionary<DateTime, DayNetwork>();

        Dictionary<string, string> subjectsColorAssignments = new Dictionary<string, string>();

        Dictionary<String, Node> nodes = new Dictionary<string, Node>();
        Dictionary<String, Edge> edges = new Dictionary<string, Edge>();


        public Network(//String[] datesI, 
         String szSourceFileNameI,
         String szMappingFileNameI,
         List<int> nodeColumnsI,
         List<int> edgeColumnsI,
         int subjectIdColumnI,
         int partnerIdColumnI, 
         int partnerStatusColumnI,
         int subjectStatusColumnI,
         int subjectTypeColumnI,
         int partnerTypeColumnI,
         int dateColumnI,
         int divColumnI,
         String classNameI,
         String classYearI,
         Dictionary<string, string>  colorAssignmentsI)
         { 

            szSourceFileName = szSourceFileNameI;
            szMappingFileName=szMappingFileNameI;
            nodeColumns = nodeColumnsI;
            edgeColumns = edgeColumnsI;
         
            subjectIdColumn = subjectIdColumnI;
            partnerIdColumn = partnerIdColumnI;
            dateColumn = dateColumnI;
            divColumn = divColumnI;
            className = classNameI;
            classYear = classYearI;
            colorAssignments = colorAssignmentsI;
            partnerStatusColumn = partnerStatusColumnI;
            subjectStatusColumn = subjectStatusColumnI;
            subjectTypeColumn = subjectTypeColumnI;
            partnerTypeColumn = partnerTypeColumnI;
        }
        public Network(//String[] datesI, 
         String szSourceFileNameI,
         String szMappingFileNameI,
         List<int> nodeColumnsI,
         List<int> edgeColumnsI,
         List<int> nodeColumnsTI,
         List<int> edgeColumnsTI,
         int subjectIdColumnI,
         int partnerIdColumnI,
         int partnerStatusColumnI,
         int subjectStatusColumnI,
         int subjectTypeColumnI,
         int partnerTypeColumnI,
         int dateColumnI,
         int divColumnI,
         String classNameI,
         String classYearI,
         Dictionary<string, string> colorAssignmentsI,
         Boolean includeTeachersI)
         {
            includeTeachers = includeTeachersI;
            if (includeTeachers)
            {
                scaleNodesAndEdges = true;
                scaleTeachersApart = true;
                scaleTeachersApart = true;
            }
            szSourceFileName = szSourceFileNameI;
            szMappingFileName = szMappingFileNameI;
            nodeColumns = nodeColumnsI;
            edgeColumns = edgeColumnsI;
            nodeColumnsT = nodeColumnsTI;
            edgeColumnsT = edgeColumnsTI;
            subjectIdColumn = subjectIdColumnI;
            partnerIdColumn = partnerIdColumnI;
            dateColumn = dateColumnI;
            divColumn = divColumnI;
            className = classNameI;
            classYear = classYearI;
            colorAssignments = colorAssignmentsI;
            partnerStatusColumn = partnerStatusColumnI;
            subjectStatusColumn = subjectStatusColumnI;
            subjectTypeColumn = subjectTypeColumnI;
            partnerTypeColumn = partnerTypeColumnI;
        }
        public Network(String[] dates)
        {

        }
        public void setMappings()
        {
            String szTeacherColor = "#4daf4a";// "#ffffff"; //white
            String szLabColor = "#4daf4a";// "#ffffff"; //white

            using (StreamReader sr = new StreamReader(szMappingFileName))
            {
                if (!sr.EndOfStream)
                {
                    sr.ReadLine();
                }
                while (!sr.EndOfStream)
                {
                    String[] line = sr.ReadLine().Split(',');

                    String szShortId = "";
                    foreach(int mapId in mapIdsColumn )
                    {
                        szShortId = szShortId.Length == 0 ? line[mapId].Trim().ToUpper() : szShortId.Length > line[mapId].Trim().Length ? line[mapId].Trim().ToUpper() : szShortId;
                    }

                    String szNumId = Regex.Match(szShortId, @"\d+\.*\d*").Value;

                    if (szShortId.Contains("T" + szNumId) || szShortId.Contains("TEACHER"))
                        subjectsColorAssignments.Add(szShortId, szTeacherColor);
                    else if (szShortId.Contains("L" + szNumId) || szShortId.Contains("LAB"))
                        subjectsColorAssignments.Add(szShortId, szLabColor);
                    else if (szColorMapType == "DIAGNOSIS")
                    {
                        if (!subjectsColorAssignments.ContainsKey(szShortId))
                        {
                            if (colorAssignments.ContainsKey(line[mapDiagnosisColumn].Trim().ToUpper()))
                                subjectsColorAssignments.Add(szShortId, colorAssignments[line[mapDiagnosisColumn].Trim().ToUpper()]);
                            else if (colorAssignments.ContainsKey("OTHERS"))
                                subjectsColorAssignments.Add(szShortId, colorAssignments["OTHERS"]);
                            else
                                subjectsColorAssignments.Add(szShortId, "ORANGE");
                        }
                    }
                    else if (szColorMapType == "GENDER")
                    {

                        if (!subjectsColorAssignments.ContainsKey(szShortId))
                        {
                            if (colorAssignments.ContainsKey(line[mapGenderColumn].Trim().ToUpper()))
                                subjectsColorAssignments.Add(szShortId, colorAssignments[line[mapGenderColumn].Trim().ToUpper()]);
                            else
                                subjectsColorAssignments.Add(szShortId, "ORANGE");
                        }
                    }   


                }
            }
        }
        public void fromPairActivityFile()
        {
            using (StreamReader sr = new StreamReader(szSourceFileName))
            {
                String[] line;
                if (!sr.EndOfStream)
                {
                    line = sr.ReadLine().Split(',');
                    String szVersionType = "NODE";
                    foreach (int mapId in nodeColumns)
                    {
                        szVersionType += line[mapId].Trim().Replace(" ", "");
                    }
                    szVersionType += "_EDGE";
                    foreach (int mapId in edgeColumns)
                    {
                        szVersionType += line[mapId].Trim().Replace(" ", "");
                    }

                    if (divColumn >= 0)
                    {
                        szVersionType += ("_DIV"+line[divColumn].Trim().Replace(" ", ""));
                        
                    }
                    szVersion = szVersionType+"_" + szVersion;
                    sr.ReadLine();
                }
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine().Split(',');

                    String szSubjectStatus = line[subjectStatusColumn].Trim().ToUpper();
                    String szPartnerStatus = line[partnerStatusColumn].Trim().ToUpper();

                    String szSubjectType = line[subjectTypeColumn].Trim().ToUpper();
                    String szPartnerType = line[partnerTypeColumn].Trim().ToUpper();

                    Boolean include = szSubjectStatus == "PRESENT" && szPartnerStatus == "PRESENT";
                    String szShortIdS = line[subjectIdColumn].Trim().ToUpper();
                    String szShortIdP = line[partnerIdColumn].Trim().ToUpper();
                    if (include)
                    {
                        if (szSubjectType.StartsWith("C") && szPartnerType.StartsWith("C"))
                        {
                            include = true;
                        }
                        else if (includeTeachers && szSubjectType.StartsWith("C") && szPartnerType.StartsWith("T"))
                        {
                            if(!szShortIdP.Contains("T1"))//t1 hack delete debug
                            include = true;
                            else
                                include = false;
                        }
                        else
                            include = false;
                    }

                    if (include)
                    {
                        
                        DateTime collectionDate = Convert.ToDateTime(line[dateColumn].Trim());
                        if (!dayNetworks.ContainsKey(collectionDate))
                        {
                            dayNetworks.Add(collectionDate, new DayNetwork());
                        }
                        DayNetwork dayNetwork = dayNetworks[collectionDate];

                        String szShortId = szPartnerType.StartsWith("C")? szShortIdS : szShortIdP;
                        Node node = new Node(szShortId);

                        if (!dayNetwork.nodes.ContainsKey(szShortId))
                        {
                            if (subjectsColorAssignments.ContainsKey(node.szOriginalId))
                            {
                                node.szColor = subjectsColorAssignments[node.szOriginalId];
                            }
                            dayNetwork.nodes.Add(szShortId, node);
                        }
                        node = dayNetwork.nodes[szShortId];
                         
                        double nodeValue = 0;
                        List<int> nodeCols = szPartnerType.StartsWith("C") ? nodeColumns : nodeColumnsT;
                        foreach (int mapId in nodeCols)
                        {
                            double thisValue = 0;
                            if (Double.TryParse(line[mapId].Trim(), out thisValue))
                                nodeValue += thisValue;
                        }
                        if (divColumn >= 0)
                        {
                            double thisValue = 0;
                            if (Double.TryParse(line[divColumn].Trim(), out thisValue))
                                nodeValue = nodeValue / thisValue;
                        }

                        node.size += nodeValue;
                        node.value += nodeValue;



                        Edge edge = new Edge(szShortIdS, szShortIdP);
                        if (!dayNetwork.edges.ContainsKey(edge.szId))
                        {

                            double edgeValue = 0;
                            List<int> edgeCols = szPartnerType.StartsWith("C") ? edgeColumns : edgeColumnsT;
                            foreach (int mapId in edgeCols)
                            {
                                double thisValue = 0;
                                if (Double.TryParse(line[mapId].Trim(), out thisValue))
                                    edgeValue += thisValue;
                            }
                            if (divColumn >= 0)
                            {
                                double thisValue = 0;
                                if (Double.TryParse(line[divColumn].Trim(), out thisValue))
                                    edgeValue = edgeValue / thisValue;
                            }
                            edge.size = edgeValue;
                            edge.value = edgeValue;
                            dayNetwork.edges.Add(edge.szId, edge);
                        }
                    }
                }
            }

        }

        public double getEdgeMult(double dayEdgeMaxValue, double dayChildEdgeMaxValue, double dayTeaherEdgeMaxValue, Edge edge, double edgeMult)
        {

            double res = edgeMult;
            if (scaleNodesAndEdges)
            {
                double maxEdgeVal = 0;
                if (includeTeachers && scaleTeachersApart)
                {
                    if (edge.isEdgeWithTeacher)
                    {
                        maxEdgeVal = dayTeaherEdgeMaxValue;
                    }
                    else
                    {
                        maxEdgeVal = dayChildEdgeMaxValue;
                    }
                }
                else
                {
                    maxEdgeVal = dayEdgeMaxValue;
                }


                 


                res = Math.Round((edge.value * scaleNodeValue) / maxEdgeVal,8);
                double res2  = Math.Round((scaleNodeValue / maxEdgeVal)* edge.value, 8);
                if (res!=res2)
                {
                    bool stop = true;
                }
                res = (scaleEdgeValue / maxEdgeVal);
            }

            return res;

        }

        public double getNodeMult(double dayNodeMaxValue, double dayChildNodeMaxValue, double dayTeaherNodeMaxValue, Node node, double nodeMult)
        {
 
            double res = nodeMult;
            if(scaleNodesAndEdges)
            {
                double maxNodeVal = 0;
                if (includeTeachers && scaleTeachersApart)
                {
                    if(node.isTeacher(node.szOriginalId))
                    {
                        maxNodeVal = dayTeaherNodeMaxValue;
                    }
                    else
                    {
                        maxNodeVal = dayChildNodeMaxValue;
                    }
                }
                else
                {
                    maxNodeVal = dayNodeMaxValue;
                }
                res = (node.value * scaleNodeValue) / maxNodeVal;
                if(Math.Round(res,8) != Math.Round(((scaleNodeValue / maxNodeVal)* node.value),8))
                {
                    bool stop = true;
                }
                res = (scaleNodeValue / maxNodeVal);
            }

            return res;

        }

        public void toXMLandCSV(double nodeMult, double edgeMult, bool prune)
        {

            nodes = new Dictionary<string, Node>();
            edges = new Dictionary<string, Edge>();
            int networkEdgeCount = 0;
            double networkEdgeSum = 0;

            int networkEdgeCount2 = 0;
            double networkEdgeSum2 = 0;

            int networkEdgeCount3 = 0;
            double networkEdgeSum3 = 0;

            szVersion = szVersion.Replace("NODE",   (scaleNodesAndEdges? "NS" + scaleNodeValue : "NM" + nodeMult));
            szVersion = szVersion.Replace("EDGE", (scaleNodesAndEdges ? "ES" + scaleEdgeValue : "EM" + edgeMult)   );


            
             
            List<String> csvDayNodeLines = new List<string>();
            List<String> csvDayEdgeLines = new List<string>();
            List<String> csvDayPrunedEdgeLines = new List<string>();
            List<String> csvNodeLines = new List<string>();
            List<String> csvEdgeLines = new List<string>();
                //csvNetwork.WriteLine("DAY,TYPE,NAME,NODE1,NODE2,VALUE,SIZE,PRUNED,AVERAGE");
            /* csvNetwork.WriteLine("DAY,NODE,NODEVALUE,NODESIZE");
                csvNetwork.WriteLine("DAY,NODE1,NODE2,EDGE,EDGEVALUE,EDGEWIDTH");
                csvNetwork.WriteLine(colDate.Month + "/" + colDate.Day + "/" + colDate.Year + "," + node.szOriginalId + "," + node.value + "," + (nodeMult * node.size));
                csvNetwork.WriteLine(colDate.Month + "/" + colDate.Day + "/" + colDate.Year + "," + edge.szId1 + "," + edge.szId2 + "," + edge.szId + "," + edge.value + "," + ( edgeMult * edge.size));
            */
            foreach (DateTime colDate in dayNetworks.Keys)
            {
                DayNetwork dn = dayNetworks[colDate];
                double dayNodeMaxValue = 0;
                double dayChildNodeMaxValue = 0;
                double dayTeaherNodeMaxValue = 0;
                getMaxNodeValue(dn.nodes, ref dayNodeMaxValue, ref dayChildNodeMaxValue, ref dayTeaherNodeMaxValue);

                double dayEdgeMaxValue = 0;
                double dayChildEdgeMaxValue = 0;
                double dayTeaherEdgeMaxValue = 0;
                getMaxEdgeValue(dn.edges, ref dayEdgeMaxValue, ref dayChildEdgeMaxValue, ref dayTeaherEdgeMaxValue);



                //getEdgeMult(double dayEdgeMaxValue, double dayChildEdgeMaxValue, double dayTeaherEdgeMaxValue, Edge edge, double edgeMult)

                using (TextWriter sw = new StreamWriter(className + classYear + (prune ? "_PRUNE" : "") + "_" + (colDate.Month < 10 ? "0" : "") + colDate.Month + (colDate.Day < 10 ? "0" : "")  + colDate.Day + colDate.Year + "_" + szVersion + ".xml"))
                {
                    String header = "Creator \"Y\"\nVersion 1.0\ngraph\n[";
                    sw.WriteLine(header);
                    foreach (Node n in dn.nodes.Values)
                    {
                        //n.size = n.size * nodeMult;
                        double thisNodeMult = getNodeMult(dayNodeMaxValue, dayChildNodeMaxValue, dayTeaherNodeMaxValue, n, nodeMult);
                        sw.WriteLine("\t" + n.toXml(thisNodeMult));

                        csvDayNodeLines.Add(colDate.Month + "/" + colDate.Day + "/" + colDate.Year + "," + n.szOriginalId + "," + n.value + "," + (thisNodeMult * n.size)  );
                        //csvNetwork.WriteLine(colDate.Month + "/" + colDate.Day + "/" + colDate.Year + ",NODE," + n.szOriginalId + "," + n.szOriginalId + ",," + n.value + "," + (nodeMult * n.size)+ ",NA,NA");
                        // csvNetwork.WriteLine("DAY,TYPE,NODE1,NODE2,VALUE,SIZE,PRUNED");
                        Node networkNode = new Node(n.szOriginalId);

                        if (!nodes.ContainsKey(n.szOriginalId))
                        {
                            nodes.Add(n.szOriginalId, networkNode);
                        }
                        nodes[n.szOriginalId].numId = n.numId;
                        nodes[n.szOriginalId].szColor = n.szColor;
                        nodes[n.szOriginalId].value += n.value;
                        nodes[n.szOriginalId].size += n.size;
                        nodes[n.szOriginalId].numCollectionDates += 1;
                    }

                    int edgeCount = dn.edges.Count;
                    double edgeSum = 0;
                    if (prune)
                    {
                        foreach (Edge e in dn.edges.Values)
                        {
                            edgeSum += e.value;
                        }
                    }
                    double edgeAvg = edgeSum > 0 && edgeCount > 0 ? edgeSum / edgeCount : 0;
 
                    foreach (Edge e in dn.edges.Values)
                    {
                        //e.size = e.size * edgeMult;
                        double thisEdgeMult = getEdgeMult(dayEdgeMaxValue, dayChildEdgeMaxValue, dayTeaherEdgeMaxValue, e, edgeMult);
                       
                        if ((!prune) || e.value >= edgeAvg)
                        {
                            sw.WriteLine("\t" + e.toXml(thisEdgeMult));
                            Edge networkEdge = new Edge(e.szId1, e.szId2);

                            if (!edges.ContainsKey(e.szId))
                            {
                                edges.Add(e.szId, networkEdge);
                            }
                            edges[e.szId].value += e.value;
                            edges[e.szId].size += e.size;
                            edges[e.szId].numCollectionDates += 1;



                            // Console.WriteLine(file.Substring(file.LastIndexOf("ALL_") + 4, 10).Replace("_", "/") + "," + source.Replace("-", "") + "," + target.Replace("-", "") + "," + width.Trim());
                            Console.WriteLine(colDate.Month + "/" + colDate.Day + "/" + colDate.Year + "," + e.szId + "," + e.numId1 + "," + e.numId2 + "," + e.size + "," + (e.size * thisEdgeMult));
                            csvDayPrunedEdgeLines.Add(colDate.Month + "/" + colDate.Day + "/" + colDate.Year + "," + e.numId1 + "|" + e.numId2 + "," + e.numId1 + "," + e.numId2 + "," + e.size + "," + (e.size * thisEdgeMult) + "," + (((!prune) || e.value >= edgeAvg) ? 1 : 0) + "," + edgeAvg);

                        //networkEdgeCount++;
                        //networkEdgeSum += e.size;
                    }

                    csvDayEdgeLines.Add(colDate.Month + "/" + colDate.Day + "/" + colDate.Year + "," + e.numId1 + "|" + e.numId2 + "," + e.numId1 + "," + e.numId2 + "," + e.size + "," + (e.size * thisEdgeMult) + "," + (((!prune) || e.value >= edgeAvg) ? 1 : 0) + "," + edgeAvg);
                    
                        //csvNetwork.WriteLine("DAY,NODE,VALUE,SIZE,,,,DAY,EDGE,NODE1,NODE2,VALUE,SIZE,PRUNED,AVERAGE,,,,NODE,VALUE,SIZE,,,,EDGE,NODE1,NODE2,VALUE,SIZE,PRUNED,AVERAGE");
                        //csvNetwork.WriteLine(colDate.Month + "/" + colDate.Day + "/" + colDate.Year + ",EDGE" + "," + e.numId1 + "|" + e.numId2 + "," + e.numId1 + "," + e.numId2 + "," + e.size + "," + (e.size * edgeMult) +","+(((!prune) || e.value >= edgeAvg)?"NO":"YES")+","+ edgeAvg);
                        // csvNetwork.WriteLine("DAY,TYPE,NODE1,NODE2,VALUE,SIZE,PRUNED");
                    }

                    sw.WriteLine("]");
                }

            }



            double nodeMaxValue = 0;
            double childNodeMaxValue = 0;
            double teaherNodeMaxValue = 0;
            getMaxNodeValue(nodes, ref nodeMaxValue, ref childNodeMaxValue, ref teaherNodeMaxValue);
            double edgeMaxValue = 0;
            double childEdgeMaxValue = 0;
            double teaherEdgeMaxValue = 0;
            getMaxEdgeValue(edges, ref edgeMaxValue, ref childEdgeMaxValue, ref teaherEdgeMaxValue);

             


            using (TextWriter sw = new StreamWriter(className + classYear + (prune ? "_PRUNE" : "") + "_" + szVersion + ".xml"))
            {
                String header = "Creator \"Y\"\nVersion 1.0\ngraph\n[";
                sw.WriteLine(header);
                int csvLineCount = 0;
                foreach (Node n in nodes.Values)
                {
                    n.size = n.size / n.numCollectionDates;
                    n.value = n.value / n.numCollectionDates;
                    double thisNodeMult = getNodeMult(nodeMaxValue, childNodeMaxValue, teaherNodeMaxValue, n, nodeMult);
                    sw.WriteLine("\t" + n.toXml(thisNodeMult));

                    //csvNetwork.WriteLine("ALL,NODE," + n.szOriginalId + "," + n.szOriginalId + ",," + n.value + "," + (nodeMult * n.size) + ",NA,NA");
                        
                    csvNodeLines.Add(n.szOriginalId + "," + n.value + "," + (thisNodeMult * n.size));


                    csvLineCount++;
                }


                foreach (Edge e in edges.Values)
                {
                    // Console.WriteLine(file.Substring(file.LastIndexOf("ALL_") + 4, 10).Replace("_", "/") + "," + source.Replace("-", "") + "," + target.Replace("-", "") + "," + width.Trim());


                    //networkEdgeSum += e.size;
                    networkEdgeSum3 +=  e.size ;
                    networkEdgeCount3+= e.numCollectionDates;

                    networkEdgeSum2 += (e.value / e.numCollectionDates);
                    networkEdgeCount2++;

                    networkEdgeSum += (e.size / e.numCollectionDates);
                    networkEdgeCount++;
                }
                double networkEdgeAvg = networkEdgeSum > 0 && networkEdgeCount > 0 ? networkEdgeSum / networkEdgeCount : 0;
                double networkEdgeAvg2 = networkEdgeSum2 > 0 && networkEdgeCount2 > 0 ? networkEdgeSum2 / networkEdgeCount2 : 0;
                double networkEdgeAvg3 = networkEdgeSum3 > 0 && networkEdgeCount3 > 0 ? networkEdgeSum3 / networkEdgeCount3 : 0;

                foreach (Edge e in edges.Values)
                {
                    double thisEdgeMult = getEdgeMult(edgeMaxValue, childEdgeMaxValue, teaherEdgeMaxValue, e, edgeMult);

                    if ((!prune) || e.size >= networkEdgeAvg)
                    {
                        sw.WriteLine("\t" + e.toXml(thisEdgeMult));
                        //Console.WriteLine("ALL," + e.numId1 + "," + e.numId2 + "," + e.size + "," + (e.size * edgeMult));
                    }
                         
                    csvEdgeLines.Add(e.numId1 + "|" + e.numId2 + "," + e.numId1 + "," + e.numId2 + "," + e.size + "," + (e.size * thisEdgeMult) + "," + (((!prune) || e.value >= networkEdgeAvg) ? 1 : 0) + "," + networkEdgeAvg);
                    //csvNetwork.WriteLine("ALL,EDGE," + e.numId1 + "," + e.numId2 + "," + e.size + "," + (e.size * edgeMult) + "," + (((!prune) || e.value >= networkEdgeAvg) ? "NO" : "YES") + "," + networkEdgeAvg);
                    Console.WriteLine("ALL," + e.numId1 + "|" + e.numId2+","+ + e.numId1 + "," + e.numId2 + "," + e.size + "," + (e.size * thisEdgeMult) + "," + (((!prune) || e.size >= networkEdgeAvg) ? "NO" : "YES"));
                }
                    

                    
                sw.WriteLine("]");
            }
            using (TextWriter csvNetwork = new StreamWriter(className + classYear + (prune ? "_PRUNE" : "") + "_" + szDayVersion + "XML.CSV"))
            {
                csvNetwork.WriteLine(szVersion);
                csvNetwork.WriteLine("DAY,NODE,VALUE,SIZE,,,,DAY,EDGE,NODE1,NODE2,VALUE,SIZE,INCLUDE_NOTPRUNED,AVERAGE,,,,DAY,EDGE,NODE1,NODE2,VALUE,SIZE,INCLUDE_NOTPRUNED,AVERAGE,,,,NODE,VALUE,SIZE,,,,EDGE,NODE1,NODE2,VALUE,SIZE,INCLUDE_NOTPRUNED,AVERAGE");
                int numOfDayNodes = csvDayNodeLines.Count;
                int numOfNodes = csvNodeLines.Count;
                int numOfDayEdges = csvDayEdgeLines.Count;
                int numOfDayPrunedEdges = csvDayPrunedEdgeLines.Count;
                int numOfEdges = csvEdgeLines.Count;
                int max = Math.Max(numOfDayNodes, numOfNodes);
                max = Math.Max(max, numOfDayEdges);
                max = Math.Max(max, numOfDayPrunedEdges);
                max = Math.Max(max, numOfEdges);
                for (int m = 0; m < max; m++)
                {
                    if (m >= 57)
                    {
                        bool stop = true;
                    }
                    if (numOfDayNodes > m)
                        csvNetwork.Write(csvDayNodeLines[m]);
                    else
                        csvNetwork.Write(",,,");

                    csvNetwork.Write(",,,,");

                    if (numOfDayEdges > m)
                        csvNetwork.Write(csvDayEdgeLines[m]);
                    else
                        csvNetwork.Write(",,,,,,,");

                    csvNetwork.Write(",,,,");

                    if (numOfDayPrunedEdges > m)
                        csvNetwork.Write(csvDayPrunedEdgeLines[m]);
                    else
                        csvNetwork.Write(",,,,,,,");

                    csvNetwork.Write(",,,,");

                    


                    //csvNetwork.WriteLine("DAY,NODE,VALUE,SIZE,,,,DAY,EDGE,NODE1,NODE2,VALUE,SIZE,PRUNED,AVERAGE,,,,NODE,VALUE,SIZE,,,,EDGE,NODE1,NODE2,VALUE,SIZE,PRUNED,AVERAGE");

                    if (numOfNodes > m)
                        csvNetwork.Write(csvNodeLines[m]);
                    else
                        csvNetwork.Write(",,");

                    csvNetwork.Write(",,,,");

                    if (numOfEdges > m)
                        csvNetwork.WriteLine(csvEdgeLines[m]);
                    else
                        csvNetwork.WriteLine("");



                }
            }




           



        }
         
        public void getMaxEdgeValue(Dictionary<String, Edge> edges,ref double max, ref double maxChild, ref double maxTeacher)
        {
            foreach (Edge edge in edges.Values)
            {
                if (max < edge.value)
                    max = edge.value;

                if (!edge.isEdgeWithTeacher)
                {
                    if (maxChild < edge.value)
                        maxChild = edge.value;
                }
                else
                {
                    if (maxTeacher < edge.value)
                        maxTeacher = edge.value;
                }
            }
        }

       
        public void getMaxNodeValue(Dictionary<String, Node> nodes,ref double max, ref double maxChild, ref double maxTeacher)
        {
            foreach (Node node in nodes.Values)
            {
                if (max < node.value)
                    max = node.value;

                if ((!node.isTeacher(node.szOriginalId)) && (!node.isLab(node.szOriginalId)))
                {
                    if (maxChild < node.value)
                        maxChild = node.value;
                }
                else
                {
                    if (maxTeacher < node.value)
                        maxTeacher = node.value;
                }
            }
        }

        public void fromCSV()
        {


        }
    }
    class DayNetwork
    {
        public Dictionary<String,Node> nodes = new Dictionary<String, Node>();
        public Dictionary<String, Edge> edges = new Dictionary<String, Edge>();

        
    }
 
     
}
