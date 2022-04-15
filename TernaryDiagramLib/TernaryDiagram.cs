using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TernaryDiagramLib
{
    [DisplayName("TernaryDiagram")]
    [ToolboxBitmap(typeof(TernaryDiagram), "ternary.ico")]
    public class TernaryDiagram : Control
    {
        private void Initialize()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;

            this.Margin = new Padding(3, 8, 3, 8);

            _currentPoint = new PointT(0, 0, 0, 0, this);
            //_gradient.ExclusiveColors.AddRange(new int[] { _ab_to_bc_LineColor.ToArgb(), _bc_to_ca_LineColor.ToArgb(), _ca_to_ab_LineColor.ToArgb(), _triangleBorderColor.ToArgb(), _triangleBackgroundColor.ToArgb() });
            _zoomFactor = 0;
            _diagramPoints = new List<PointT>();
            _backColor2 = Color.Transparent;
            _gradType = GradType.Vertical;

            _trackingBox = new TrackingBox();
            
            this.PropertyChanged += TernaryDiagram_PropertyChanged;

            _diagramAreas = new DiagramAreaCollection();
            _diagramAreas.CollectionChanged += _diagramAreas_CollectionChanged;

            this.Size = new Size(380, 220);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public TernaryDiagram()
        {
            Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataTable">Source data table</param>
        /// <param name="dataColumnA">Column with A coordinates</param>
        /// <param name="dataColumnB">Column with B coordinates</param>
        /// <param name="dataColumnC">Column with C coordinates</param>
        public TernaryDiagram(DataTable dataTable, DataColumn dataColumnA, DataColumn dataColumnB, DataColumn dataColumnC)
        {
            Initialize();

            // Get first diagram area
            var diagramArea = _diagramAreas[0];
            diagramArea.SourceDataTable = dataTable;
            diagramArea.ValidateAndAddData(dataTable, dataColumnA, dataColumnB, dataColumnC);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataTable">Source data table</param>
        /// <param name="dataColumnA">Column with A coordinates</param>
        /// <param name="dataColumnB">Column with B coordinates</param>
        /// <param name="dataColumnC">Column with C coordinates</param>
        /// <param name="dataColumnD">Column with additional values (shown as color)</param>
        public TernaryDiagram(DataTable dataTable, DataColumn dataColumnA, DataColumn dataColumnB, DataColumn dataColumnC, DataColumn dataColumnD)
        {

            Initialize();
            // Get first diagram area
            var diagramArea = _diagramAreas[0];
            diagramArea.SourceDataTable = dataTable;
            diagramArea.ValidateAndAddData(dataTable, dataColumnA, dataColumnB, dataColumnC, dataColumnD);
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        private DiagramAreaCollection _diagramAreas;

        #region Variables
        // Box with coordinates

        // Point on the screen with mouse cursor on it
        private PointT _currentPoint;

        // Transformation matrix
        private Matrix _transformMatrix = new Matrix();

        // Zoom factor >= 1
        private float _zoomFactor;
        private bool _zoomEnabled = true;

        // Points
        private List<PointT> _diagramPoints;

        // Background gradient
        private Color _backColor2;
        private bool _backgroundGradientEnabled = false;
        private GradType _gradType;

        private TrackingBox _trackingBox;
        #endregion // Vars

        #region Properties
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Diagram")]
        [Description("Collection of diagram areas")]
        public DiagramAreaCollection DiagramAreas
        {
            get { return _diagramAreas; }
        }

        /// <summary>
        /// Gets or sets box tracking diagram coordinates
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(TrackingBoxConverter))]
        [Category("Tracking box")]
        [Description("Gets or sets tracking box")]
        public TrackingBox TrackingBox
        {
            get { return _trackingBox; }
            set
            {
                _trackingBox = value;
            }
        }

        [Category("Appearance")]
        [Description("Enables or disables zooming with mouse scroll")]
        [DefaultValue(true)]
        public bool ZoomEnabled
        {
            get { return _zoomEnabled; }
            set { _zoomEnabled = value; }
        }

        [Description("Background color of the control. If background gradient is eneabled then it is mixed with second back color.")]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
                OnChanged(this, new PropertyChangedEventArgs("BackColor"));
            }
        }

        [Category("Appearance")]
        [Description("Second background color for gradient. Background gradient has to be enabled.")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color BackColor2
        {
            get { return _backColor2; }
            set
            {
                _backColor2 = value;
                OnChanged(this, new PropertyChangedEventArgs("BackColor2"));
            }
        }

        [Category("Appearance")]
        [Description("Enables or disables background gradient")]
        [DefaultValue(false)]
        public bool BackgroundGradientEnabled
        {
            get { return _backgroundGradientEnabled; }
            set
            {
                if (this.BackColor == Color.Transparent) this.BackColor = Color.White;
                if (this.BackColor2 == Color.Transparent) this.BackColor2 = Color.White;
                _backgroundGradientEnabled = value;
                OnChanged(this, new PropertyChangedEventArgs("BackgroundGradientEnabled"));
            }
        }

        [Category("Appearance")]
        [Description("Type of background gradient - vertical or horizontal")]
        [DefaultValue(GradType.Vertical)]
        public GradType BackgroundGradientType
        {
            get { return _gradType; }
            set
            {
                _gradType = value;
                OnChanged(this, new PropertyChangedEventArgs("BackgroundGradientType"));
            }
        }

        /// <summary>
        /// Gets list of ternary diagram points
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public List<PointT> DiagramPoints
        {
            get { return _diagramPoints; }
        }
        #endregion // Properties

        #region Handling painting and other events
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (_backgroundGradientEnabled)
            {
                Graphics g = e.Graphics;
                RectangleF bounds = e.Graphics.ClipBounds;
                PointF point1 = new PointF(bounds.Width / 2, 0);
                PointF point2 = new PointF(bounds.Width / 2, bounds.Height);

                if (_gradType == GradType.Horizontal)
                {
                    point1 = new PointF(0, bounds.Height / 2);
                    point2 = new PointF(bounds.Width, bounds.Height / 2);
                }

                LinearGradientBrush brush = new LinearGradientBrush(point1, point2, BackColor, BackColor2);
                g.FillRectangle(brush, bounds);
            }
            else base.OnPaintBackground(e);
        }

        private void CalculateAreasInnerPositionsAndSizes(PaintEventArgs e)
        {
            // Get the working area of the control
            var controlWorkspace = e.ClipRectangle;
            // Taking into account margins
            var newX = controlWorkspace.Left + this.Margin.Left;
            var newY = controlWorkspace.Top + this.Margin.Top;
            var newWidth = controlWorkspace.Width - this.Margin.Left - this.Margin.Right;
            var newHeight = controlWorkspace.Height - this.Margin.Top - this.Margin.Bottom;
            var controlWorkspaceWithoutMargins = new Rectangle(newX, newY, newWidth, newHeight); 
            
            // Decide how to split control workspace to fit all diagrams
            // First expand grid horizontally and then vertically, so it's gonna be 1x1, 2x1, 2x2, 3x2, 3x3, 4x3,...
            var diagramCount = _diagramAreas.Count();
            var horizontalSplit = 1;
            var verticalSplit = 1;
            while (horizontalSplit * verticalSplit < diagramCount)
            {
                if (horizontalSplit == verticalSplit)
                {
                    horizontalSplit++;
                }
                else
                {
                    verticalSplit++;
                }
            }

            // Set working area for each diagram
            foreach (var diagram in _diagramAreas)
            {
                var diagramIndex = _diagramAreas.IndexOf(diagram);
                var rowIndex = diagramIndex / horizontalSplit;
                var columnIndex = diagramIndex % horizontalSplit;

                var diagramX = controlWorkspaceWithoutMargins.Left + columnIndex*(controlWorkspaceWithoutMargins.Width/horizontalSplit);
                var diagramY = controlWorkspaceWithoutMargins.Top + rowIndex * (controlWorkspaceWithoutMargins.Height / verticalSplit);
                var diagramWidth = controlWorkspaceWithoutMargins.Width/horizontalSplit;
                var diagramHeight = controlWorkspaceWithoutMargins.Height/verticalSplit;

                diagram.WorkingArea = new Rectangle(diagramX, diagramY, diagramWidth, diagramHeight);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            CalculateAreasInnerPositionsAndSizes(e);

            foreach (var diagram in _diagramAreas)
            {
                Rectangle bounds = diagram.WorkingArea;
                Graphics g = e.Graphics;
                // Tuning graphics
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Diagram middle point
                float centerX = bounds.X + bounds.Width / 2;
                float centerY = bounds.Y + bounds.Height / 2;

                int topMargin = diagram.Margin.Top;
                if (!String.IsNullOrEmpty(diagram.Title))
                {
                    // Extra margin for title
                    SizeF titleSize = g.MeasureString(diagram.Title, diagram.TitleFont);
                    topMargin += (int)titleSize.Height + 5;

                    // Draw title
                    g.DrawString(diagram.Title, diagram.TitleFont, new SolidBrush(diagram.TitleColor), bounds.Left + bounds.Width / 2 - titleSize.Width / 2, bounds.Top);
                }

                // Zooming
                Matrix matrix = this._transformMatrix.Clone();
                g.Transform = matrix;

                // Correct bottom margin
                SizeF labelS = g.MeasureString("00%", diagram.AxisC.LabelFont);
                int bottomMargin = diagram.Margin.Bottom + (int)labelS.Width;

                // Correct left & right margin
                SizeF leftLabelSize = g.MeasureString(diagram.AxisB.Title, diagram.AxisB.TitleFont);
                SizeF rightLabelSize = g.MeasureString(diagram.AxisC.Title, diagram.AxisC.TitleFont);
                int leftMargin = diagram.Margin.Left + (int)leftLabelSize.Width + 3;
                int rightMargin = diagram.Margin.Right + (int)rightLabelSize.Width + 3;

                diagram.DiagramTriangle.Calculate(bounds, topMargin, bottomMargin, leftMargin, rightMargin);

                // Diagram scale "resolution"
                float stepNum = 10;// Scale labels and lines every stepNum %

                float maxFontSize = diagram.Axes.Max(n => n.LabelFont.Size);
                Axis largestFontAxis = diagram.Axes.First(x => x.LabelFont.Size == maxFontSize);
                SizeF axisLabelsSize = g.MeasureString("00%", largestFontAxis.LabelFont);

                // Finds best "resolution"
                if (diagram.DiagramTriangle.SideLength < 10 * axisLabelsSize.Width)
                {
                    // Acceptable resolutions
                    float[] resolutions = new float[] { 10, 5, 4, 2 };
                    while ((diagram.DiagramTriangle.SideLength < stepNum * axisLabelsSize.Width && stepNum >= 0) || (!resolutions.Contains(stepNum) && stepNum >= 0))
                    {
                        stepNum--;
                    }
                }

                // Triangle background
                g.FillPath(new SolidBrush(diagram.DiagramTriangle.BackColor), diagram.DiagramTriangle.Path);

                for (int i = 1; i < stepNum; i++)
                {
                    // Chart support lines
                    float abX = diagram.DiagramTriangle.VertexA.X - (i * (diagram.DiagramTriangle.SideLength / (2 * stepNum)));
                    float abY = diagram.DiagramTriangle.VertexA.Y + (i * (diagram.DiagramTriangle.Height / stepNum));
                    // Point on AB axis
                    PointF ab = new PointF(abX, abY);

                    float bcX = diagram.DiagramTriangle.VertexC.X - (i * (diagram.DiagramTriangle.SideLength / stepNum));
                    float bcY = diagram.DiagramTriangle.VertexC.Y;
                    // Point on BC axis
                    PointF bc = new PointF(bcX, bcY);

                    float caX = diagram.DiagramTriangle.VertexA.X + (i * (diagram.DiagramTriangle.SideLength / (2 * stepNum)));
                    float caY = diagram.DiagramTriangle.VertexA.Y + (i * (diagram.DiagramTriangle.Height / stepNum));
                    // Reversed point on CA axis
                    PointF ca = new PointF(caX, caY);

                    float caX2 = diagram.DiagramTriangle.VertexC.X - (i * (diagram.DiagramTriangle.SideLength / (2 * stepNum)));
                    float caY2 = diagram.DiagramTriangle.VertexC.Y - (i * (diagram.DiagramTriangle.Height / stepNum));
                    // Point on CA axis 
                    PointF ca2 = new PointF(caX2, caY2);

                    Pen p1 = new Pen(diagram.AxisB.Grid.LineColor, diagram.AxisB.Grid.LineWidth);
                    p1.DashStyle = diagram.AxisB.Grid.LineDashStyle;
                    Pen p2 = new Pen(diagram.AxisC.Grid.LineColor, diagram.AxisC.Grid.LineWidth);
                    p2.DashStyle = diagram.AxisC.Grid.LineDashStyle;
                    Pen p3 = new Pen(diagram.AxisA.Grid.LineColor, diagram.AxisA.Grid.LineWidth);
                    p3.DashStyle = diagram.AxisA.Grid.LineDashStyle;

                    if (diagram.AxisB.Grid.Enabled) g.DrawLine(p1, ab, bc);
                    if (diagram.AxisC.Grid.Enabled) g.DrawLine(p2, bc, ca2);
                    if (diagram.AxisA.Grid.Enabled) g.DrawLine(p3, ca, ab);

                    // Scale labels
                    string textAB = String.Format("{0}%", 100 * i / stepNum);
                    SizeF textABSize = g.MeasureString(textAB, diagram.AxisB.LabelFont);
                    Matrix m = this._transformMatrix.Clone();
                    m.RotateAt(60, new PointF(abX, abY), MatrixOrder.Prepend);
                    g.Transform = m;
                    g.DrawString(textAB, diagram.AxisB.LabelFont, new SolidBrush(diagram.AxisB.LabelColor), abX - textABSize.Width, abY - textABSize.Height / 2);
                    g.Transform = matrix;


                    string textBC = String.Format("{0}%", 100 - (100 * i / stepNum));
                    SizeF textBCSize = g.MeasureString(textBC, diagram.AxisC.LabelFont);
                    m = this._transformMatrix.Clone();
                    m.RotateAt(-60, new PointF(bcX, bcY + 15), MatrixOrder.Prepend);
                    g.Transform = m;
                    g.DrawString(textBC, diagram.AxisC.LabelFont, new SolidBrush(diagram.AxisC.LabelColor), bcX - textBCSize.Width / 2, bcY - textBCSize.Height / 2 + 15);
                    g.Transform = matrix;

                    string textCA = String.Format("{0}%", 100 * i / stepNum);
                    SizeF textCASize = g.MeasureString(textCA, diagram.AxisA.LabelFont);
                    g.DrawString(textCA, diagram.AxisA.LabelFont, new SolidBrush(diagram.AxisA.LabelColor), caX2 + 5, caY2 - textABSize.Height / 2);
                }

                // Triangle foreground
                g.DrawPath(new Pen(diagram.DiagramTriangle.BorderColor, diagram.DiagramTriangle.BorderWidth), diagram.DiagramTriangle.Path);

                // Scale titles
                SizeF sizeA = g.MeasureString(diagram.AxisA.Title, diagram.AxisA.TitleFont);
                g.DrawString(diagram.AxisA.Title, diagram.AxisA.TitleFont, new SolidBrush(diagram.AxisA.TitleColor), diagram.DiagramTriangle.VertexA.X - sizeA.Width / 2, diagram.DiagramTriangle.VertexA.Y - sizeA.Height);
                SizeF sizeB = g.MeasureString(diagram.AxisB.Title, diagram.AxisB.TitleFont);
                g.DrawString(diagram.AxisB.Title, diagram.AxisB.TitleFont, new SolidBrush(diagram.AxisB.TitleColor), diagram.DiagramTriangle.VertexB.X - sizeB.Width - 3, diagram.DiagramTriangle.VertexB.Y);
                SizeF sizeC = g.MeasureString(diagram.AxisC.Title, diagram.AxisC.TitleFont);
                g.DrawString(diagram.AxisC.Title, diagram.AxisC.TitleFont, new SolidBrush(diagram.AxisC.TitleColor), diagram.DiagramTriangle.VertexC.X + 3, diagram.DiagramTriangle.VertexC.Y);

                // Support arrows with axes description
                float lineLen = diagram.DiagramTriangle.SideLength * 0.4f;
                PointF p1L = new PointF(diagram.DiagramTriangle.VertexA.X - lineLen / 2, diagram.DiagramTriangle.VertexA.Y + diagram.DiagramTriangle.Height / 3 - axisLabelsSize.Width - 5);
                PointF p2L = new PointF(diagram.DiagramTriangle.VertexA.X + lineLen / 2, diagram.DiagramTriangle.VertexA.Y + diagram.DiagramTriangle.Height / 3 - axisLabelsSize.Width - 5);

                Matrix rot;
                SizeF lSize;
                // CA
                if (diagram.AxisA.SupportArrow.Enabled)
                {
                    rot = this._transformMatrix.Clone();
                    rot.RotateAt(60, new PointF(diagram.DiagramTriangle.VertexA.X, diagram.DiagramTriangle.VertexA.Y + 2 * diagram.DiagramTriangle.Height / 3));
                    g.Transform = rot;
                    g.DrawLine(new Pen(diagram.AxisA.SupportArrow.Color, 1), p1L, p2L);
                    // Arrow
                    g.FillClosedCurve(new SolidBrush(diagram.AxisA.SupportArrow.Color), new PointF[]
                    {p1L, new PointF(p1L.X + 10, p1L.Y - 2), new PointF(p1L.X + 10, p1L.Y + 2)}, FillMode.Alternate);
                    lSize = g.MeasureString(diagram.AxisA.SupportArrow.LabelText, diagram.AxisA.SupportArrow.LabelFont);
                    g.DrawString(diagram.AxisA.SupportArrow.LabelText, diagram.AxisA.SupportArrow.LabelFont,
                        new SolidBrush(diagram.AxisA.SupportArrow.Color),
                        new PointF(diagram.DiagramTriangle.VertexA.X - lSize.Width / 2, diagram.DiagramTriangle.VertexA.Y + diagram.DiagramTriangle.Height / 3 - axisLabelsSize.Width - 5 - axisLabelsSize.Height));
                    g.Transform = matrix;
                }
                // BC
                if (diagram.AxisC.SupportArrow.Enabled)
                {
                    rot = this._transformMatrix.Clone();
                    rot.RotateAt(180, new PointF(diagram.DiagramTriangle.VertexA.X, diagram.DiagramTriangle.VertexA.Y + 2 * diagram.DiagramTriangle.Height / 3));
                    g.Transform = rot;
                    g.DrawLine(new Pen(diagram.AxisC.SupportArrow.Color, 1), p1L, p2L);
                    // Arrow
                    g.FillClosedCurve(new SolidBrush(diagram.AxisC.SupportArrow.Color), new PointF[]
                    {p1L, new PointF(p1L.X + 10, p1L.Y - 2), new PointF(p1L.X + 10, p1L.Y + 2)}, FillMode.Alternate);
                    lSize = g.MeasureString(diagram.AxisC.SupportArrow.LabelText, diagram.AxisC.SupportArrow.LabelFont);
                    //g.DrawString(_scaleC_Name, _axisLabelsFont,
                    //    new SolidBrush(_scaleC_NameColor),
                    //    new PointF(VertexA.X - lSize.Width / 2, diagram.DiagramTriangle.VertexA.Y + diagram.DiagramTriangle.Height / 3 - axisLabelsSize.Width - 5 - axisLabelsSize.Height));
                    g.Transform = matrix;

                    g.DrawString(diagram.AxisC.SupportArrow.LabelText, diagram.AxisC.SupportArrow.LabelFont,
                        new SolidBrush(diagram.AxisC.SupportArrow.Color),
                        new PointF(diagram.DiagramTriangle.VertexA.X - lSize.Width / 2,
                            diagram.DiagramTriangle.VertexB.Y + axisLabelsSize.Width + 8));
                }
                // AB
                if (diagram.AxisB.SupportArrow.Enabled)
                {
                    rot = this._transformMatrix.Clone();
                    rot.RotateAt(-60, new PointF(diagram.DiagramTriangle.VertexA.X, diagram.DiagramTriangle.VertexA.Y + 2 * diagram.DiagramTriangle.Height / 3));
                    g.Transform = rot;
                    g.DrawLine(new Pen(diagram.AxisB.SupportArrow.Color, 1), p1L, p2L);
                    // Arrow
                    g.FillClosedCurve(new SolidBrush(diagram.AxisB.SupportArrow.Color), new PointF[]
                {
                p1L, new PointF(p1L.X + 10, p1L.Y - 2), new PointF(p1L.X + 10, p1L.Y + 2)
                }, FillMode.Alternate);
                    lSize = g.MeasureString(diagram.AxisB.SupportArrow.LabelText, diagram.AxisB.SupportArrow.LabelFont);

                    g.DrawString(diagram.AxisB.SupportArrow.LabelText, diagram.AxisB.SupportArrow.LabelFont,
                        new SolidBrush(diagram.AxisB.SupportArrow.Color),
                        new PointF(diagram.DiagramTriangle.VertexA.X - lSize.Width / 2, diagram.DiagramTriangle.VertexA.Y + diagram.DiagramTriangle.Height / 3 - axisLabelsSize.Width - 5 - axisLabelsSize.Height));
                    g.Transform = matrix;
                }
                // Points
                DrawPoints(g, diagram);

                // Reset transform so value gradient will not be scaled while zooming
                g.ResetTransform();

                // Draw value gradient (D)
                DrawValueGradient(g, diagram);

                //// Box with coordinates
                //if (this._trackingBox.Enabled)
                //{
                //    //_boxSize = new System.Drawing.Size(150, 90);
                //    this._trackingBox.Location = new Point(10, topMargin);
                //    //drawTrackingBox(g);
                //}
            }
        }

        void area_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Invalidate();
        }

        void _diagramAreas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (DiagramArea area in _diagramAreas)
            {
                area.PropertyChanged -= area_PropertyChanged;
                area.PropertyChanged += area_PropertyChanged;
            }
        }

        void TernaryDiagram_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Invalidate();
        }
        #endregion // Handling events

        #region Mouse Events
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (this._trackingBox.Enabled)
            {
                PointF pos = this.PointToClient(Cursor.Position);

                _currentPoint = ScreenToDiagramPoint(pos.X, pos.Y);

                Graphics g = this.CreateGraphics();

                DrawTrackingBox(g);
                //this.Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (_zoomEnabled)
            {
                if (e.Delta > 0)
                {

                    this._zoomFactor += 0.1f;
                    ZoomToPoint(1 + 0.1f, e.Location);

                }
                else
                {
                    this._zoomFactor -= 0.1f;
                    ZoomToPoint(1 - 0.1f, e.Location);
                }
                this.Invalidate();
            }
        }
        #endregion // MouseEvents 

        #region Methods
        private void DrawValueGradient(Graphics g, DiagramArea diagram)
        {
            var valueGradient = diagram.ValueGradient;
            if (diagram.ValueGradient.Enabled)
            {
                int margLen = 20;
                if (diagram.ValueGradient.Minimum != double.NaN)
                {
                    SizeF gls1 = g.MeasureString(valueGradient.Minimum.ToString("N4"), valueGradient.Font);
                    SizeF gls2 = g.MeasureString(valueGradient.Maximum.ToString("N4"), valueGradient.Font);
                    margLen = (int)Math.Max(gls1.Width, gls2.Width);
                }

                diagram.ValueGradient.Location = new Point(diagram.WorkingArea.Right - this.Margin.Right - valueGradient.Size.Width - margLen, diagram.Margin.Top);

                // Title
                int titleMargin = 0;
                if (!String.IsNullOrEmpty(valueGradient.Title))
                {
                    SizeF size = g.MeasureString(valueGradient.Title, valueGradient.TitleFont);
                    float titleX = valueGradient.Location.X - (size.Width / 2) + (valueGradient.Size.Width / 2);
                    float titleY = valueGradient.Location.Y;
                    g.DrawString(valueGradient.Title, valueGradient.TitleFont, new SolidBrush(valueGradient.TitleColor), new Point((int)titleX, (int)titleY));
                    titleMargin = (int)size.Height + 5;
                }

                Point location = valueGradient.Location;
                location.Y += titleMargin;

                // Gradient
                g.DrawImage(valueGradient.GetGradientImage(valueGradient.Size.Width, valueGradient.Size.Height), location);
                // Border
                g.DrawRectangle(new Pen(Color.Black), location.X, location.Y, valueGradient.Size.Width, valueGradient.Size.Height);

                // Distance between gradient labels
                int step = 50;
                while (valueGradient.Size.Height % step != 0)
                {
                    step--;
                }

                if (diagram.DiagramDataTable.Rows.Count > 1)
                {
                    for (int i = 0; i <= valueGradient.Size.Height; i += step)
                    {
                        // Marker
                        Point p1 = new Point(location.X + valueGradient.Size.Width, location.Y + i);
                        Point p2 = new Point(location.X + valueGradient.Size.Width + 5, location.Y + i);
                        g.DrawLine(new Pen(Color.Black), p1, p2);

                        double gradVal = valueGradient.Maximum - ((float)i / (float)valueGradient.Size.Height) * (valueGradient.Maximum - valueGradient.Minimum);
                        string txt = String.Format("{0:0.00}", gradVal);
                        SizeF strSize = g.MeasureString(txt, valueGradient.Font);
                        Point p3 = new Point(p2.X + 3, p2.Y - (int)(strSize.Height / 2));
                        g.DrawString(txt, valueGradient.Font, new SolidBrush(Color.Black), p3);
                    }
                }
            }
        }

        private void DrawPoints(Graphics g, DiagramArea diagram)
        {
            if (diagram.DiagramDataTable != null && diagram.DiagramDataTable.Rows.Count > 0)
            {
                var valueGradient = diagram.ValueGradient;

                foreach (DataRow dataRow in diagram.DiagramDataTable.Rows)
                {
                    // Calculating real coordinates (control coordinates)
                    float percA = (float)(double)dataRow[0];
                    float percB = (float)(double)dataRow[1];
                    float percC = (float)(double)dataRow[2];

                    // If value is not given use max 
                    //TODO: try something else
                    double value = dataRow[3] != System.DBNull.Value ? (double)dataRow[3] : valueGradient.Maximum;

                    PointT abc = new PointT(percA, percB, percC, value, this);
                    _diagramPoints.Add(abc);

                    
                    double percV = (value - valueGradient.Minimum) / (valueGradient.Maximum - valueGradient.Minimum);
                    Color pointColor = valueGradient.GetColorAtValue((float)percV);
                    
                    // Draw point
                    switch (this.DiagramAreas[0].MarkerType)
                    {
                        case MarkerType.Circle:
                            g.FillEllipse(new SolidBrush(pointColor), abc.MarkerRect);
                            break;
                        case MarkerType.Square:
                            g.FillRectangle(new SolidBrush(pointColor), abc.MarkerRect);
                            break;
                        case MarkerType.Triangle:
                            g.FillPolygon(new SolidBrush(pointColor), new PointF[]
                            {
                                new PointF(abc.MarkerRect.Left, abc.MarkerRect.Top),
                                new PointF(abc.MarkerRect.Right, abc.MarkerRect.Top),
                                new PointF(abc.MarkerRect.Left + abc.MarkerRect.Width/2, abc.MarkerRect.Bottom)
                            });
                            break;
                    }
                }
            }
        }

        private void DrawTrackingBox(Graphics g)
        {
            TrackingBox box = this._trackingBox;
            if (box.Enabled && !box.DisplayedPoint.Equals(_currentPoint))
            {
                DiagramArea diagram = this._diagramAreas[0];

                g.TextRenderingHint = TextRenderingHint.AntiAlias;

                SizeF labelSize = g.MeasureString("A", box.LabelsFont);
                int height = (int)(3 * (labelSize.Height + 5) + 3);

                // Background
                Rectangle boxRect = new Rectangle(box.Location, new Size(box.Width, height));
                g.FillRectangle(new SolidBrush(box.BackColor), boxRect);
                // Border
                if (box.BorderWidth > 0)
                    g.DrawRectangle(new Pen(box.BorderColor, box.BorderWidth), boxRect);
                // Labels
                string emptyVal = "-----";
                if (!float.IsNaN(_currentPoint.A))
                {
                    g.DrawString(String.Format("{0}: {1:00.0}%", diagram.AxisA.Title, _currentPoint.A),
                        box.LabelsFont, new SolidBrush(box.ForeColorA),
                        new Point(box.Location.X + 5, box.Location.Y + 5));
                    g.DrawString(String.Format("{0}: {1:00.0}%", diagram.AxisB.Title, _currentPoint.B),
                        box.LabelsFont, new SolidBrush(box.ForeColorB),
                        new Point(box.Location.X + 5, box.Location.Y + 10 + (int)labelSize.Height));
                    g.DrawString(String.Format("{0}: {1:00.0}%", diagram.AxisC.Title, _currentPoint.C),
                        box.LabelsFont, new SolidBrush(box.ForeColorC),
                        new Point(box.Location.X + 5, box.Location.Y + 15 + 2 * (int)labelSize.Height));
                }
                else //if (box.DisplayedPoint != null && !float.IsNaN(box.DisplayedPoint.ValueA))
                {
                    g.DrawString(String.Format("{0}: {1}", diagram.AxisA.Title, emptyVal),
                        box.LabelsFont, new SolidBrush(box.ForeColorA),
                        new Point(box.Location.X + 5, box.Location.Y + 5));
                    g.DrawString(String.Format("{0}: {1}", diagram.AxisB.Title, emptyVal),
                        box.LabelsFont, new SolidBrush(box.ForeColorB),
                        new Point(box.Location.X + 5, box.Location.Y + 10 + (int)labelSize.Height));
                    g.DrawString(String.Format("{0}: {1}", diagram.AxisC.Title, emptyVal),
                        box.LabelsFont, new SolidBrush(box.ForeColorC),
                        new Point(box.Location.X + 5, box.Location.Y + 15 + 2 * (int)labelSize.Height));
                }

                box.DisplayedPoint = _currentPoint;
            }
        }

        private void ZoomToPoint(float scale, Point origin)
        {
            Matrix matrix = this._transformMatrix;
            matrix.Translate(-origin.X, -origin.Y, MatrixOrder.Append);
            matrix.Scale(scale, scale, MatrixOrder.Append);
            matrix.Translate(origin.X, origin.Y, MatrixOrder.Append);

            this._transformMatrix = matrix;
        }

        /// <summary>
        /// Computes the location of ABC diagram point in control coordinates
        /// </summary>
        /// <param name="_coordA"></param>
        /// <param name="_coordB"></param>
        /// <param name="_coordC"></param>
        /// <returns>PointF object with control coordinates</returns>
        public PointF DiagramToScreenPoint(float _coordA, float _coordB, float _coordC)
        {
            // B value on AB axis
            DiagramArea diagram = this.DiagramAreas[0];
            float abX = diagram.DiagramTriangle.VertexA.X - (_coordB * (diagram.DiagramTriangle.SideLength / (2 * 100)));
            float abY = diagram.DiagramTriangle.VertexA.Y + (_coordB * (diagram.DiagramTriangle.Height / 100));
            PointF axisBval = new PointF(abX, abY);

            // C value on BC axis
            float bcX = diagram.DiagramTriangle.VertexB.X + (_coordC * (diagram.DiagramTriangle.SideLength / 100));
            float bcY = diagram.DiagramTriangle.VertexB.Y;
            PointF axisCval = new PointF(bcX, bcY);

            // A value on CA axis
            float caX = diagram.DiagramTriangle.VertexC.X - (_coordA * (diagram.DiagramTriangle.SideLength / (2 * 100)));
            float caY = diagram.DiagramTriangle.VertexC.Y - (_coordA * (diagram.DiagramTriangle.Height / 100));
            PointF axisAval = new PointF(caX, caY);

            // y=ax+b
            // Coef "a" of line creating axis AB
            float coefAab = (diagram.DiagramTriangle.VertexA.Y - diagram.DiagramTriangle.VertexB.Y) / (diagram.DiagramTriangle.VertexA.X - diagram.DiagramTriangle.VertexB.X);

            // Coef "b" of line going through the diagram point given by percA, percB and percC and crossing axis BC
            float coefBbcv = bcY - coefAab * bcX;

            // Point ABC
            float py = caY;
            float px = (py - coefBbcv) / coefAab;
            return new PointF(px, py);
        }

        /// <summary>
        /// Computes the location of ABC point on the screen in diagram coordinates
        /// </summary>
        /// <param name="px">X screen coordinate of the point</param>
        /// <param name="py">Y screen coordinate of the point</param>
        /// <returns>If correct point was found, then PointT object with diagram coordinates is returned, otherwise PointT object but with NaN coordinates</returns>
        public PointT ScreenToDiagramPoint(float px, float py)
        {
            var nullPoint = new PointT(float.NaN, float.NaN, float.NaN, float.NaN, this);
            if (!this.DiagramAreas.Any()) return nullPoint;
            DiagramArea diagram = this.DiagramAreas[0];

            if (diagram.DiagramTriangle.Path == null || diagram.DiagramTriangle.Path.PointCount == 0) return nullPoint;
            PointF[] tpts = new PointF[diagram.DiagramTriangle.Path.PointCount];
            tpts = diagram.DiagramTriangle.Path.PathPoints;
            _transformMatrix.TransformPoints(tpts);
            GraphicsPath path = new GraphicsPath(tpts, diagram.DiagramTriangle.Path.PathTypes);
            path.CloseAllFigures();

            if (path.IsVisible(new Point((int)px, (int)py)))
            {
                //PointT p = _diagramPoints.LastOrDefault<PointT>(n => n.MarkerRect.Contains(new PointF(px, py)));

                //// Color of pixel
                //Point point = new Point();
                //GetCursorPos(ref point);
                //Color color = GetColorAt(point);//PointToScreen(new Point((int)px, (int)py))
                //double range = _gradient.GetValueByColor(color);
                //double val = _minVal + range * (_maxVal - _minVal);

                // Tranform points according to scale of the triangle
                PointF[] pts = new PointF[] { diagram.DiagramTriangle.VertexA, diagram.DiagramTriangle.VertexB, diagram.DiagramTriangle.VertexC };
                _transformMatrix.TransformPoints(pts);
                float tSideLength = pts[2].X - pts[1].X;

                float coefAab = (pts[0].Y - pts[1].Y) / (pts[0].X - pts[1].X);
                float coefBbcv = py - coefAab * px;
                float ycv = pts[1].Y;
                float xcv = (ycv - coefBbcv) / coefAab;

                float lenBcv = xcv - pts[1].X;
                float Cval = lenBcv * 100 / tSideLength;

                float coefAca = -coefAab;
                float coefBca = pts[2].Y - coefAca * pts[2].X;

                float yav = py;
                float xav = (py - coefBca) / coefAca;
                float lenCav = (float)Math.Sqrt(Math.Pow(xav - pts[2].X, 2) + Math.Pow(yav - pts[2].Y, 2));
                float Aval = lenCav * 100 / tSideLength;

                float Bval = 100 - Cval - Aval;
                return new PointT(Aval, Bval, Cval, double.NaN, this);
            }
            else
            {
                return nullPoint;
            }
        }

        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        /// <summary>
        /// Gets pixel color at screen location
        /// </summary>
        /// <param name="location">Point of pixel location in screen coordinates</param>
        /// <returns>Color of pixel at specified location on the screen</returns>
        public Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }

        /// <summary>
        /// Exports diagram to bitmap with a specified height and width
        /// </summary>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <returns>GDI+ Bitmap</returns>
        public Bitmap ToBitmap(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height, this.CreateGraphics());
            using (Graphics g = Graphics.FromImage(bmp))
            {
                OnPaint(new PaintEventArgs(g, new Rectangle(0, 0, width, height)));
            }
            return bmp;
        }
        #endregion // Methods

        #region Eevents
        private event PropertyChangedEventHandler _propertyChanged;
        /// <summary>
        /// Occurs when property value of any diagram element changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                bool first = _propertyChanged == null;
                _propertyChanged += value;
                if (first && _propertyChanged != null)
                {
                    TrackingBox.PropertyChanged += OnChanged;
                }
            }
            remove
            {
                TrackingBox.PropertyChanged -= OnChanged;
            }
        }

        private void OnChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, e);
            }
        }
        #endregion // Events
    }
}
