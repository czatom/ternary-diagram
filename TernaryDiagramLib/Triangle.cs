using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TernaryDiagramLib
{
    public class Triangle
    {
        #region Properties
        private GraphicsPath _path;
        /// <summary>
        /// Gets path of the diagram triangle
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public GraphicsPath Path
        {
            get { return _path; }
        }

        private PointF _vertexA;
        /// <summary>
        /// Gets point of vertex A
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public PointF VertexA
        {
            get { return _vertexA; }
        }

        private PointF _vertexB;
        /// <summary>
        /// Gets point of vertex B
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public PointF VertexB
        {
            get { return _vertexB; }
        }

        private PointF _vertexC;
        /// <summary>
        /// Gets point of vertex C
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public PointF VertexC
        {
            get { return _vertexC; }
        }

        private float _height;
        /// <summary>
        /// Gets height of triangle
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public float Height
        {
            get { return _height; }
        }

        private float _sideLength;
        /// <summary>
        /// Gets length of triangle side
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public float SideLength
        {
            get { return _sideLength; }
            //set { _tSideLength = value; }
        }

        private Color _backColor = Color.White;
        [Category("Appearance")]
        [Description("Background color")]
        [DefaultValue(typeof(Color), "White")]
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;
                OnChanged(this, new PropertyChangedEventArgs("BackColor"));
            }
        }

        private Color _borderColor = Color.Black;
        [Category("Appearance")]
        [Description("Color of the triangle border")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                OnChanged(this, new PropertyChangedEventArgs("BorderColor"));
            }
        }

        private int _borderWidth = 2;
        [Category("Appearance")]
        [Description("Width of the triangle border")]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set
            {
                _borderWidth = value;
                OnChanged(this, new PropertyChangedEventArgs("BorderWidth"));
            }
        }
        #endregion //Properties

        #region Methods
        /// <summary>
        /// Calculates parameters of the diagram triangle
        /// </summary>
        /// <param name="bounds">Diagram bounds</param>
        /// <param name="topMargin">Miminum margin from top where you can draw triangle with labels and arrows</param>
        /// <param name="bottomMargin">Miminum margin from bottom where you can draw triangle with labels and arrows<</param>
        /// <param name="leftMargin">Miminum margin from left where you can draw triangle with labels and arrows<</param>
        /// <param name="rightMargin">Miminum margin from right where you can draw triangle with labels and arrows<</param>
        public void Calculate(Rectangle bounds, int topMargin, int bottomMargin, int leftMargin, int rightMargin)
        {
            float centerX = bounds.Left + bounds.Width / 2;
            float centerY =  bounds.Top + bounds.Height / 2;

            // Length of equilateral triangle side
            int left_to_right = bounds.Width - leftMargin - rightMargin;
            int top_to_bottom = bounds.Height - topMargin - bottomMargin;

            // Check if lenght of triangle side will be determined by width or height of the control
            if (left_to_right > top_to_bottom) _sideLength = top_to_bottom;
            else _sideLength = left_to_right;

            // Height of triangle
            _height = (float)(Math.Sqrt(3) * _sideLength / 2);

            float centering_margin = (bounds.Height - _height - topMargin - bottomMargin) / 2;

            // Vertexes
            //   A
            // B   C
            float vaX = bounds.Left + leftMargin + (bounds.Width - leftMargin - rightMargin) / 2;
            float vaY = bounds.Top + topMargin + centering_margin;

            float vbX = vaX - _sideLength / 2;
            float vbY = vaY + _height;

            float vcX = vaX + _sideLength / 2;
            float vcY = vaY + _height;

            _vertexA = new PointF(vaX, vaY);
            _vertexB = new PointF(vbX, vbY);
            _vertexC = new PointF(vcX, vcY);

            PointF[] triangle_vertexes = new PointF[] { _vertexA, _vertexB, _vertexC };
            byte[] pthPointType = new byte[] { (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line };

            _path = new GraphicsPath(triangle_vertexes, pthPointType);
            _path.CloseAllFigures();
        }
        #endregion // Methods 

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
