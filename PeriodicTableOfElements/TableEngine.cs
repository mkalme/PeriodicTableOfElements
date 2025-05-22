using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;
using System.Diagnostics;

namespace PeriodicTableOfElements
{
    class TableEngine
    {
        static string[] colors =  {"#FFEA84", "#FF4F4F", "#FFD177",
                                   "#E599AB", "#F4C1F4", "#F79BCB",
                                   "#DBDBDB", "#A5A5A5", "#CDCEAD",
                                   "#7CFFFF"
        };

        static int[][] colorBelonging = {
            new int[]{0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 9},
            new int[]{1, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 8, 0, 0, 0, 0, 9},
            new int[]{1, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 7, 8, 0, 0, 0, 9 },
            new int[]{1, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 7, 7, 8, 8, 0, 0, 9},
            new int[]{1, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 7, 7, 7, 8, 8, 0, 9 },
            new int[]{1, 2, 4, 3, 3, 3, 3, 3, 3, 3, 3, 7, 7, 7, 7, 7, 8, 9 },
            new int[]{1, 2, 5, 3, 3, 3, 3, 3, 6, 6, 6, 7, 6, 6, 6, 6, 6, 6 },
            new int[]{-1, -1, -1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,-1},
            new int[]{ -1, -1, -1, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, -1 }
        };

        public static string getNewString() {
            XmlDocument document = new XmlDocument();
            document.LoadXml(Properties.Resources.Storage);

            XmlNodeList rowList = document.SelectNodes("/root/row");

            for (int i = 0; i < rowList.Count; i++) {
                XmlNodeList elemList = rowList[i].ChildNodes;

                for (int b = 0; b < elemList.Count; b++) {
                    if (elemList[b].Name.Equals("elem")) {

                        elemList[b].Attributes["color"].Value = colors[colorBelonging[i][b]];
                    }
                }
            }

            string text = document.InnerXml;

            //Replace
            text = text.Replace("><", ">\n<");

            text = text.Replace("<row", "\t<row");
            text = text.Replace("</row", "\t</row");

            text = text.Replace("<elem", "\t\t<elem");
            text = text.Replace("<bl", "\t\t<bl");

            text = text.Replace("<root", "\n<root");


            return text;
        }
    }
}
