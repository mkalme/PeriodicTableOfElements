using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace PeriodicTableOfElements
{
    class GraphicsEngine
    {
        PictureBox pictureBox;
        Bitmap bmp;
        Graphics bitmapGraphics;
        string fileText;

        public Point StartingPoint = new Point(0, 0);
        public XmlNodeList[] Table = { };

        public  int[][] SpacerX;
        public  int[] SpacerY;

        public int[,] RowAreas;
        public int[][,] CellAreas;

        public GraphicsEngine(string fileText, PictureBox pictureBox) {
            bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            bitmapGraphics = Graphics.FromImage(bmp);

            bitmapGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            //bitmapGraphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            bitmapGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;

            this.pictureBox = pictureBox;
            this.fileText = fileText;

            loadTable();
            loadSpacers();

            setStartingPoint();

            setRowAreas();
            setCellAreas();
        }

        private void loadTable()
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(fileText);

            XmlNodeList rows = document.SelectNodes("/root/row");
            Table = new XmlNodeList[rows.Count];

            for (int i = 0; i < rows.Count; i++)
            {
                Table[i] = rows[i].ChildNodes;
            }
        }
        private void loadSpacers()
        {
            SpacerX = new int[Table.Length][];
            SpacerY = new int[Table.Length];

            for (int i = 0; i < Table.Length; i++)
            {
                SpacerX[i] = new int[Table[i].Count];
                for (int b = 0; b < Table[i].Count; b++)
                {
                    //X
                    if (nodeHasAttribute(Table[i][b], "spacer"))
                    {
                        int currentSpacerX = 0;

                        Int32.TryParse(Table[i][b].Attributes["spacer"].Value, out currentSpacerX);

                        SpacerX[i][b] = currentSpacerX;
                    }
                    else
                    {
                        SpacerX[i][b] = 0;
                    }

                    //Y
                    if (b == Table[i].Count - 1)
                    {
                        if (nodeHasAttribute(Table[i][b].ParentNode, "spacer"))
                        {
                            int currentSpacerY = 0;

                            Int32.TryParse(Table[i][b].ParentNode.Attributes["spacer"].Value, out currentSpacerY);

                            SpacerY[i] = currentSpacerY;
                        }
                        else
                        {
                            SpacerY[i] = 0;
                        }
                    }
                }
            }
        }
        private bool nodeHasAttribute(XmlNode node, string attribute)
        {
            bool hasOne = false;

            for (int i = 0; i < node.Attributes.Count; i++)
            {
                if (node.Attributes[i].Name.Equals(attribute))
                {
                    hasOne = true;
                    goto after_loop;
                }
            }
        after_loop:

            return hasOne;
        }

        private void setStartingPoint()
        {
            //X
            int tableWidth = 0;

            if (Table.Length > 0)
            {
                tableWidth = (Table[0].Count * (Settings.CellSize + Settings.BorderWidth)) + SpacerX[0].Sum();
            }

            StartingPoint.X = (pictureBox.Width - tableWidth) / 2;

            //Y
            int tableHeight = 0;

            tableHeight = (Table.Length * (Settings.CellSize + Settings.BorderWidth)) + SpacerY.Sum();

            StartingPoint.Y = (pictureBox.Height - tableHeight) / 2;
        }
        private void setRowAreas() {
            RowAreas = new int[Table.Length, 2];

            for (int i = 0; i < RowAreas.GetLength(0); i++) {
                int y1 = StartingPoint.Y + ((Settings.CellSize + Settings.BorderWidth) * i);

                for (int b = 0; b < i; b++) {
                    y1 += SpacerY[b];
                }

                int y2 = y1 + (Settings.CellSize + Settings.BorderWidth);

                RowAreas[i, 0] = y1;
                RowAreas[i, 1] = y2;
            }
        }
        private void setCellAreas() {
            CellAreas = new int[Table.Length][,];

            for (int i = 0; i < CellAreas.GetLength(0); i++) {
                CellAreas[i] = new int[Table[i].Count, 2];

                for (int b = 0; b < CellAreas[i].GetLength(0); b++) {
                    int x1 = StartingPoint.X + ((Settings.CellSize + Settings.BorderWidth) * b);

                    for (int c = 0; c < b; c++)
                    {
                        x1 += SpacerX[i][c];
                    }

                    int x2 = x1 + (Settings.CellSize + Settings.BorderWidth);

                    CellAreas[i][b, 0] = x1;
                    CellAreas[i][b, 1] = x2;
                }
            }
        }

        public int[] getCell(int x, int y) {
            int x1 = -1;
            int y1 = -1;

            //get row
            for (int i = 0; i < RowAreas.GetLength(0); i++) {
                if (y >= RowAreas[i, 0] && y <= RowAreas[i, 1]) {
                    y1 = i;

                    goto after_loop;
                }
            }
            after_loop:

            if (y1 > -1){
                //get row
                for (int i = 0; i < CellAreas[y1].GetLength(0); i++)
                {
                    if (x >= CellAreas[y1][i, 0] && x <= CellAreas[y1][i, 1])
                    {
                        x1 = i;

                        goto after_loop2;
                    }
                }
            }
            after_loop2:

            if (x1 < 0) {
                y1 = -1;
            }

            return new int[]{x1, y1};
        }

        //Graphics
        public void drawTable() {
            bitmapGraphics.Clear(SystemColors.Control);

            //rows
            for (int y = 0; y < Table.Length; y++)
            {
                //col
                for (int x = 0; x < Table[y].Count; x++)
                {
                    drawCell(x, y);
                }
            }

            Base.bmp = bmp;
        }
        private void drawCell(int x, int y) {
            int spacerX = 0;
            int spacerY = 0;

            //X
            for (int i = 0; i < x; i++) {
                spacerX += SpacerX[y][i];
            }
            //Y
            for (int i = 0; i < y; i++){
                spacerY += SpacerY[i];
            }

            Point currentPoint = new Point(StartingPoint.X + (x * (Settings.CellSize + Settings.BorderWidth)) + spacerX,
                                           StartingPoint.Y + (y * (Settings.CellSize + Settings.BorderWidth)) + spacerY
            );

            //check if not empty
            if (Table[y][x].Name.Equals("elem"))
            {
                Color color = ColorTranslator.FromHtml(Table[y][x].Attributes["color"].Value);

                //Color
                bitmapGraphics.FillRectangle(
                    new SolidBrush(color),
                    new Rectangle(new Point(currentPoint.X, currentPoint.Y), new Size(Settings.CellSize, Settings.CellSize))
                );

                //Border
                bitmapGraphics.DrawRectangle(
                    new Pen(ColorTranslator.FromHtml("#232323"), Settings.BorderWidth),
                    new Rectangle(new Point(currentPoint.X, currentPoint.Y), new Size(Settings.CellSize, Settings.CellSize))
                );

                //Text
                bitmapGraphics.DrawString(
                    Table[y][x].Attributes["text"].Value.Replace(@"\n", Environment.NewLine),
                    new Font("Arial", (int)((double)Settings.CellSize / 3), FontStyle.Bold),
                    new SolidBrush(ColorTranslator.FromHtml("#232323")),
                    new Point(currentPoint.X + Settings.CellSize / 20, currentPoint.Y + Settings.CellSize / 20)
                );
            }
        }

        public void changeColor(int x, int y, string color) {
            if (Table[y][x].Name.Equals("elem")) {
                Table[y][x].Attributes["color"].Value = color;
            }
        }
    }
}
