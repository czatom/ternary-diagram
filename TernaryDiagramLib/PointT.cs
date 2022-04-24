using System;
using System.Drawing;

namespace TernaryDiagramLib
{
    public class PointT
    {
        /// <summary>
        /// Creates ternary diagram point
        /// </summary>
        /// <param name="a">A coordinate</param>
        /// <param name="b">B coordinate</param>
        /// <param name="c">C coordinate</param>
        /// <param name="value">Value</param>
        /// <param name="diagram">Ternary diagram reference</param>
        public PointT(float a, float b, float c, double value, DiagramArea diagram)
        {
            this.A = a;
            this.B = b;
            this.C = c;
            this.Value = value;
            _diagram = diagram;
        }

        #region Variables
        private readonly DiagramArea _diagram;
        #endregion // Variables

        #region Properties
        /// <summary>
        /// Gets or sets value of A coordinate
        /// </summary>
        public float A { get; set; }

        /// <summary>
        /// Gets or sets value of B coordinate
        /// </summary>
        public float B { get; set; }

        /// <summary>
        /// Gets or sets value of C coordinate
        /// </summary>
        public float C { get; set; }

        /// <summary>
        /// Gets or sets value of D (marked with color)
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets ABC point in control coordinate system
        /// </summary>
        public PointF PointABC
        {
            get
            {
                return RealCoords();
            }
        }

        /// <summary>
        /// Gets rectangle containing ABC point marker
        /// </summary>
        public RectangleF MarkerRect
        {
            get
            {
                PointF abc = RealCoords();
                RectangleF rect = new RectangleF();
                rect.X = abc.X - _diagram.MarkerSize / 2;
                rect.Y = abc.Y - _diagram.MarkerSize / 2;
                rect.Size = new SizeF(_diagram.MarkerSize, _diagram.MarkerSize);
                return rect;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates real diagram coordinates of current ternary point
        /// </summary>
        /// <returns>XY point</returns>
        private PointF RealCoords()
        {
            Triangle triangle = _diagram.DiagramTriangle;

            // B value on AB axis
            float bPctRange = _diagram.AxisB.Maximum - _diagram.AxisB.Minimum;
            float abX = triangle.VertexA.X - (B - _diagram.AxisB.Minimum) * (triangle.SideLength / (2 * bPctRange));
            float abY = triangle.VertexA.Y + (B - _diagram.AxisB.Minimum) * (triangle.Height / bPctRange);
            PointF axisBval = new PointF(abX, abY);

            // C value on BC axis
            float cPctRange = _diagram.AxisC.Maximum - _diagram.AxisC.Minimum;
            float onePctInPixels = triangle.SideLength / cPctRange;
            float bcX = triangle.VertexB.X + (C - _diagram.AxisC.Minimum) * onePctInPixels;
            float bcY = triangle.VertexB.Y;
            PointF axisCval = new PointF(bcX, bcY);

            // A value on CA axis
            float aPctRange = _diagram.AxisA.Maximum - _diagram.AxisA.Minimum;
            float caX = triangle.VertexC.X - (A - _diagram.AxisA.Minimum) * (triangle.SideLength / (2 * aPctRange));
            float caY = triangle.VertexC.Y - (A - _diagram.AxisA.Minimum) * (triangle.Height / aPctRange);
            PointF axisAval = new PointF(caX, caY);

            // y=ax+b
            // Coefficient "a" of line creating axis AB
            float coefAab = (triangle.VertexA.Y - triangle.VertexB.Y) / (triangle.VertexA.X - triangle.VertexB.X);

            // Coefficient "b" of line going through the diagram point given by percA, perB and percC and crossing axis BC
            float coefBbcv = bcY - coefAab * bcX;

            // Point ABC
            float py = caY;
            float px = (py - coefBbcv) / coefAab;
            return new PointF(px, py);
        }
        #endregion // Methods

        #region Overrides
        public override bool Equals(Object obj)
        {
            PointT pointObj = obj as PointT;
            if (pointObj == null)
                return false;
            else
                return A.Equals(pointObj.A) && B.Equals(pointObj.B) && C.Equals(pointObj.C) && pointObj._diagram == _diagram; //TODO do we need to compare values?
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
        #endregion // Overrides

    }
}
