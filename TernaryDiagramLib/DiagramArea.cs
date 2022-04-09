using System;
using System.ComponentModel;
using System.Drawing;

namespace TernaryDiagramLib
{
    [DefaultProperty("Axes")]
    public class DiagramArea : DiagramElement
    {
        internal void Initialize()
        {
            this._axes = new Axis[3];
            this.axisA = new Axis("A");
            this.axisB = new Axis("B");
            this.axisC = new Axis("C");
            this._axes[0] = this.axisA;
            this._axes[1] = this.axisB;
            this._axes[2] = this.axisC;

            this._markerSize = 3;
            this._markerType = MarkerType.Triangle;
            this._backColor1 = Color.White;
            this._backColor2 = Color.Transparent;
            this._gradType = GradType.Vertical;
            this._diagramTriangle = new Triangle();
            this._title = "";
            this._titleFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
            this._titleColor = Color.Black;
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

        #region Properties
        private string _name;
        /// <summary>
        /// Name of the diagram area
        /// </summary>
        [Category("Design")]
        [Description("Name of the area")]
        [NotifyParentProperty(true)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        private Axis[] _axes;
        [Category("Axes")]
        [Description("Collection of diagram axes")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Editor(typeof(AxesCollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [TypeConverter(typeof(AxesArrayConverter))]
        public Axis[] Axes
        {
            get { return _axes; }
            set
            {
                this.AxisA = value[0];
                this.AxisB = value[1];
                this.AxisC = value[2];
            }
        }

        internal Axis axisA;
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

        internal Axis axisB;
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

        internal Axis axisC;
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
        #endregion //Properties

        #region Methods
        public override string ToString()
        {
            if (String.IsNullOrEmpty(_name))
                return base.ToString();
            else
                return _name;
        }
        #endregion //Methods

        #region Events
        private event PropertyChangedEventHandler _propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                bool first = _propertyChanged == null;
                _propertyChanged += value;
                if (first && _propertyChanged != null)
                {
                    AxisA.PropertyChanged += OnChanged;
                    AxisB.PropertyChanged += OnChanged;
                    AxisC.PropertyChanged += OnChanged;
                }
            }
            remove
            {
                AxisA.PropertyChanged -= OnChanged;
                AxisB.PropertyChanged -= OnChanged;
                AxisC.PropertyChanged -= OnChanged;
            }
        }

        protected void OnChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = _propertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion //Events
    }
}
