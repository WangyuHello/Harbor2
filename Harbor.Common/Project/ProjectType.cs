using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Harbor.Common.Project
{
    public enum ProjectType
    {
        Analog,
        Digital,
        Memory,
        Ip
    }

    public sealed class ProjectTypeConverter : TypeConverter
    {
        public static readonly Dictionary<string, ProjectType> Lookup = new Dictionary<string, ProjectType>(StringComparer.OrdinalIgnoreCase)
        {
            { "a", ProjectType.Analog },
            { "analog", ProjectType.Analog },
            { "d", ProjectType.Digital },
            { "digital", ProjectType.Digital },
            { "m", ProjectType.Memory },
            { "memory", ProjectType.Memory },
            { "ip", ProjectType.Ip },
        };

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                var result = Lookup.TryGetValue(stringValue, out var type);
                if (!result)
                {
                    return null;
                }
                return type;
            }
            throw new NotSupportedException("无法转换项目类型");
        }
    }
}
