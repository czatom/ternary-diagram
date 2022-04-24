using System;
using System.ComponentModel;
using System.Globalization;

namespace TernaryDiagramLib
{
    class TrackingBoxConverter : ExpandableObjectConverter
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
                var status = (value as TrackingBox).Enabled ? "Enabled" : "Disabled";
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
