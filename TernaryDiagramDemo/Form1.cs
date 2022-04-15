using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TernaryDiagram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DataTable dataTable = new DataTable();

        private void Form1_Load(object sender, EventArgs e)
        {
            diagramDataGridView.AutoGenerateColumns = true;

            // Read slag data
            ReadSlagData();
        }

        private void ReadSlagData()
        {
            DiagramDataSet.SlagDataDataTable dt = diagramDataSet.SlagData;
            using (StreamReader sr = new StreamReader("slag_data.csv"))
            {
                // Skip header
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string[] values = sr.ReadLine().Split(';');
                    DiagramDataSet.SlagDataRow dr = dt.NewSlagDataRow();
                    dr.Si = double.Parse(values[0]);
                    dr.Fe = double.Parse(values[1]);
                    dr.CaO = double.Parse(values[2]);
                    dr.SiO2 = double.Parse(values[3]);
                    dr.MgO = double.Parse(values[4]);
                    dr.Al2O3 = double.Parse(values[5]);
                    dr.MnO = double.Parse(values[6]);
                    dr.P2O5 = double.Parse(values[7]);
                    dr.S = double.Parse(values[8]);
                    dr.basicity = double.Parse(values[9]);
                    dr.FeOn = double.Parse(values[10]);
                    dr.FeOn_t = double.Parse(values[11]);
                    dr.CaO_t = double.Parse(values[12]);
                    dr.SiO2_t = double.Parse(values[13]);

                    dt.Rows.Add(dr);
                }
            }
        }

        private void GeneratePointsToolStripButton_Click(object sender, EventArgs e)
        {
            diagramDataSet.RandomData.Clear();

            // Generate some random data
            Random ran = new Random();
            int numberOfPoints = 0;
            if (int.TryParse(toolStripTextBox1.Text, out numberOfPoints) && numberOfPoints > 0)
            {
                for (int i = 0; i < numberOfPoints; i++)
                {
                    double valA = ran.NextDouble() * 100;
                    double valB = ran.NextDouble() * (100 - valA);
                    double valC = 100 - valA - valB;

                    double valD = 1630 + ran.NextDouble() * 80;

                    diagramDataSet.RandomData.AddRandomDataRow(valA, valB, valC, valD);
                }

                ternaryDiagram.DiagramAreas[0].LoadData(diagramDataSet.RandomData,
                    diagramDataSet.RandomData.AColumn,
                    diagramDataSet.RandomData.BColumn,
                    diagramDataSet.RandomData.CColumn,
                    diagramDataSet.RandomData.DColumn);

                ternaryDiagram.DiagramAreas[0].Title = "Random data";
                ternaryDiagram.DiagramAreas[0].AxisA.Title = "A";
                ternaryDiagram.DiagramAreas[0].AxisB.Title = "B";
                ternaryDiagram.DiagramAreas[0].AxisC.Title = "C";
                ternaryDiagram.DiagramAreas[0].AxisA.SupportArrow.LabelText = "% A";
                ternaryDiagram.DiagramAreas[0].AxisB.SupportArrow.LabelText = "% B";
                ternaryDiagram.DiagramAreas[0].AxisC.SupportArrow.LabelText = "% C";

                bindingSource1.DataMember = "RandomData";
            }
        }

        private void RefreshToolStripButton_Click(object sender, EventArgs e)
        {
            ternaryDiagram.Refresh();
        }

        private void SaveToPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = ternaryDiagram.ToBitmap(1920, 1600);
            bmp.Save("test.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void LoadSlagDataToolStripButton_Click(object sender, EventArgs e)
        {
            ternaryDiagram.DiagramAreas[0].LoadData(diagramDataSet.SlagData,
                                diagramDataSet.SlagData.CaO_tColumn,
                                diagramDataSet.SlagData.FeOn_tColumn,
                                diagramDataSet.SlagData.SiO2_tColumn, 
                                diagramDataSet.SlagData.SiColumn);

            ternaryDiagram.DiagramAreas[0].Title = "Slag data";
            ternaryDiagram.DiagramAreas[0].AxisA.Title = "CaO";
            ternaryDiagram.DiagramAreas[0].AxisB.Title = "FeO";
            ternaryDiagram.DiagramAreas[0].AxisC.Title = "SiO2";
            ternaryDiagram.DiagramAreas[0].AxisA.SupportArrow.LabelText = "CaO [%]";
            ternaryDiagram.DiagramAreas[0].AxisB.SupportArrow.LabelText = "FeO [%]";
            ternaryDiagram.DiagramAreas[0].AxisC.SupportArrow.LabelText = "SiO2 [%]";
            ternaryDiagram.DiagramAreas[0].ValueGradient.Title = "Si content in the hot metal [ppm]";

            diagramDataGridView.AutoGenerateColumns = true;
            bindingSource1.DataMember = "SlagData";
        }

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
