using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;

namespace TernaryDiagramLib
{
    internal class AxesArrayConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return new CollectionConverter().ConvertToString(new ArrayList());
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
