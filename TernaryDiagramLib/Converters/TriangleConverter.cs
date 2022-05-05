using System;
using System.ComponentModel;
using System.Globalization;

namespace TernaryDiagramLib
{
    public class TriangleConverter : ExpandableObjectConverter
    {
        // This override prevents the PropertyGrid from  
        // displaying the full type name in the value cell. 
        public override object ConvertTo(
            ITypeDescriptorContext context,
            CultureInfo culture,
            object value,
            Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var triangle = value as Triangle;
                var description = $"A: {triangle.VertexA}, B: {triangle.VertexB}, C: {triangle.VertexC}";
                return description;
            }

            return base.ConvertTo(
                context,
                culture,
                value,
                destinationType);
        }
    }
}
