using System.ComponentModel;
using System.Drawing;

namespace TernaryDiagramLib
{
    [DefaultProperty("Enabled")]
    public class TrackingBox
    {
        #region Properties
        private bool _enabled = true;
        [Category("Bahavior")]
        [Description("Enable or disable tracking box")]
        [DefaultValue(true)]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnChanged(this, new PropertyChangedEventArgs("Enabled"));

            }
        }

        private PointT _displayedPoint = new PointT(float.NaN, float.NaN, float.NaN, float.NaN, null);
        [Category("Data")]
        [Description("Last displayed point")]
        public PointT DisplayedPoint
        {
            get { return _displayedPoint; }
            set { _displayedPoint = value; }
        }



        private int _width = 190;
        [Category("Layout")]
        [Description("Gets or sets width of the tracking box")]
        [DefaultValue(190)]
        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
            }
        }

        private Point _location = new Point(10, 10);
        [Category("Layout")]
        [Description("Gets or sets location point of the tracking box")]
        public Point Location
        {
            get { return _location; }
            set
            {
                _location = value;
                // Too many changes
                //OnChanged(this, new PropertyChangedEventArgs("Location"));
            }
        }

        private Color _backColor = Color.LightGray;
        [Category("Appearance")]
        [Description("Gets or sets background color of the tracking box")]
        [DefaultValue(typeof(Color), "LightGray")]
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;
                OnChanged(this, new PropertyChangedEventArgs("BackColor"));

            }
        }

        private Color _foreColorA = Color.Black;
        [Category("Appearance")]
        [Description("Gets or sets fore color of the A component label")]
        [DefaultValue(typeof(Color), "Black")]
        public Color ForeColorA
        {
            get { return _foreColorA; }
            set
            {
                _foreColorA = value;
                OnChanged(this, new PropertyChangedEventArgs("ForeColorA"));
            }
        }

        private Color _foreColorB = Color.Black;
        [Category("Appearance")]
        [Description("Gets or sets fore color of the B component label")]
        [DefaultValue(typeof(Color), "Black")]
        public Color ForeColorB
        {
            get { return _foreColorB; }
            set
            {
                _foreColorB = value;
                OnChanged(this, new PropertyChangedEventArgs("ForeColorB"));
            }
        }

        private Color _foreColorC = Color.Black;
        [Category("Appearance")]
        [Description("Gets or sets fore color of the C component label")]
        [DefaultValue(typeof(Color), "Black")]
        public Color ForeColorC
        {
            get { return _foreColorC; }
            set
            {
                _foreColorC = value;
                OnChanged(this, new PropertyChangedEventArgs("ForeColorC"));
            }
        }

        private int _borderWidth = 1;
        [Category("Appearance")]
        [Description("Gets or sets width of the box border")]
        [DefaultValue(1)]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set
            {
                _borderWidth = value;
                OnChanged(this, new PropertyChangedEventArgs("BorderWidth"));
            }
        }

        private Color _borderColor = Color.Black;
        [Category("Appearance")]
        [Description("Gets or sets color of the box border")]
        [DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                OnChanged(this, new PropertyChangedEventArgs("BorderColor"));
            }
        }

        private Font _labelsFont = new Font(FontFamily.GenericSansSerif, 10);
        [Category("Appearance")]
        [Description("Gets or sets font of the A,B and C component labels")]
        [DefaultValue(typeof(Font), "Arial, 10pt")]
        public Font LabelsFont
        {
            get { return _labelsFont; }
            set
            {
                _labelsFont = value;
                OnChanged(this, new PropertyChangedEventArgs("LabelsFont"));
            }
        }
        #endregion // Properties

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion // Events
    }
}
