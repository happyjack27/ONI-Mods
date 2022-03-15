using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerParts
{
    class BinaryFormatter : IFormatProvider, ICustomFormatter
    {
        public BinaryFormatter()
        {
        }
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!(arg is int))
            {
                if (!string.IsNullOrEmpty(format))
                {
                    return string.Format(format, arg);
                }
                else
                {
                    return arg.ToString();
                }
            }
            // Check whether this is an appropriate callback
            if (!this.Equals(formatProvider))
                return null;
            var components = (format ?? "D").Split(':');
            var primary = components[0];
            var bits = 4;
            if (components.Length > 1)
            {
                bits = Convert.ToInt32(components[1]);
            }

            string numericString = arg.ToString();
            int value = (int)arg;
            var normalValue = value;
            normalValue = BinaryUtils.GetValue(value, bits);
            
            if (primary == "H")
            {
                return "0x" + Convert.ToString(value, 16);
            }
            if (primary == "D")
            {
                return normalValue.ToString();
            }
            else if (primary == "B")
            {
                var binaryValue = "";
                for (int i = 1; i <= bits; i++)
                {
                    var bit = (1 << (bits - i));
                    var hasBit = (value & bit) == bit;
                    if (hasBit)
                    {
                        binaryValue += "1";
                    }
                    else
                    {
                        binaryValue += "0";
                    }

                }
                return binaryValue;
            }
            else if (primary == "F")
            {
                return $"{this.Format("D", arg, formatProvider)} ({this.Format($"B:{bits}", arg, formatProvider)})";
            }
            return numericString;
        }
    }
}
