using System;
using System.Collections.Generic;

namespace ProjetoArtCouro.Web.Extensions.HtmlHelperExtensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class HtmlPropertiesAttribute : Attribute
    {
        public string CssClass
        {
            get;
            set;
        }
        public int MaxLength
        {
            get;
            set;
        }
        public int Size
        {
            get;
            set;
        }
        public IDictionary<string, object> HtmlAttributes()
        {
            IDictionary<string, object> htmlatts = new Dictionary<string, object>();
            if (MaxLength != 0)
            {
                htmlatts.Add("MaxLength", MaxLength);
            }
            if (Size != 0)
            {
                htmlatts.Add("Size", Size);
            }
            return htmlatts;
        }
    }
}