using System;
using System.Collections.Generic;
using System.Text;

namespace ErpCore.EntityFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DecimalPrecisionAttr : Attribute
    {
        public DecimalPrecisionAttr(byte precision, byte scale)
        {
            Precision = precision;
            Scale = scale;

        }

        public byte Precision { get; set; }
        public byte Scale { get; set; }

    }
}
