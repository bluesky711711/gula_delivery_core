using System;
using System.Collections.Generic;
using System.Text;

namespace ErpCore.EntityFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class InfoGroupAttr : Attribute
    {
        public string[] InfoGroups { get; private set; }

        public InfoGroupAttr(string _infoGroupStr)
        {
            InfoGroups = _infoGroupStr.Split(',');
        }
    }
}
