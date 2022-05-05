using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TernaryDiagramLib
{
    /// <summary>
    /// Class representing ternary diagram value gradient 
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ValueGradient : DiagramElement
    {
        internal void Initialize()
        {
            _enabled = true;
            _size = new System.Drawing.Size(20, 150);
            _location = new Point();
            _font = new System.Drawing.Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
            _foreColor = Color.Black;
            _ignoredColors = new List<int>();
            _titleColor = Color.Black;
            _title = "";
            _titleFont = new System.Drawing.Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);
            _minValue = double.NaN;
            _maxValue = double.NaN;
        }

        /// <summary>
        /// Creates default color gradient with JetFlame color map
        /// </summary>
        public ValueGradient()
        {
            _colorMap = ColorMaps.JetFlame;
            Initialize();
        }

        /// <summary>
        /// Creates gradient from given color map
        /// </summary>
        /// <param name="colorMap">Color map with gradient ColorPoints</param>
        public ValueGradient(ColorMap colorMap)
        {
            _colorMap = colorMap;
            Initialize();
        }

        #region Properties
        //List of color points in ascending value.
        private ColorMap _colorMap;

        private bool _enabled;
        /// <summary>
        /// Enables or disables value gradient
        /// </summary>
        [Category("Behavior")]
        [Description("Enables or disables value gradient")]
        [DefaultValue(true)]
        [NotifyParentProperty(true)]
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                OnChanged(this, new PropertyChangedEventArgs("Enabled"));
            }
        }

        private double _minValue;
        /// <summary>
        /// Minimum value to display
        /// </summary>
        [Category("Data")]
        [Description("Minimum value to display")]
        [RefreshProperties(RefreshProperties.All)]
        public double Minimum
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        private double _maxValue;
        /// <summary>
        /// Maximum value to display
        /// </summary>
        [Category("Data")]
        [Description("Maximum value to display")]
        [RefreshProperties(RefreshProperties.All)]
        public double Maximum
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        private Size _size;
        /// <summary>
        /// Gets or sets size of the value gradient
        /// </summary>
        [Category("Layout")]
        [Description("Size of the value gradient")]
        [DefaultValue(typeof(Size), "20, 150")]
        [NotifyParentProperty(true)]
        public Size Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnChanged(this, new PropertyChangedEventArgs("Size"));
            }
        }

        private Point _location;
        [Category("Layout")]
        [Description("Gets or sets location of the value gradient")]
        [NotifyParentProperty(true)]
        [ReadOnly(true)]
        public Point Location
        {
            get { return _location; }
            set
            {
                _location = value;
                //OnChanged(this, new PropertyChangedEventArgs("Location"));
            }
        }

        private Font _font;
        [Category("Appearance")]
        [Description("Gets or sets font of the gradient labels")]
        [DefaultValue(typeof(Font), "Arial, 10pt")]
        [NotifyParentProperty(true)]
        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                OnChanged(this, new PropertyChangedEventArgs("Font"));
            }
        }

        private Color _foreColor;
        [Category("Appearance")]
        [Description("Gets or sets color of the gradient labels")]
        [DefaultValue(typeof(Color), "Black")]
        [NotifyParentProperty(true)]
        public Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                _foreColor = value;
                OnChanged(this, new PropertyChangedEventArgs("ForeColor"));
            }
        }

        private string _title;
        [Category("Appearance")]
        [Description("Gets or sets title of gradient")]
        [DefaultValue("Gradient")]
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
        [Description("Gets or sets font of the gradient title")]
        [DefaultValue(typeof(Font), "Arial, 10pt")]
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
        [Description("Gets or sets color of gradient title")]
        [DefaultValue(typeof(Color), "0x000000")]
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

        private List<int> _ignoredColors;
        /// <summary>
        /// Colors excluded from method GetValueByColor
        /// Notice: not good enough since control is using anti-aliasing and there will be a lot of different shades of base colors,
        /// because edges are "blurred" therefore each point is not actually represented by one solid color 
        /// </summary>
        public List<int> IgnoredColors
        {
            get { return _ignoredColors; }
            set
            {
                _ignoredColors = value;
                OnChanged(this, new PropertyChangedEventArgs("ExclusiveColors"));
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Gets the corresponding color from the gradient for a specific diagram value
        /// </summary>
        /// <param name="value">Diagram value from ternary point</param>
        /// <returns>Color for a specific diagram value</returns>
        public Color GetColorForDiagramValue(double value)
        {
            int red = 0;
            int green = 0;
            int blue = 0;

            // Change it to value between 0 and 1
            double percV = (value - Minimum) / (Maximum - Minimum);

            if (_colorMap.ColorPoints.Count > 0)
            {
                red = _colorMap.ColorPoints.Last().R;
                green = _colorMap.ColorPoints.Last().G;
                blue = _colorMap.ColorPoints.Last().B;

                for (int i = 0; i < _colorMap.ColorPoints.Count; i++)
                {
                    ColorPoint currColor = _colorMap.ColorPoints[i];
                    if (percV < currColor.Value)
                    {
                        ColorPoint prevColor = _colorMap.ColorPoints[Math.Max(0, i - 1)];
                        double valueDiff = (prevColor.Value - currColor.Value);
                        double fractBetween = (valueDiff == 0) ? 0 : (percV - currColor.Value) / valueDiff;
                        red = (int)(Math.Round((prevColor.R - currColor.R) * fractBetween) + currColor.R);
                        green = (int)(Math.Round((prevColor.G - currColor.G) * fractBetween) + currColor.G);
                        blue = (int)(Math.Round((prevColor.B - currColor.B) * fractBetween) + currColor.B);
                        break;
                    }
                }
            }

            return Color.FromArgb(255, red, green, blue);
        }

        /// <summary>
        /// Gets color for gradient value. Gradient values are between 0 and 1.
        /// </summary>
        /// <param name="value">Gradient value between 0 and 1, representing position in the gradient</param>
        /// <returns>Color for specified gradient value</returns>
        private Color GetColorAtGradientValue(float value)
        {
            int red = 0;
            int green = 0;
            int blue = 0;

            if (_colorMap.ColorPoints.Count > 0)
            {
                red = _colorMap.ColorPoints.Last().R;
                green = _colorMap.ColorPoints.Last().G;
                blue = _colorMap.ColorPoints.Last().B;

                for (int i = 0; i < _colorMap.ColorPoints.Count; i++)
                {
                    ColorPoint currColor = _colorMap.ColorPoints[i];
                    if (value < currColor.Value)
                    {
                        ColorPoint prevColor = _colorMap.ColorPoints[Math.Max(0, i - 1)];
                        double valueDiff = (prevColor.Value - currColor.Value);
                        double fractBetween = (valueDiff == 0) ? 0 : (value - currColor.Value) / valueDiff;
                        red = (int)(Math.Round((prevColor.R - currColor.R) * fractBetween) + currColor.R);
                        green = (int)(Math.Round((prevColor.G - currColor.G) * fractBetween) + currColor.G);
                        blue = (int)(Math.Round((prevColor.B - currColor.B) * fractBetween) + currColor.B);
                        break;
                    }
                }
            }

            return Color.FromArgb(255, red, green, blue);
        }

        /// <summary>
        /// Calculates gradient value from color
        /// (it's not precise)
        /// </summary>
        /// <param name="color">Color to find value for</param>
        /// <returns>Gradient value (0-1)</returns>
        public double GetValueByColor(Color color)
        {
            if (!_ignoredColors.Contains(color.ToArgb()))
            {
                if (_colorMap.Colors.Contains(color))
                {
                    ColorPoint cp0 = _colorMap.ColorPoints.First(n => n.Color == color);
                    return cp0.Value;
                }
                else
                {
                    // Closest color
                    ColorPoint cp1 = GetNearestColorPoint(color);
                    // Second closest color
                    ColorPoint cp2 = GetNearestColorPoint(color, cp1.Color);

                    double fract = double.NaN;
                    double value = double.NaN;

                    double fractB = double.NaN;
                    double fractG = double.NaN;
                    double fractR = double.NaN;

                    if (cp1.Value < cp2.Value)
                    {
                        if (cp1.B != cp2.B)
                            fract = (color.B - cp1.B) / (double)(cp2.B - cp1.B);
                        else if (cp1.G != cp2.G)
                            fract = (color.G - cp1.G) / (double)(cp2.G - cp1.G);
                        else if (cp1.R != cp2.R)
                            fract = (color.R - cp1.R) / (double)(cp2.R - cp1.R);

                        if (cp1.B != cp2.B)
                            fractB = (color.B - cp1.B) / (double)(cp2.B - cp1.B);
                        if (cp1.G != cp2.G)
                            fractG = (color.G - cp1.G) / (double)(cp2.G - cp1.G);
                        if (cp1.R != cp2.R)
                            fractR = (color.R - cp1.R) / (double)(cp2.R - cp1.R);

                        value = cp1.Value + fract * (cp2.Value - cp1.Value);
                        double value2 = (fract / (cp2.Value - cp1.Value)) + cp1.Value;
                        double result = 900 * value;

                    }
                    else
                    {
                        if (cp1.B != cp2.B)
                            fract = (color.B - cp1.B) / (double)(cp2.B - cp1.B);
                        else if (cp1.G != cp2.G)
                            fract = (color.G - cp1.G) / (double)(cp2.G - cp1.G);
                        else if (cp1.R != cp2.R)
                            fract = (color.R - cp2.R) / (double)(cp1.R - cp2.R);

                        if (cp1.B != cp2.B)
                            fractB = (color.B - cp1.B) / (double)(cp2.B - cp1.B);
                        if (cp1.G != cp2.G)
                            fractG = (color.G - cp1.G) / (double)(cp2.G - cp1.G);
                        if (cp1.R != cp2.R)
                            fractR = (color.R - cp2.R) / (double)(cp1.R - cp2.R);

                        value = cp2.Value + fract * (cp1.Value - cp2.Value);
                        double value2 = (fract / (cp1.Value - cp2.Value)) + cp2.Value;
                        double result = 1000 + 900 * value;

                    }

                    return value;
                }
            }
            else return double.NaN;
        }

        /// <summary>
        /// Searches for closest color from color map
        /// </summary>
        /// <param name="input_color">Given color to search for closest match</param>
        /// <returns>Matching gradient ColorPoint</returns>
        private ColorPoint GetNearestColorPoint(Color input_color)
        {
            double dbl_input_red = Convert.ToDouble(input_color.R);
            double dbl_input_green = Convert.ToDouble(input_color.G);
            double dbl_input_blue = Convert.ToDouble(input_color.B);

            double distance = 500.0;

            ColorPoint nearest_color = new ColorPoint();

            foreach (ColorPoint colorPoint in _colorMap.ColorPoints)
            {
                Color o = colorPoint.Color;
                // Compute the Euclidean distance between the two colors
                // Note, that the alpha-component is not used in this example
                double dbl_test_red = Math.Pow(Convert.ToDouble(o.R) - dbl_input_red, 2.0);
                double dbl_test_green = Math.Pow(Convert.ToDouble(o.G) - dbl_input_green, 2.0);
                double dbl_test_blue = Math.Pow(Convert.ToDouble(o.B) - dbl_input_blue, 2.0);

                double temp = Math.Sqrt(dbl_test_blue + dbl_test_green + dbl_test_red);
                // Explore the result and store the nearest color
                if (temp == 0.0)
                {
                    nearest_color = colorPoint;
                    break;
                }
                else if (temp < distance)
                {
                    distance = temp;
                    nearest_color = colorPoint;
                }
            }

            return nearest_color;
        }

        /// <summary>
        /// Searches for closest color from color map
        /// </summary>
        /// <param name="input_color">Given color to search for closest match</param>
        /// <param name="exception">Color excluded from search results</param>
        /// <returns>Matching gradient ColorPoint</returns>
        private ColorPoint GetNearestColorPoint(Color input_color, Color exception)
        {
            double dbl_input_red = Convert.ToDouble(input_color.R);
            double dbl_input_green = Convert.ToDouble(input_color.G);
            double dbl_input_blue = Convert.ToDouble(input_color.B);

            double distance = 500.0;

            ColorPoint nearest_color = new ColorPoint();

            foreach (ColorPoint colorPoint in _colorMap.ColorPoints)
            {
                Color o = colorPoint.Color;
                if (o != exception)
                {
                    // Compute the Euclidean distance between the two colors
                    // Note, that the alpha-component is not used
                    double dbl_test_red = Math.Pow(Convert.ToDouble(o.R) - dbl_input_red, 2.0);
                    double dbl_test_green = Math.Pow(Convert.ToDouble(o.G) - dbl_input_green, 2.0);
                    double dbl_test_blue = Math.Pow(Convert.ToDouble(o.B) - dbl_input_blue, 2.0);

                    double temp = Math.Sqrt(dbl_test_blue + dbl_test_green + dbl_test_red);
                    // Explore the result and store the nearest color
                    if (temp == 0.0)
                    {
                        nearest_color = colorPoint;
                        break;
                    }
                    else if (temp < distance)
                    {
                        distance = temp;
                        nearest_color = colorPoint;
                    }
                }
            }

            return nearest_color;
        }

        /// <summary>
        /// Creates image of gradient with specified width and height
        /// </summary>
        /// <param name="width">Width of the image</param>
        /// <param name="height">Height of the image</param>
        /// <returns>Image of gradient</returns>
        public Image GetGradientImage(int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            float h = (float)bmp.Height;

            for (int y = 0; y < bmp.Height; y++)
            {
                Color color = GetColorAtGradientValue(1 - (float)y / h);
                for (int x = 0; x < bmp.Width; x++)
                {
                    bmp.SetPixel(x, y, color);
                }
            }

            return (Image)bmp;
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

    /// <summary>
    /// Structure used to store colors at different points in the gradient.
    /// </summary>
    [Serializable]
    public struct ColorPoint
    {
        /// <summary>
        /// Creates color point from given RGB values and gradient value
        /// </summary>
        /// <param name="red">Red component value of color structure</param>
        /// <param name="green">Green component value of color structure</param>
        /// <param name="blue">Blue component value of color structure</param>
        /// <param name="value">Position of color along the gradient (between 0 and 1)</param>
        public ColorPoint(int red, int green, int blue, double value)
        {
            R = red;
            G = green;
            B = blue;
            if (value < 0) Value = 0;
            else if (value > 1) Value = 1;
            else Value = value;
            Color = Color.FromArgb(255, red, green, blue);
        }

        /// <summary>
        /// Creates color point from given color and gradient value
        /// </summary>
        /// <param name="color">Color</param>
        /// <param name="value">Position of color along the gradient (between 0 and 1)</param>
        public ColorPoint(Color color, double value)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            if (value < 0) Value = 0;
            else if (value > 1) Value = 1;
            else Value = value;
            Color = color;
        }

        // Red, green and blue component values of color.
        public int R, G, B;
        // Position of our color along the gradient (between 0 and 1).
        public double Value;
        // Color component of the ColorPoint
        public Color Color;

        public override string ToString()
        {
            return String.Format("R:{0}, G:{1}, B:{2}, Value:{3}", R, G, B, Value);
        }
    }

    /// <summary>
    /// Map of gradient colors
    /// </summary>
    [Serializable]
    public class ColorMap
    {
        public ColorMap(List<Color> colorList)
        {
            Colors = colorList;
        }

        private List<Color> _colors = new List<Color>();
        /// <summary>
        /// Base colors
        /// </summary>
        public List<Color> Colors
        {
            get { return _colors; }
            set
            {
                _colorPoints = new List<ColorPoint>();
                _colors = new List<Color>();
                foreach (Color color in value)
                {
                    _colorPoints.Add(new ColorPoint(color, value.IndexOf(color) / (double)(value.Count - 1)));
                    _colors.Add(color);
                }
            }
        }

        private List<ColorPoint> _colorPoints = new List<ColorPoint>();
        /// <summary>
        /// ColorPoints for ColorGradient - each color has a value which indicates where in gradient it should be placed
        /// </summary>
        public List<ColorPoint> ColorPoints
        {
            get { return _colorPoints; }
        }

        /// <summary>
        /// Clear all colors and points
        /// </summary>
        public void ClearMap()
        {
            _colorPoints.Clear();
            _colors.Clear();
        }
    }

    /// <summary>
    /// Color schemes for ternary value gradient
    /// </summary>
    public static class ColorMaps
    {
        /// <summary>
        /// Pretty much like some gas flame - blue, yellow, a little bit of green and red
        /// </summary>
        public static readonly ColorMap JetFlame = new ColorMap(new List<Color>()
        {
            Color.FromArgb(0, 0, 0x8F), Color.FromArgb(0,0, 0xFF), Color.FromArgb(0,0x6F,0xFF),
            Color.FromArgb(0, 0xDF, 0xFF), Color.FromArgb(0x4F, 0xFF, 0xAF), Color.FromArgb(0xBF, 0xFF, 0x3F),
            Color.FromArgb(0xFF, 0xCF, 0), Color.FromArgb(0xFF, 0x5F, 0), Color.FromArgb(0xEF, 0, 0),
            Color.FromArgb(0x7F, 0, 0)
        });

        /// <summary>
        /// Similar to JetFlame, but more green
        /// </summary>
        public static readonly ColorMap Rainbow = new ColorMap(new List<Color>()
        {
            Color.FromArgb(0, 0, 255),      // Blue
            Color.FromArgb(0, 255, 255),    // Cyan
            Color.FromArgb(0, 255, 0),      // Green
            Color.FromArgb(255, 255, 0),    // Yellow
            Color.FromArgb(255, 0, 0)       // Red
        });
    }
}
