using System;
using System.Drawing;

namespace TernaryDiagramLib
{
    public class PointT
    {
        /// <summary>
        /// Creates ternary diagram point
        /// </summary>
        /// <param name="valA">Value of A coordinate</param>
        /// <param name="valB">Value of B coordinate</param>
        /// <param name="valC">Value of C coordinate</param>
        /// <param name="valD">Value of D</param>
        /// <param name="diagram">Ternary diagram reference</param>
        public PointT(float valA, float valB, float valC, float valD, TernaryDiagram diagram)
        {
            this._valueA = valA;
            this._valueB = valB;
            this._valueC = valC;
            this._valueD = valD;
            _diagram = diagram;
        }

        #region Variables
        private TernaryDiagram _diagram;
        private float _valueA;
        private float _valueB;
        private float _valueC;
        private float _valueD;
        #endregion //Variables

        #region Properties
        /// <summary>
        /// Gets or sets value of A coordinate
        /// </summary>
        public float ValueA
        {
            get { return _valueA; }
            set { _valueA = value; }
        }

        /// <summary>
        /// Gets or sets value of B coodinate
        /// </summary>
        public float ValueB
        {
            get { return _valueB; }
            set { _valueB = value; }
        }

        /// <summary>
        /// Gets or sets value of C coordinate
        /// </summary>
        public float ValueC
        {
            get { return _valueC; }
            set { _valueC = value; }
        }

        /// <summary>
        /// Gets or sets value of D (marked with color)
        /// </summary>
        public float ValueD
        {
            get { return _valueD; }
            set { _valueD = value; }
        }

        /// <summary>
        /// Gets ABC point in control coordinate system
        /// </summary>
        public PointF PointABC
        {
            get
            {
                return realCoords();
            }
        }

        /// <summary>
        /// Gets rectangle containing ABC point marker
        /// </summary>
        public RectangleF MarkerRect
        {
            get
            {
                DiagramArea diagram = _diagram.DiagramAreas[0];
                PointF abc = realCoords();
                RectangleF rect = new RectangleF();
                rect.X = abc.X - diagram.MarkerSize / 2;
                rect.Y = abc.Y - diagram.MarkerSize / 2;
                rect.Size = new SizeF(diagram.MarkerSize, diagram.MarkerSize);
                return rect;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Calculates real diagram coordinates of current ternary point
        /// </summary>
        /// <returns>XY point</returns>
        private PointF realCoords()
        {
            Triangle triangle = this._diagram.DiagramAreas[0].DiagramTriangle;
            // B value on AB axis
            float abX = triangle.VertexA.X - (_valueB * (triangle.SideLength / (2 * 100)));
            float abY = triangle.VertexA.Y + (_valueB * (triangle.Height / 100));
            PointF axisBval = new PointF(abX, abY);

            // C value on BC axis
            float bcX = triangle.VertexB.X + (_valueC * (triangle.SideLength / 100));
            float bcY = triangle.VertexB.Y;
            PointF axisCval = new PointF(bcX, bcY);

            // A value on CA axis
            float caX = triangle.VertexC.X - (_valueA * (triangle.SideLength / (2 * 100)));
            float caY = triangle.VertexC.Y - (_valueA * (triangle.Height / 100));
            PointF axisAval = new PointF(caX, caY);

            // y=ax+b
            // Coef "a" of line creating axis AB
            float coefAab = (triangle.VertexA.Y - triangle.VertexB.Y) / (triangle.VertexA.X - triangle.VertexB.X);

            // Coef "b" of line going through the diagram point given by percA, perB and percC and crossing axis BC
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
                return _valueA.Equals(pointObj._valueA) && _valueB.Equals(pointObj._valueB) && _valueC.Equals(pointObj._valueC) && pointObj._diagram.Equals(_diagram);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
        #endregion // Overrides

    }
}
