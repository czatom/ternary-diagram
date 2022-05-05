using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TernaryDiagramLib
{
    [DisplayName("TernaryDiagram")]
    [ToolboxBitmap(typeof(TernaryDiagram), "ternary")]
    public class TernaryDiagram : Control
    {
        private void Initialize()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;

            this.Margin = new Padding(3, 8, 3, 8);

            _currentPoint = new PointT(0, 0, 0, 0, null);
            //_gradient.ExclusiveColors.AddRange(new int[] { _ab_to_bc_LineColor.ToArgb(), _bc_to_ca_LineColor.ToArgb(), _ca_to_ab_LineColor.ToArgb(), _triangleBorderColor.ToArgb(), _triangleBackgroundColor.ToArgb() });
            _zoomFactor = 0;
            _backColor2 = Color.Transparent;
            _gradType = GradType.Vertical;

            _trackingBox = new TrackingBox();
            
            this.PropertyChanged += TernaryDiagram_PropertyChanged;

            _diagramAreas = new DiagramAreaCollection();
            _diagramAreas.CollectionChanged += DiagramAreas_CollectionChanged;
            _diagramAreas.Add(new DiagramArea());

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
            diagramArea.ValidateAndAddNewData(dataTable, dataColumnA, dataColumnB, dataColumnC);
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
            diagramArea.ValidateAndAddNewData(dataTable, dataColumnA, dataColumnB, dataColumnC, dataColumnD);
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        #region Variables
        // Collection of diagram areas
        private DiagramAreaCollection _diagramAreas;

        // Point on the screen with mouse cursor on it
        private PointT _currentPoint;

        // Zoom factor >= 1
        private float _zoomFactor;
        private bool _zoomEnabled = true;

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

        [Description("Background color of the control. If background gradient is enabled then it is mixed with second back color.")]
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

        /// <summary>
        /// Calculates position and size for each DiagramArea 
        /// </summary>
        /// <param name="controlWorkspace">The working area of the control</param>
        private void CalculateAreasInnerPositionsAndSizes(Rectangle controlWorkspace)
        {
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

            // Get the position and size for each DiagramArea
            CalculateAreasInnerPositionsAndSizes(e.ClipRectangle);

            // Shallow copy for reordering
            var orderedDiagramAreas = new List<DiagramArea>(_diagramAreas);
            // Get diagram which is currently pointed with the mouse cursor
            var selectedDiagram = _currentPoint.DiagramArea;
            // If the selected diagram is not null, then we want to print it as the last diagram,
            // so that it can be displayed on top of other diagrams when zoomed in 
            if (selectedDiagram != null && _diagramAreas.Last() != selectedDiagram)
            {
                orderedDiagramAreas.Remove(selectedDiagram);
                orderedDiagramAreas.Add(selectedDiagram);
            }


            foreach (var diagram in orderedDiagramAreas)
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
                Matrix matrix = diagram.TransformMatrix.Clone();
                g.Transform = matrix;

                // Correct bottom margin
                SizeF labelS = g.MeasureString("00%", diagram.AxisC.LabelFont);
                int bottomMargin = diagram.Margin.Bottom + (int)labelS.Width;

                // Correct left & right margin
                SizeF leftLabelSize = g.MeasureString(diagram.AxisB.Title, diagram.AxisB.TitleFont);
                SizeF rightLabelSize = g.MeasureString(diagram.AxisC.Title, diagram.AxisC.TitleFont);
                int leftMargin = diagram.Margin.Left + (int)leftLabelSize.Width + 3;
                int rightMargin = diagram.Margin.Right + (int)rightLabelSize.Width + 3;

                // Triangle
                diagram.DiagramTriangle.Calculate(bounds, topMargin, bottomMargin, leftMargin, rightMargin);

                // Diagram scale "resolution"
                float stepCount = 10;// Scale labels and lines every stepCount %

                float maxFontSize = diagram.Axes.Max(n => n.LabelFont.Size);
                Axis largestFontAxis = diagram.Axes.First(x => x.LabelFont.Size == maxFontSize);
                SizeF axisLabelsSize = g.MeasureString("00%", largestFontAxis.LabelFont);

                // Finds best "resolution"
                if (diagram.DiagramTriangle.SideLength < 10 * axisLabelsSize.Width)
                {
                    // Acceptable resolutions
                    float[] resolutions = new float[] { 10, 5, 4, 2 };
                    while ((diagram.DiagramTriangle.SideLength < stepCount * axisLabelsSize.Width && stepCount >= 0) || (!resolutions.Contains(stepCount) && stepCount >= 0))
                    {
                        stepCount--;
                    }
                }

                // Triangle background
                g.FillPath(new SolidBrush(diagram.DiagramTriangle.BackColor), diagram.DiagramTriangle.Path);

                for (int step = 1; step < stepCount; step++)
                {
                    // Chart support lines
                    float abX = diagram.DiagramTriangle.VertexA.X - (step * (diagram.DiagramTriangle.SideLength / (2 * stepCount)));
                    float abY = diagram.DiagramTriangle.VertexA.Y + (step * (diagram.DiagramTriangle.Height / stepCount));
                    // Point on AB axis
                    PointF ab = new PointF(abX, abY);

                    float bcX = diagram.DiagramTriangle.VertexC.X - (step * (diagram.DiagramTriangle.SideLength / stepCount));
                    float bcY = diagram.DiagramTriangle.VertexC.Y;
                    // Point on BC axis
                    PointF bc = new PointF(bcX, bcY);

                    float caX = diagram.DiagramTriangle.VertexA.X + (step * (diagram.DiagramTriangle.SideLength / (2 * stepCount)));
                    float caY = diagram.DiagramTriangle.VertexA.Y + (step * (diagram.DiagramTriangle.Height / stepCount));
                    // Reversed point on CA axis
                    PointF ca = new PointF(caX, caY);

                    float caX2 = diagram.DiagramTriangle.VertexC.X - (step * (diagram.DiagramTriangle.SideLength / (2 * stepCount)));
                    float caY2 = diagram.DiagramTriangle.VertexC.Y - (step * (diagram.DiagramTriangle.Height / stepCount));
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
                    var distanceAB = diagram.AxisB.Maximum - diagram.AxisB.Minimum;
                    string textAB = String.Format("{0}%", diagram.AxisB.Minimum + (distanceAB * step / stepCount));
                    SizeF textABSize = g.MeasureString(textAB, diagram.AxisB.LabelFont);
                    Matrix m = diagram.TransformMatrix.Clone();
                    m.RotateAt(60, new PointF(abX, abY), MatrixOrder.Prepend);
                    g.Transform = m;
                    g.DrawString(textAB, diagram.AxisB.LabelFont, new SolidBrush(diagram.AxisB.LabelColor), abX - textABSize.Width, abY - textABSize.Height / 2);
                    g.Transform = matrix;

                    var distanceBC = diagram.AxisC.Maximum - diagram.AxisC.Minimum;
                    string textBC = String.Format("{0}%", diagram.AxisC.Maximum - (distanceBC * step / stepCount));
                    SizeF textBCSize = g.MeasureString(textBC, diagram.AxisC.LabelFont);
                    m = diagram.TransformMatrix.Clone();
                    m.RotateAt(-60, new PointF(bcX, bcY + 15), MatrixOrder.Prepend);
                    g.Transform = m;
                    g.DrawString(textBC, diagram.AxisC.LabelFont, new SolidBrush(diagram.AxisC.LabelColor), bcX - textBCSize.Width / 2, bcY - textBCSize.Height / 2 + 15);
                    g.Transform = matrix;

                    var distanceCA = diagram.AxisA.Maximum - diagram.AxisA.Minimum;
                    string textCA = String.Format("{0}%", diagram.AxisA.Minimum + (distanceCA * step / stepCount));
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
                    rot = diagram.TransformMatrix.Clone();
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
                    rot = diagram.TransformMatrix.Clone();
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
                    rot = diagram.TransformMatrix.Clone();
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

        private void Area_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Invalidate();
        }

        private void DiagramAreas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                // Give unique name to every new area with empty Name property
                foreach (var area in e.NewItems.Cast<DiagramArea>())
                {
                    if (string.IsNullOrEmpty(area.Name))
                    {
                        area.Name = GetUniqueName(_diagramAreas);
                    }
                }
            }

            // Subscribe to the event
            foreach (var area in _diagramAreas)
            {
                area.PropertyChanged -= Area_PropertyChanged;
                area.PropertyChanged += Area_PropertyChanged;
            }
        }

        /// <summary>
        /// Gets unique name, that is not set for another named object in the collection
        /// </summary>
        /// <param name="collection">The collection with named objects</param>
        /// <returns>Returns unique name or empty string if name couldn't be found</returns>
        private string GetUniqueName<T>(IEnumerable<T> collection) where T : IDiagramNamedElement
        {
            var uniqueName = "";
            if (collection != null)
            {
                var baseName = typeof(T).Name;
                for (int suffix = 1; suffix < Int32.MaxValue; suffix++)
                {
                    uniqueName = $"{baseName}{suffix}";
                    if (!collection.Any(el => el.Name == uniqueName))
                    {
                        break;
                    }
                }
            }
            return uniqueName;
        }

        private void TernaryDiagram_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
            if (this._trackingBox.Enabled)
            {
                PointF pos = this.PointToClient(Cursor.Position);

                _currentPoint = ScreenToDiagramPoint(pos.X, pos.Y);

                Graphics g = this.CreateGraphics();

                DrawTrackingBox(g);
                //this.Invalidate();
            }

            base.OnMouseMove(e);
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

                diagram.ValueGradient.Location = new Point(diagram.WorkingArea.Right - diagram.Margin.Right - valueGradient.Size.Width - margLen, diagram.WorkingArea.Top + diagram.Margin.Top);

                // Title
                int titleMargin = 0;
                if (!String.IsNullOrEmpty(valueGradient.Title))
                {
                    SizeF size = g.MeasureString(valueGradient.Title, valueGradient.TitleFont);
                    float titleX = valueGradient.Location.X - (size.Width / 2) + (valueGradient.Size.Width / 2);
                    float titleY = valueGradient.Location.Y;
                    g.DrawString(valueGradient.Title, valueGradient.TitleFont, new SolidBrush(valueGradient.TitleColor), new Point((int)titleX, (int)titleY));
                    titleMargin = (int)size.Height + 15;
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
            if (diagram.DiagramPoints.Any())
            {
                var valueGradient = diagram.ValueGradient;

                foreach (var point in diagram.DiagramPoints)
                {
                    Color pointColor = diagram.MarkerDefaultColor;
                    if (valueGradient.Enabled && !Double.IsNaN(point.Value))
                    {
                        pointColor = valueGradient.GetColorForDiagramValue(point.Value);
                    }

                    // Draw point
                    switch (diagram.MarkerType)
                    {
                        case MarkerType.Circle:
                            g.FillEllipse(new SolidBrush(pointColor), point.MarkerRect);
                            break;
                        case MarkerType.Square:
                            g.FillRectangle(new SolidBrush(pointColor), point.MarkerRect);
                            break;
                        case MarkerType.Triangle:
                            g.FillPolygon(new SolidBrush(pointColor), new PointF[]
                            {
                                new PointF(point.MarkerRect.Left, point.MarkerRect.Top),
                                new PointF(point.MarkerRect.Right, point.MarkerRect.Top),
                                new PointF(point.MarkerRect.Left + point.MarkerRect.Width/2, point.MarkerRect.Bottom)
                            });
                            break;
                    }
                }
            }
        }

        private void DrawTrackingBox(Graphics g)
        {
            TrackingBox box = this._trackingBox;
            // Diagram area current point belongs to
            DiagramArea diagram = _currentPoint.DiagramArea;

            if (box.Enabled && diagram != null && !box.DisplayedPoint.Equals(_currentPoint))
            {
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
            // Find diagram area that the mouse cursor is pointing to
            DiagramArea diagram = this.DiagramAreas.FirstOrDefault(da => da.WorkingArea.Contains(origin.X, origin.Y));

            Matrix matrix = diagram.TransformMatrix;
            matrix.Translate(-origin.X, -origin.Y, MatrixOrder.Append);
            matrix.Scale(scale, scale, MatrixOrder.Append);
            matrix.Translate(origin.X, origin.Y, MatrixOrder.Append);

            diagram.TransformMatrix = matrix;
        }

        /// <summary>
        /// Computes the location of ABC diagram point in control coordinates
        /// </summary>
        /// <param name="_coordA">Ternary coordinate A</param>
        /// <param name="_coordB">Ternary coordinate B</param>
        /// <param name="_coordC">Ternary coordinate C</param>
        /// <param name="diagram">The diagram area</param>
        /// <returns>PointF object with control coordinates</returns>
        public PointF DiagramToScreenPoint(DiagramArea diagram, float _coordA, float _coordB, float _coordC)
        {
            // B value on AB axis
            float bPctRange = diagram.AxisB.Maximum - diagram.AxisB.Minimum;
            float abX = diagram.DiagramTriangle.VertexA.X - (_coordB - diagram.AxisB.Minimum) * (diagram.DiagramTriangle.SideLength / (2 * bPctRange));
            float abY = diagram.DiagramTriangle.VertexA.Y + (_coordB - diagram.AxisB.Minimum) * (diagram.DiagramTriangle.Height / bPctRange);
            PointF axisBval = new PointF(abX, abY);

            // C value on BC axis
            float cPctRange = diagram.AxisC.Maximum - diagram.AxisC.Minimum;
            float onePctInPixels = diagram.DiagramTriangle.SideLength / cPctRange;
            float bcX = diagram.DiagramTriangle.VertexB.X + (_coordC - diagram.AxisC.Minimum) * onePctInPixels;
            float bcY = diagram.DiagramTriangle.VertexB.Y;
            PointF axisCval = new PointF(bcX, bcY);

            // A value on CA axis
            float aPctRange = diagram.AxisA.Maximum - diagram.AxisA.Minimum;
            float caX = diagram.DiagramTriangle.VertexC.X - (_coordA - diagram.AxisA.Minimum) * (diagram.DiagramTriangle.SideLength / (2 * aPctRange));
            float caY = diagram.DiagramTriangle.VertexC.Y - (_coordA - diagram.AxisA.Minimum) * (diagram.DiagramTriangle.Height / aPctRange);
            PointF axisAval = new PointF(caX, caY);

            // y=ax+b
            // Coefficient "a" of line creating axis AB
            float coefAab = (diagram.DiagramTriangle.VertexA.Y - diagram.DiagramTriangle.VertexB.Y) / (diagram.DiagramTriangle.VertexA.X - diagram.DiagramTriangle.VertexB.X);

            // Coefficient "b" of line going through the diagram point given by percA, percB and percC and crossing axis BC
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
            var nullPoint = new PointT(float.NaN, float.NaN, float.NaN, float.NaN, null);

            // Find diagram area that the mouse cursor is pointing to
            DiagramArea diagram = this.DiagramAreas.FirstOrDefault(da=>da.WorkingArea.Contains((int)px, (int)py));

            if (diagram == null || diagram.DiagramTriangle.Path == null || diagram.DiagramTriangle.Path.PointCount == 0) return nullPoint;

            PointF[] tpts = new PointF[diagram.DiagramTriangle.Path.PointCount];
            tpts = diagram.DiagramTriangle.Path.PathPoints;
            diagram.TransformMatrix.TransformPoints(tpts);
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

                // Transform points according to scale of the triangle
                PointF[] pts = new PointF[] { diagram.DiagramTriangle.VertexA, diagram.DiagramTriangle.VertexB, diagram.DiagramTriangle.VertexC };
                diagram.TransformMatrix.TransformPoints(pts);
                float tSideLength = pts[2].X - pts[1].X; //Should be the same as diagram.DiagramTriangle.SideLength

                float coefAab = (pts[0].Y - pts[1].Y) / (pts[0].X - pts[1].X);
                float coefBbcv = py - coefAab * px;
                float ycv = pts[1].Y;
                float xcv = (ycv - coefBbcv) / coefAab;

                float lenBcv = xcv - pts[1].X;
                float Cval = diagram.AxisC.Minimum + (lenBcv / tSideLength) * (diagram.AxisC.Maximum - diagram.AxisC.Minimum);

                float coefAca = -coefAab;
                float coefBca = pts[2].Y - coefAca * pts[2].X;

                float yav = py;
                float xav = (py - coefBca) / coefAca;
                float lenCav = (float)Math.Sqrt(Math.Pow(xav - pts[2].X, 2) + Math.Pow(yav - pts[2].Y, 2));
                float Aval = diagram.AxisA.Minimum + (lenCav / tSideLength) * (diagram.AxisA.Maximum - diagram.AxisA.Minimum);

                float Bval = 100 - Cval - Aval;
                return new PointT(Aval, Bval, Cval, double.NaN, diagram);
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

        #region Events
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
