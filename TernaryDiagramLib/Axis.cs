using System;
using System.ComponentModel;
using System.Drawing;

namespace TernaryDiagramLib
{
    public class Axis : DiagramElement, INotifyPropertyChanged
    {
        internal void Initialize(string axis_name="")
        {
            this._name = axis_name;
            this._title = axis_name;

            this._supportArrow = new Arrow();
            this._supportArrow.LabelText = axis_name;
            SupportArrow.PropertyChanged += OnChanged;

            _titleFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
            _titleColor = Color.Black;
            _labelFont = new Font(FontFamily.GenericSansSerif, 10);
            _labelColor = Color.Black;
            
            _grid = new Grid();
            Grid.PropertyChanged += OnChanged;

            _minimum = 0;
            _maximum = 100;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Axis()
        {
            this.Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="axis_name">Name of the axis</param>
        public Axis(string axis_name)
        {
            this.Initialize();
        }

        #region Properties
        private float _minimum;
        /// <summary>
        /// Minimum value on the axis
        /// </summary>
        [Category("Scale")]
        [Description("Minimum value on the axis")]
        public float Minimum
        {
            get { return _minimum; }
            set 
            {
                if (value >= 0 && value <= 100 && value < Maximum)
                {
                    _minimum = value;
                    OnChanged(this, new PropertyChangedEventArgs("Minimum"));
                }
            }
        }

        private float _maximum;
        /// <summary>
        /// Maximum value on the axis
        /// </summary>
        [Category("Scale")]
        [Description("Maximum value on the axis")]
        public float Maximum
        {
            get { return _maximum; }
            set
            {
                if (value >= 0 && value <= 100 && value > Minimum)
                {
                    _maximum = value;
                    OnChanged(this, new PropertyChangedEventArgs("Maximum"));
                }
            }
        }

        private string _name;
        [Category("Design")]
        [Description("Name of the axis")]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnChanged(this, new PropertyChangedEventArgs("Name"));
            }
        }

        private string _title;
        [Category("Appearance")]
        [Description("Title of the axis")]
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
        [Category("Appearance")]
        [Description("Font of the axis title")]
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
        [Category("Appearance")]
        [Description("Color of the axis title")]
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

        private Font _labelFont;
        [Category("Appearance")]
        [Description("Font of the axis labels")]
        [DefaultValue(typeof(Font), "Arial, 10pt")]
        [NotifyParentProperty(true)]
        public Font LabelFont
        {
            get { return _labelFont; }
            set
            {
                _labelFont = value;
                OnChanged(this, new PropertyChangedEventArgs("LabelFont"));
            }
        }

        private Color _labelColor;
        [Category("Appearance")]
        [Description("Color of the axis labels")]
        [DefaultValue(typeof(Color), "Black")]
        [NotifyParentProperty(true)]
        public Color LabelColor
        {
            get { return _labelColor; }
            set
            {
                _labelColor = value;
                OnChanged(this, new PropertyChangedEventArgs("LabelColor"));
            }
        }

        private Grid _grid;
        [Category("Appearance")]
        [Description("Axis grid")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public Grid Grid
        {
            get { return _grid; }
            set
            {
                _grid = value;
            }
        }

        private Arrow _supportArrow;
        [Category("Appearance")]
        [Description("Arrow indicating what parameter should be read from this axis")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        public Arrow SupportArrow
        {
            get { return _supportArrow; }
            set
            {
                _supportArrow = value;
            }
        }
        #endregion // Properties 

        #region Overrides
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_title))
                return String.Format("Axis {0}", _title);
            else
                return base.ToString();
        }
        #endregion // Overrides

        #region Events
        private event PropertyChangedEventHandler _propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add => _propertyChanged += value;
            remove => _propertyChanged -= value;
        }

        private void OnChanged(object sender, PropertyChangedEventArgs e)
        {
            _propertyChanged?.Invoke(this, e);
        }
        #endregion // Events
    }
}
