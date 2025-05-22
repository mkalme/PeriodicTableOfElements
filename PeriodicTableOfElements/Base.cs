using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Reflection;

namespace PeriodicTableOfElements
{
    public partial class Base : Form
    {
        GraphicsEngine graphics;
        public static Bitmap bmp;

        BackgroundWorker bgw;

        public Base()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Base_Load(object sender, EventArgs e)
        {
            graphics = new GraphicsEngine(Properties.Resources.Storage, pictureBox1);

            TableEventEngine.load();

            bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.ProgressChanged += new ProgressChangedEventHandler(bgw_ProgressChanged);
            bgw.WorkerReportsProgress = true;
            bgw.RunWorkerAsync();
        }


        void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pictureBox1.Image = bmp;
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            int c = 0;
            while (true)
            {
                Thread.Sleep(5);

                graphics.drawTable();

                bgw.ReportProgress(c++);
            }
        }


        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int[] cell = graphics.getCell(e.X, e.Y);

            if (cell[0] > -1 && cell[1] > -1) {
                if (graphics.Table[cell[1]][cell[0]].Name.Equals("elem")) {
                    string name = graphics.Table[cell[1]][cell[0]].Attributes["name"].Value;

                    if (TableEventEngine.check(name))
                    {
                        graphics.changeColor(
                            cell[0],
                            cell[1],
                            "#FFFFFF"
                        );
                        prevCellColor = "#FFFFFF";

                        TableEventEngine.generate();
                    }
                    else {
                        Debug.WriteLine("Wrong!");
                    }
                }
            }
        }
        private Color changeColorBrightness(Color color, double value) {
            int r = (int)((double)color.R * value);
            int g = (int)((double)color.G * value);
            int b = (int)((double)color.B * value);

            r = r > 255 ? 255 : r;
            g = g > 255 ? 255 : g;
            b = b > 255 ? 255 : b;

            return Color.FromArgb(1, r, g, b);
        }


        int[] prevCell = new int[] {-1, -1};
        string prevCellColor = "";
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int[] cell = graphics.getCell(e.X, e.Y);

            if (prevCell[0] != cell[0] || prevCell[1] != cell[1])
            {
                if (prevCell[0] > -1 && prevCell[1] > -1)
                {
                    if (graphics.Table[prevCell[1]][prevCell[0]].Name.Equals("elem"))
                    {
                        graphics.changeColor(
                            prevCell[0],
                            prevCell[1],
                            prevCellColor
                        );
                    }
                }
                prevCell[0] = cell[0];
                prevCell[1] = cell[1];

                if (cell[0] > -1 && cell[1] > -1)
                {
                    if (graphics.Table[cell[1]][cell[0]].Name.Equals("elem"))
                    {
                        string color = graphics.Table[cell[1]][cell[0]].Attributes["color"].Value;

                        graphics.changeColor(
                            cell[0],
                            cell[1],
                            ColorTranslator.ToHtml(changeColorBrightness(ColorTranslator.FromHtml(color), 0.88))
                        );

                        prevCellColor = color;
                    }
                }
            }
        }
    }
}
