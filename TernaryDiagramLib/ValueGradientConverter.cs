using System;
using System.ComponentModel;
using System.Globalization;

namespace TernaryDiagramLib
{
    public class ValueGradientConverter : ExpandableObjectConverter
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
                var status = (value as ValueGradient).Enabled ? "Enabled" : "Disabled";
                return status;
            }

            return base.ConvertTo(
                context,
                culture,
                value,
                destinationType);
        }
    }
}
