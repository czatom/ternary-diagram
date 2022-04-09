using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TernaryDiagramLib
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Grid : DiagramElement
    {
        internal void Initialize()
        {
            this._lineDashStyle = DashStyle.Dash;
            this._lineWidth = 1;
            this._lineColor = Color.Gray;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Grid()
        {
            this.Initialize();
        }

        #region Properties
        private bool _enabled = true;
        [Category("Behavior")]
        [Description("Enable or disable grid")]
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

        private DashStyle _lineDashStyle;
        [Category("Appearance")]
        [Description("Gets ot sets style of the grid lines")]
        [DefaultValue(DashStyle.Dash)]
        public DashStyle LineDashStyle
        {
            get { return _lineDashStyle; }
            set
            {
                _lineDashStyle = value;
                OnChanged(this, new PropertyChangedEventArgs("LineDashStyle"));
            }
        }

        private int _lineWidth;
        [Category("Appearance")]
        [Description("Gets or sets width of the grid lines")]
        [DefaultValue(1)]
        public int LineWidth
        {
            get { return _lineWidth; }
            set
            {
                _lineWidth = value;
                OnChanged(this, new PropertyChangedEventArgs("LineWidth"));
            }
        }

        private Color _lineColor;
        [Category("Appearance")]
        [Description("Gets or sets color of the grid lines")]
        [DefaultValue(typeof(Color), "Gray")]
        public Color LineColor
        {
            get { return _lineColor; }
            set
            {
                _lineColor = value;
                OnChanged(this, new PropertyChangedEventArgs("LineColor"));
            }
        }
        #endregion //Properties

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion //Events
    }
}
