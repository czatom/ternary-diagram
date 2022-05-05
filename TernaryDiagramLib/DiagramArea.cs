using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace TernaryDiagramLib
{
    [DefaultProperty("Axes")]
    public class DiagramArea : DiagramElement, IDiagramNamedElement
    {
        internal void Initialize()
        {
            // Create diagram data table
            _diagramDataTable = new DataTable("Diagram");
            _diagramDataTable.Columns.Add("A", typeof(double));
            _diagramDataTable.Columns.Add("B", typeof(double));
            _diagramDataTable.Columns.Add("C", typeof(double));
            _diagramDataTable.Columns.Add("Value", typeof(double));

            _diagramPoints = new List<PointT>();

            this.axisA = new Axis("A");
            this.axisB = new Axis("B");
            this.axisC = new Axis("C");
            this._axes = new List<Axis>(3) { axisA, axisB, axisC };

            AxisA.PropertyChanged += OnChanged;
            AxisB.PropertyChanged += OnChanged;
            AxisC.PropertyChanged += OnChanged;

            this._markerSize = 3;
            this._markerType = MarkerType.Triangle;
            this._markerDefaultColor = Color.Black;
            this._backColor1 = Color.White;
            this._backColor2 = Color.Transparent;
            this._gradType = GradType.Vertical;
            this._diagramTriangle = new Triangle();
            this._title = "";
            this._titleFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
            this._titleColor = Color.Black;

            _valueGradient = new ValueGradient();
            ValueGradient.PropertyChanged += OnChanged;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DiagramArea()
        {
            this.Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="diagram_name">Name of the diagram</param>
        public DiagramArea(string diagram_name)
        {
            this._name = diagram_name;
            this.Initialize();
        }

        private readonly HashSet<Type> _numericTypes = new HashSet<Type>
        {
            typeof(decimal), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(double)
        };

        #region Properties
        private DataTable _sourceDataTable;
        /// <summary>
        /// Source data table
        /// </summary>
        [Category("Data")]
        [Description("Source data table")]
        [NotifyParentProperty(true)]
        public DataTable SourceDataTable
        {
            get { return _sourceDataTable; }
            set { _sourceDataTable = value; }
        }

        private DataTable _diagramDataTable;
        /// <summary>
        /// Diagram data table with underlying data
        /// </summary>
        [Category("Data")]
        [Description("Source data table")]
        public DataTable DiagramDataTable
        {
            get { return _diagramDataTable; }
        }

        List<PointT> _diagramPoints;
        /// <summary>
        /// Gets list of ternary diagram points
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public List<PointT> DiagramPoints
        {
            get { return _diagramPoints; }
        }

        private float _minimumA;
        /// <summary>
        /// Gets the minimum value from column A
        /// </summary>
        [Category("Data")]
        [Description("Minimum value of A")]
        public float MinimumA
        {
            get { return _minimumA; }
        }

        private float _maximumA;
        /// <summary>
        /// Gets the maximum value from column A
        /// </summary>
        [Category("Data")]
        [Description("Maximum value of A")]
        public float MaximumA
        {
            get { return _maximumA; }
        }

        private float _minimumB;
        /// <summary>
        /// Gets the minimum value from column B
        /// </summary>
        [Category("Data")]
        [Description("Minimum value of B")]
        public float MinimumB
        {
            get { return _minimumB; }
        }

        private float _maximumB;
        /// <summary>
        /// Gets the maximum value from column B
        /// </summary>
        [Category("Data")]
        [Description("Maximum value of B")]
        public float MaximumB
        {
            get { return _maximumB; }
        }

        private float _minimumC;
        /// <summary>
        /// Gets the minimum value from column C
        /// </summary>
        [Category("Data")]
        [Description("Minimum value of C")]
        public float MinimumC
        {
            get { return _minimumC; }
        }

        private float _maximumC;
        /// <summary>
        /// Gets the maximum value from column A
        /// </summary>
        [Category("Data")]
        [Description("Maximum value of C")]
        public float MaximumC
        {
            get { return _maximumC; }
        }

        private string _name;
        /// <summary>
        /// Name of the diagram area
        /// </summary>
        [Category("Design")]
        [Description("Name of the area")]
        [NotifyParentProperty(true)]
        [DefaultValue("")]
        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
                OnChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        private Rectangle _workingArea;
        /// <summary>
        /// Size and coordinates of the upper-left corner of diagram's working area relative to upper-left corner of the control
        /// </summary>
        [Category("Layout")]
        [Description("Working area")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle WorkingArea
        {
            get { return _workingArea; }
            set 
            {
                if (_workingArea != value)
                {
                    _workingArea = value;
                    OnChanged(this, new PropertyChangedEventArgs("WorkingArea"));
                    OnChanged(this, new PropertyChangedEventArgs("Size"));
                    OnChanged(this, new PropertyChangedEventArgs("Location"));
                }
            }
        }

        /// <summary>
        /// Size of the diagram area
        /// </summary>
        [Category("Layout")]
        [Description("Size of the area")]
        public Size Size
        { 
            get { return _workingArea.Size; }
        }

        /// <summary>
        /// Coordinates of the upper-left corner of diagram's working area relative to upper-left corner of the control
        /// </summary>
        [Category("Layout")]
        [Description("Coordinates of the upper-left corner of diagram's working area relative to upper-left corner of the control")]
        public Point Location
        {
            get { return _workingArea.Location; }
        }

        private Padding _margin = new Padding(0, 0, 0, 0);
        /// <summary>
        /// Gets or sets the space between other diagram areas or main control
        /// </summary>
        [Category("Layout")]
        [Description("Specifies space between this diagram area and other diagram areas or main control")]
        public Padding Margin
        {
            get
            {
                return _margin;
            }
            set
            {
                if (value != Margin)
                {
                    _margin = value;
                    OnChanged(this, new PropertyChangedEventArgs("Margin"));
                }
            }
        }

        private List<Axis> _axes;
        [Category("Axes")]
        [Description("Collection of diagram axes")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Editor(typeof(AxesCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(AxesListConverter))]
        public IReadOnlyList<Axis> Axes
        {
            get { return _axes.AsReadOnly(); }
        }

        private Axis axisA;
        /// <summary>
        /// Right axis of the ternary diagram
        /// </summary>
        [Category("Axes")]
        [Description("Gets right axis of the ternary diagram")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Axis AxisA
        {
            get { return this.axisA; }
            set
            {
                this.axisA = value;
                this._axes[0] = this.axisA;
            }
        }

        private Axis axisB;
        /// <summary>
        /// Left axis of the ternary diagram
        /// </summary>
        [Category("Axes")]
        [Description("Gets left axis of the ternary diagram")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Axis AxisB
        {
            get { return axisB; }
            set
            {
                this.axisB = value;
                this._axes[1] = this.axisB;
            }
        }

        private Axis axisC;
        /// <summary>
        /// Bottom axis of the ternary diagram
        /// </summary>
        [Category("Axes")]
        [Description("Gets bottom axis of the ternary diagram")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Browsable(false)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Axis AxisC
        {
            get { return axisC; }
            set
            {
                this.axisC = value;
                this._axes[2] = this.axisC;
            }
        }

        private bool _autoScale;
        /// <summary>
        /// Enables or disables automatic scaling of the axes
        /// </summary>
        [Category("Axes")]
        [Description("Enables or disables automatic scaling of the axes")]
        [NotifyParentProperty(true)]
        public bool AutoScale
        {
            get { return _autoScale; }
            set { _autoScale = value; }
        }

        private int _markerSize;
        /// <summary>
        /// Size of the point marker
        /// </summary>
        [Category("Points")]
        [Description("Gets or sets size of the point marker")]
        [DefaultValue(3)]
        [NotifyParentProperty(true)]
        public int MarkerSize
        {
            get { return _markerSize; }
            set
            {
                _markerSize = value;
                OnChanged(this, new PropertyChangedEventArgs("MarkerSize"));
            }
        }

        private MarkerType _markerType;
        /// <summary>
        /// Shape of the point marker
        /// </summary>
        [Category("Points")]
        [Description("Gets or sets shape of the point marker")]
        [DefaultValue(MarkerType.Triangle)]
        [NotifyParentProperty(true)]
        public MarkerType MarkerType
        {
            get { return _markerType; }
            set
            {
                _markerType = value;
                OnChanged(this, new PropertyChangedEventArgs("MarkerType"));
            }
        }

        private Color _markerDefaultColor;
        /// <summary>
        /// Default color of marker. Used in case value gradient is disabled or there is not value data.
        /// </summary>
        [Category("Points")]
        [Description("Default color of marker. Used in case value gradient is disabled or there is not value data.")]
        [DefaultValue(typeof(Color), "Black")]
        [NotifyParentProperty(true)]
        public Color MarkerDefaultColor
        {
            get { return _markerDefaultColor; }
            set
            {
                _markerDefaultColor = value;
                OnChanged(this, new PropertyChangedEventArgs("MarkerType"));
            }
        }

        private Matrix _transformMatrix = new Matrix();
        /// <summary>
        /// Transformation matrix used for zooming
        /// </summary>
        [Category("Appearance")]
        [Description("Transformation matrix")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Matrix TransformMatrix
        {
            get { return _transformMatrix; }
            set { _transformMatrix = value; }
        }

        private Color _backColor1;
        [Category("Appearance")]
        [Description("Gets or sets first background color")]
        [DefaultValue(typeof(Color), "White")]
        [NotifyParentProperty(true)]
        public Color BackColor1
        {
            get { return _backColor1; }
            set
            {
                _backColor1 = value;
                OnChanged(this, new PropertyChangedEventArgs("BackColor1"));
            }
        }

        private Color _backColor2;
        [Category("Appearance")]
        [Description("Gets or sets second background color")]
        [DefaultValue(typeof(Color), "Transparent")]
        [NotifyParentProperty(true)]
        public Color BackColor2
        {
            get { return _backColor2; }
            set
            {
                _backColor2 = value;
                OnChanged(this, new PropertyChangedEventArgs("BackColor2"));
            }
        }

        private bool _backGradientEnabled;
        /// <summary>
        /// Enable or disable background gradient
        /// </summary>
        [Category("Appearance")]
        [Description("Background gradient on/off")]
        [DefaultValue(false)]
        [NotifyParentProperty(true)]
        public bool BackGradientEnabled
        {
            get { return _backGradientEnabled; }
            set
            {
                if (this.BackColor1 == Color.Transparent) this._backColor1 = Color.White;
                if (this.BackColor2 == Color.Transparent) this._backColor2 = Color.White;
                _backGradientEnabled = value;
                OnChanged(this, new PropertyChangedEventArgs("BackGradientEnabled"));
            }
        }

        private GradType _gradType;
        /// <summary>
        /// Type of the background gradient
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets type of background gradient - vertical or horizontal")]
        [DefaultValue(GradType.Vertical)]
        [NotifyParentProperty(true)]
        public GradType BackgroundGradientType
        {
            get { return _gradType; }
            set
            {
                _gradType = value;
                OnChanged(this, new PropertyChangedEventArgs("BackgroundGradientType"));
            }
        }

        private Triangle _diagramTriangle;
        [Category("Diagram")]
        [Description("Gets diagram triangle")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(TriangleConverter))]
        public Triangle DiagramTriangle
        {
            get { return _diagramTriangle; }
        }

        private string _title;
        /// <summary>
        /// Diagram title
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets diagram title")]
        [NotifyParentProperty(true)]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnChanged(this, new PropertyChangedEventArgs("Title"));
            }
        }

        private Font _titleFont;
        /// <summary>
        /// Font of the diagram title
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets diagram title font")]
        [DefaultValue(typeof(Font), "Arial, 12pt, style=Bold")]
        [NotifyParentProperty(true)]
        public Font TitleFont
        {
            get { return _titleFont; }
            set
            {
                _titleFont = value;
                OnChanged(this, new PropertyChangedEventArgs("TitleFont"));
            }
        }

        private Color _titleColor;
        /// <summary>
        /// Color of the diagram title
        /// </summary>
        [Category("Appearance")]
        [Description("Diagram title color")]
        [DefaultValue(typeof(Color), "Black")]
        [NotifyParentProperty(true)]
        public Color TitleColor
        {
            get { return _titleColor; }
            set
            {
                _titleColor = value;
                OnChanged(this, new PropertyChangedEventArgs("TitleColor"));
            }
        }

        private ValueGradient _valueGradient;
        /// <summary>
        /// Color gradient of the D value scale
        /// </summary>
        [Category("Value gradient")]
        [Description("Color gradient of the D value scale")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ValueGradientConverter))]
        public ValueGradient ValueGradient
        {
            get { return _valueGradient; }
            set
            {
                _valueGradient = value;
                _valueGradient.IgnoredColors.Clear();
                //_gradient.ExclusiveColors.AddRange(new int[] { _ab_to_bc_LineColor.ToArgb(), _bc_to_ca_LineColor.ToArgb(), _ca_to_ab_LineColor.ToArgb(), _triangleBorderColor.ToArgb(), _triangleBackgroundColor.ToArgb() });
            }
        }
        #endregion // Properties

        #region Methods
        public void LoadData(DataTable dataTable, DataColumn dataColumnA, DataColumn dataColumnB, DataColumn dataColumnC, DataColumn dataColumnD = null)
        {
            _sourceDataTable = dataTable;
            ValidateAndAddNewData(dataTable, dataColumnA, dataColumnB, dataColumnC, dataColumnD);
        }

        private bool IsOfNumericType(DataColumn column)
        {
            return _numericTypes.Contains(column?.DataType);
        }

        /// <summary>
        /// Adds to diagram data table correct records and creates list of ternary points
        /// </summary>
        /// <param name="dataTable">Source data table</param>
        /// <param name="dataColumnA">Source data column A</param>
        /// <param name="dataColumnB">Source data column B</param>
        /// <param name="dataColumnC">Source data column C</param>
        /// <param name="dataColumnD">Source data column D</param>
        public void ValidateAndAddNewData(DataTable dataTable, DataColumn dataColumnA, DataColumn dataColumnB, DataColumn dataColumnC, DataColumn dataColumnD = null)
        {
            var minA = 100f;
            var maxA = 0f;
            var minB = 100f;
            var maxB = 0f;
            var minC = 100f;
            var maxC = 0f;
            var minVal = double.MaxValue;
            var maxVal = double.MinValue;

            // Clear old data
            _diagramDataTable.Clear();
            DiagramPoints.Clear();

            // Check if columns are of numeric type
            if (!IsOfNumericType(dataColumnA)) throw new ArgumentException($"Data from column {dataColumnA.ColumnName} is not of numeric type");
            if (!IsOfNumericType(dataColumnB)) throw new ArgumentException($"Data from column {dataColumnB.ColumnName} is not of numeric type");
            if (!IsOfNumericType(dataColumnC)) throw new ArgumentException($"Data from column {dataColumnC.ColumnName} is not of numeric type");
            if (dataColumnD != null && !IsOfNumericType(dataColumnD)) throw new ArgumentException($"Data from column {dataColumnD.ColumnName} is not of numeric type");

            foreach (DataRow dataRow in dataTable.Rows)
            {
                // For columns A, B and C each value should be between 0-100
                var coordinateA = Convert.ToSingle(dataRow[dataColumnA]);
                var coordinateB = Convert.ToSingle(dataRow[dataColumnB]);
                var coordinateC = Convert.ToSingle(dataRow[dataColumnC]);

                if (coordinateA < 0 || coordinateA > 100)
                {
                    throw new ArgumentException($"Incorrect value {coordinateA} in column {dataColumnA.ColumnName} and row {dataTable.Rows.IndexOf(dataRow)}. Value should be between 0-100.");
                }

                if (coordinateB < 0 || coordinateB > 100)
                {
                    throw new ArgumentException($"Incorrect value {coordinateB} in column {dataColumnB.ColumnName} and row {dataTable.Rows.IndexOf(dataRow)}. Value should be between 0-100");
                }

                if (coordinateC < 0 || coordinateC > 100)
                {
                    throw new ArgumentException($"Incorrect value {coordinateC} in column {dataColumnC.ColumnName} and row {dataTable.Rows.IndexOf(dataRow)}. Value should be between 0-100");
                }

                // Sum of values from columns A, B and C should be equal 100 with some tolerance
                double total = coordinateA + coordinateB + coordinateC;
                if (Math.Abs(100 - total) < 0.001)
                {
                    double value = double.NaN;

                    if (dataColumnD != null)
                    {
                        value = Convert.ToDouble(dataRow[dataColumnD.ColumnName]);
                        minVal = Math.Min(minVal, value);
                        maxVal = Math.Max(maxVal, value);
                    }

                    minA = Math.Min(minA, coordinateA);
                    maxA = Math.Max(maxA, coordinateA);
                    minB = Math.Min(minB, coordinateB);
                    maxB = Math.Max(maxB, coordinateB);
                    minC = Math.Min(minC, coordinateC);
                    maxC = Math.Max(maxC, coordinateC);

                    // Finally add correct data row to the internal data table
                    _diagramDataTable.Rows.Add(new object[] { coordinateA, coordinateB, coordinateC, value });

                    // Create new ternary point and add it to the list
                    PointT abc = new PointT(coordinateA, coordinateB, coordinateC, value, this);
                    DiagramPoints.Add(abc);
                }
                else
                {
                    throw new ArgumentException($"Incorrect sum of values {coordinateA}, {coordinateB} and {coordinateC} in row {dataTable.Rows.IndexOf(dataRow)}. The sum is {total}, but should be 100.");
                }
            }

            ValueGradient.Minimum = minVal != double.MaxValue ? minVal : double.NaN;
            ValueGradient.Maximum = maxVal != double.MinValue ? maxVal : double.NaN;

            _minimumA = minA;
            _maximumA = maxA;
            _minimumB = minB;
            _maximumB = maxB;
            _minimumC = minC;
            _maximumC = maxC;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(_name))
                return base.ToString();
            else
                return _name;
        }
        #endregion // Methods

        #region Events
        private event PropertyChangedEventHandler _propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add => _propertyChanged += value;
            remove => _propertyChanged -= value;
        }

        protected void OnChanged(object sender, PropertyChangedEventArgs e)
        {
            // Recalculate min and max for each axis if one of those values has changed
            if (sender is Axis && e.PropertyName == "Minimum" || e.PropertyName == "Maximum")
            {
                AxisA.PropertyChanged -= OnChanged;
                AxisB.PropertyChanged -= OnChanged;
                AxisC.PropertyChanged -= OnChanged;

                var changedAxis = sender as Axis;
                var axisBefore = Axes[(Axes.ToList().IndexOf(changedAxis) + 2) % Axes.Count];
                var axisAfter = Axes[(Axes.ToList().IndexOf(changedAxis) + 1) % Axes.Count];

                if (e.PropertyName == "Minimum")
                {
                    axisBefore.Maximum = 100 - axisAfter.Minimum - changedAxis.Minimum;
                    axisAfter.Maximum = 100 - axisBefore.Minimum - changedAxis.Minimum;
                }

                if (e.PropertyName == "Maximum")
                {
                    axisBefore.Minimum = 100 - axisAfter.Minimum - changedAxis.Maximum;
                    axisAfter.Maximum = 100 - axisBefore.Minimum - changedAxis.Minimum;
                }

                AxisA.PropertyChanged += OnChanged;
                AxisB.PropertyChanged += OnChanged;
                AxisC.PropertyChanged += OnChanged;
            }

            _propertyChanged?.Invoke(this, e);
        }
        #endregion // Events
    }
}
