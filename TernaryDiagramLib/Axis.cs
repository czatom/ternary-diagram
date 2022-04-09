using System;
using System.ComponentModel;
using System.Drawing;

namespace TernaryDiagramLib
{
    public class Axis : DiagramElement, INotifyPropertyChanged
    {
        internal void Initialize()
        {
            this._titleFont = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold);
            this._titleColor = Color.Black;
            this._labelFont = new Font(FontFamily.GenericSansSerif, 10);
            this._labelColor = Color.Black;
            this._grid = new Grid();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Axis()
        {
            this._name = "";
            this._title = "";
            this._supportArrow = new Arrow();
            this.Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="axis_name">Name of the axis</param>
        public Axis(string axis_name)
        {
            this._name = axis_name;
            this._title = axis_name;
            this._supportArrow = new Arrow();
            this._supportArrow.LabelText = axis_name;
            this.Initialize();
        }

        #region Properties
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
        #endregion //Properties 

        #region Overrides
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(_title))
                return String.Format("Axis {0}", _title);
            else
                return base.ToString();
        }
        #endregion //Overrides

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
                    Grid.PropertyChanged += OnChanged;
                    SupportArrow.PropertyChanged += OnChanged;
                }
            }
            remove
            {
                Grid.PropertyChanged -= OnChanged;
                SupportArrow.PropertyChanged -= OnChanged;
            }
        }

        private void OnChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_propertyChanged != null)
            {
                _propertyChanged(this, e);
            }
        }
        #endregion //Events
    }
}
