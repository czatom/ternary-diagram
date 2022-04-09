using System.ComponentModel;
using System.Drawing;

namespace TernaryDiagramLib
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DefaultProperty("Enabled")]
    public class Arrow : DiagramElement
    {
        internal void Initialize()
        {
            this._color = Color.Black;
            this._labelFont = new Font(FontFamily.GenericSansSerif, 10);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Arrow()
        {
            this._labelText = "";
            this.Initialize();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="label_text">Text of the arrow label</param>
        public Arrow(string label_text)
        {
            this._labelText = label_text;
            this.Initialize();
        }

        #region Properties
        private bool _enabled = true;
        [Category("Behavior")]
        [Description("Enable or disable arrow")]
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

        private string _labelText;
        [Category("Appearance")]
        [Description("Gets or sets text of the arrow label")]
        [DefaultValue("")]
        public string LabelText
        {
            get { return _labelText; }
            set
            {
                _labelText = value;
                OnChanged(this, new PropertyChangedEventArgs("LabelText"));
            }
        }

        private Color _color;
        [Category("Appearance")]
        [Description("Gets or sets color of the arrow")]
        [DefaultValue(typeof(Color), "Black")]
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnChanged(this, new PropertyChangedEventArgs("Color"));
            }
        }

        private Font _labelFont;
        [Category("Appearance")]
        [Description("Gets or sets font of the arrow label")]
        [DefaultValue(typeof(Font), "Arial, 10pt")]
        public Font LabelFont
        {
            get { return _labelFont; }
            set
            {
                _labelFont = value;
                OnChanged(this, new PropertyChangedEventArgs("LabelFont"));
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
