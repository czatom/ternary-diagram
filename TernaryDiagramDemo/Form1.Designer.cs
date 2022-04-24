namespace TernaryDiagram
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            TernaryDiagramLib.DiagramArea diagramArea1 = new TernaryDiagramLib.DiagramArea();
            TernaryDiagramLib.DiagramArea diagramArea2 = new TernaryDiagramLib.DiagramArea();
            TernaryDiagramLib.DiagramArea diagramArea3 = new TernaryDiagramLib.DiagramArea();
            TernaryDiagramLib.DiagramArea diagramArea4 = new TernaryDiagramLib.DiagramArea();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageDiagram = new System.Windows.Forms.TabPage();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.diagramDataGridView = new System.Windows.Forms.DataGridView();
            this.aDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.diagramDataSet = new TernaryDiagram.DiagramDataSet();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.GeneratePointsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.LoadSlagDataToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMultipleDiagrams = new System.Windows.Forms.ToolStripButton();
            this.diagramContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ternaryDiagram = new TernaryDiagramLib.TernaryDiagram();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageDiagram.SuspendLayout();
            this.tabPageData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diagramDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagramDataSet)).BeginInit();
            this.mainToolStrip.SuspendLayout();
            this.diagramContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemExit});
            this.menuItem1.Text = "File";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 0;
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.MenuItemExit_Click);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1296, 255);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1296, 302);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.mainToolStrip);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1296, 22);
            this.statusStrip.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(1296, 255);
            this.splitContainer1.SplitterDistance = 995;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageDiagram);
            this.tabControl1.Controls.Add(this.tabPageData);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(995, 255);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageDiagram
            // 
            this.tabPageDiagram.Controls.Add(this.ternaryDiagram);
            this.tabPageDiagram.Location = new System.Drawing.Point(4, 22);
            this.tabPageDiagram.Name = "tabPageDiagram";
            this.tabPageDiagram.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiagram.Size = new System.Drawing.Size(987, 229);
            this.tabPageDiagram.TabIndex = 0;
            this.tabPageDiagram.Text = "Diagram";
            this.tabPageDiagram.UseVisualStyleBackColor = true;
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this.diagramDataGridView);
            this.tabPageData.Location = new System.Drawing.Point(4, 22);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageData.Size = new System.Drawing.Size(987, 250);
            this.tabPageData.TabIndex = 1;
            this.tabPageData.Text = "Data";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // diagramDataGridView
            // 
            this.diagramDataGridView.AutoGenerateColumns = false;
            this.diagramDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.diagramDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.aDataGridViewTextBoxColumn,
            this.bDataGridViewTextBoxColumn,
            this.cDataGridViewTextBoxColumn,
            this.dDataGridViewTextBoxColumn});
            this.diagramDataGridView.DataSource = this.bindingSource1;
            this.diagramDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramDataGridView.Location = new System.Drawing.Point(3, 3);
            this.diagramDataGridView.Name = "diagramDataGridView";
            this.diagramDataGridView.Size = new System.Drawing.Size(981, 244);
            this.diagramDataGridView.TabIndex = 0;
            // 
            // aDataGridViewTextBoxColumn
            // 
            this.aDataGridViewTextBoxColumn.DataPropertyName = "A";
            this.aDataGridViewTextBoxColumn.HeaderText = "A";
            this.aDataGridViewTextBoxColumn.Name = "aDataGridViewTextBoxColumn";
            // 
            // bDataGridViewTextBoxColumn
            // 
            this.bDataGridViewTextBoxColumn.DataPropertyName = "B";
            this.bDataGridViewTextBoxColumn.HeaderText = "B";
            this.bDataGridViewTextBoxColumn.Name = "bDataGridViewTextBoxColumn";
            // 
            // cDataGridViewTextBoxColumn
            // 
            this.cDataGridViewTextBoxColumn.DataPropertyName = "C";
            this.cDataGridViewTextBoxColumn.HeaderText = "C";
            this.cDataGridViewTextBoxColumn.Name = "cDataGridViewTextBoxColumn";
            // 
            // dDataGridViewTextBoxColumn
            // 
            this.dDataGridViewTextBoxColumn.DataPropertyName = "D";
            this.dDataGridViewTextBoxColumn.HeaderText = "D";
            this.dDataGridViewTextBoxColumn.Name = "dDataGridViewTextBoxColumn";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "RandomData";
            this.bindingSource1.DataSource = this.diagramDataSet;
            // 
            // diagramDataSet
            // 
            this.diagramDataSet.DataSetName = "DiagramDataSet";
            this.diagramDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.mainToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.GeneratePointsToolStripButton,
            this.toolStripSeparator1,
            this.LoadSlagDataToolStripButton,
            this.toolStripSeparator2,
            this.toolStripButton9,
            this.toolStripButtonMultipleDiagrams});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(1296, 25);
            this.mainToolStrip.Stretch = true;
            this.mainToolStrip.TabIndex = 0;
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBox1.Text = "100";
            // 
            // GeneratePointsToolStripButton
            // 
            this.GeneratePointsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.GeneratePointsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("GeneratePointsToolStripButton.Image")));
            this.GeneratePointsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GeneratePointsToolStripButton.Name = "GeneratePointsToolStripButton";
            this.GeneratePointsToolStripButton.Size = new System.Drawing.Size(94, 22);
            this.GeneratePointsToolStripButton.Text = "Generate points";
            this.GeneratePointsToolStripButton.Click += new System.EventHandler(this.GeneratePointsToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // LoadSlagDataToolStripButton
            // 
            this.LoadSlagDataToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.LoadSlagDataToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("LoadSlagDataToolStripButton.Image")));
            this.LoadSlagDataToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.LoadSlagDataToolStripButton.Name = "LoadSlagDataToolStripButton";
            this.LoadSlagDataToolStripButton.Size = new System.Drawing.Size(87, 22);
            this.LoadSlagDataToolStripButton.Text = "Load slag data";
            this.LoadSlagDataToolStripButton.Click += new System.EventHandler(this.LoadSlagDataToolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton9.Image")));
            this.toolStripButton9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(107, 22);
            this.toolStripButton9.Text = "Refresh control";
            this.toolStripButton9.Click += new System.EventHandler(this.RefreshToolStripButton_Click);
            // 
            // toolStripButtonMultipleDiagrams
            // 
            this.toolStripButtonMultipleDiagrams.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonMultipleDiagrams.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMultipleDiagrams.Image")));
            this.toolStripButtonMultipleDiagrams.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMultipleDiagrams.Name = "toolStripButtonMultipleDiagrams";
            this.toolStripButtonMultipleDiagrams.Size = new System.Drawing.Size(107, 22);
            this.toolStripButtonMultipleDiagrams.Text = "Multiple diagrams";
            this.toolStripButtonMultipleDiagrams.Click += new System.EventHandler(this.toolStripButtonMultipleDiagrams_Click);
            // 
            // diagramContextMenuStrip
            // 
            this.diagramContextMenuStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.diagramContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToPNGToolStripMenuItem});
            this.diagramContextMenuStrip.Name = "contextMenuStrip1";
            this.diagramContextMenuStrip.Size = new System.Drawing.Size(140, 26);
            // 
            // saveToPNGToolStripMenuItem
            // 
            this.saveToPNGToolStripMenuItem.Name = "saveToPNGToolStripMenuItem";
            this.saveToPNGToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.saveToPNGToolStripMenuItem.Text = "Save to PNG";
            this.saveToPNGToolStripMenuItem.Click += new System.EventHandler(this.SaveToPNGToolStripMenuItem_Click);
            // 
            // ternaryDiagram
            // 
            diagramArea1.AutoScale = false;
            diagramArea1.AxisA.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea1.AxisA.Maximum = 100F;
            diagramArea1.AxisA.Minimum = 0F;
            diagramArea1.AxisA.Name = "A";
            diagramArea1.AxisA.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea1.AxisA.SupportArrow.LabelText = "A";
            diagramArea1.AxisA.Title = "A";
            diagramArea1.AxisA.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea1.AxisB.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea1.AxisB.Maximum = 100F;
            diagramArea1.AxisB.Minimum = 0F;
            diagramArea1.AxisB.Name = "B";
            diagramArea1.AxisB.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea1.AxisB.SupportArrow.LabelText = "B";
            diagramArea1.AxisB.Title = "B";
            diagramArea1.AxisB.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea1.AxisC.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea1.AxisC.Maximum = 100F;
            diagramArea1.AxisC.Minimum = 0F;
            diagramArea1.AxisC.Name = "C";
            diagramArea1.AxisC.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea1.AxisC.SupportArrow.LabelText = "C";
            diagramArea1.AxisC.Title = "C";
            diagramArea1.AxisC.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea1.Margin = new System.Windows.Forms.Padding(0);
            diagramArea1.Name = "default";
            diagramArea1.SourceDataTable = null;
            diagramArea1.Title = "";
            diagramArea1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea2.AutoScale = false;
            diagramArea2.AxisA.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea2.AxisA.Maximum = 100F;
            diagramArea2.AxisA.Minimum = 0F;
            diagramArea2.AxisA.Name = "A";
            diagramArea2.AxisA.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea2.AxisA.SupportArrow.LabelText = "A";
            diagramArea2.AxisA.Title = "A";
            diagramArea2.AxisA.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea2.AxisB.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea2.AxisB.Maximum = 100F;
            diagramArea2.AxisB.Minimum = 0F;
            diagramArea2.AxisB.Name = "B";
            diagramArea2.AxisB.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea2.AxisB.SupportArrow.LabelText = "B";
            diagramArea2.AxisB.Title = "B";
            diagramArea2.AxisB.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea2.AxisC.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea2.AxisC.Maximum = 100F;
            diagramArea2.AxisC.Minimum = 0F;
            diagramArea2.AxisC.Name = "C";
            diagramArea2.AxisC.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea2.AxisC.SupportArrow.LabelText = "C";
            diagramArea2.AxisC.Title = "C";
            diagramArea2.AxisC.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea2.Margin = new System.Windows.Forms.Padding(0);
            diagramArea2.Name = null;
            diagramArea2.SourceDataTable = null;
            diagramArea2.Title = "";
            diagramArea2.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea3.AutoScale = false;
            diagramArea3.AxisA.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea3.AxisA.Maximum = 100F;
            diagramArea3.AxisA.Minimum = 0F;
            diagramArea3.AxisA.Name = "A";
            diagramArea3.AxisA.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea3.AxisA.SupportArrow.LabelText = "A";
            diagramArea3.AxisA.Title = "A";
            diagramArea3.AxisA.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea3.AxisB.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea3.AxisB.Maximum = 100F;
            diagramArea3.AxisB.Minimum = 0F;
            diagramArea3.AxisB.Name = "B";
            diagramArea3.AxisB.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea3.AxisB.SupportArrow.LabelText = "B";
            diagramArea3.AxisB.Title = "B";
            diagramArea3.AxisB.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea3.AxisC.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea3.AxisC.Maximum = 100F;
            diagramArea3.AxisC.Minimum = 0F;
            diagramArea3.AxisC.Name = "C";
            diagramArea3.AxisC.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea3.AxisC.SupportArrow.LabelText = "C";
            diagramArea3.AxisC.Title = "C";
            diagramArea3.AxisC.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea3.Margin = new System.Windows.Forms.Padding(0);
            diagramArea3.Name = null;
            diagramArea3.SourceDataTable = null;
            diagramArea3.Title = "";
            diagramArea3.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea4.AutoScale = false;
            diagramArea4.AxisA.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea4.AxisA.Maximum = 100F;
            diagramArea4.AxisA.Minimum = 0F;
            diagramArea4.AxisA.Name = "A";
            diagramArea4.AxisA.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea4.AxisA.SupportArrow.LabelText = "A";
            diagramArea4.AxisA.Title = "A";
            diagramArea4.AxisA.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea4.AxisB.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea4.AxisB.Maximum = 100F;
            diagramArea4.AxisB.Minimum = 0F;
            diagramArea4.AxisB.Name = "B";
            diagramArea4.AxisB.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea4.AxisB.SupportArrow.LabelText = "B";
            diagramArea4.AxisB.Title = "B";
            diagramArea4.AxisB.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea4.AxisC.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea4.AxisC.Maximum = 100F;
            diagramArea4.AxisC.Minimum = 0F;
            diagramArea4.AxisC.Name = "C";
            diagramArea4.AxisC.SupportArrow.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            diagramArea4.AxisC.SupportArrow.LabelText = "C";
            diagramArea4.AxisC.Title = "C";
            diagramArea4.AxisC.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            diagramArea4.Margin = new System.Windows.Forms.Padding(0);
            diagramArea4.Name = null;
            diagramArea4.SourceDataTable = null;
            diagramArea4.Title = "";
            diagramArea4.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.ternaryDiagram.DiagramAreas.Add(diagramArea1);
            this.ternaryDiagram.DiagramAreas.Add(diagramArea2);
            this.ternaryDiagram.DiagramAreas.Add(diagramArea3);
            this.ternaryDiagram.DiagramAreas.Add(diagramArea4);
            this.ternaryDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ternaryDiagram.Location = new System.Drawing.Point(3, 3);
            this.ternaryDiagram.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            this.ternaryDiagram.Name = "ternaryDiagram";
            this.ternaryDiagram.Size = new System.Drawing.Size(981, 223);
            this.ternaryDiagram.TabIndex = 0;
            this.ternaryDiagram.Text = "ternaryDiagram";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.SelectedObject = this.ternaryDiagram;
            this.propertyGrid1.Size = new System.Drawing.Size(297, 255);
            this.propertyGrid1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1296, 302);
            this.Controls.Add(this.toolStripContainer1);
            this.Menu = this.mainMenu;
            this.Name = "Form1";
            this.Text = "Ternary Diagram Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageDiagram.ResumeLayout(false);
            this.tabPageData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.diagramDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagramDataSet)).EndInit();
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.diagramContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.DataGridView diagramDataGridView;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnADataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnCDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnDDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton GeneratePointsToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private DiagramDataSet diagramDataSet;
        private System.Windows.Forms.ContextMenuStrip diagramContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem saveToPNGToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn aDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn bDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripButton LoadSlagDataToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.MenuItem menuItemExit;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDiagram;
        private System.Windows.Forms.TabPage tabPageData;
        private TernaryDiagramLib.TernaryDiagram ternaryDiagram;
        private System.Windows.Forms.ToolStripButton toolStripButtonMultipleDiagrams;
    }
}

