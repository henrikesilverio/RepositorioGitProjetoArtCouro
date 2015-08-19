using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjetoArtCouro.Web.Infra.WebValidator
{
    public class HtmlPropertiesAttribute : Attribute
    {
        private readonly IDictionary<string, object> _htmlAtrributes;

        public HtmlPropertiesAttribute(params string[] arr)
            : this(arr.ToDictionary(x => x.Split(':')[0].ToLower(), x => (object)x.Split(':')[1]))
        {
        }

        public HtmlPropertiesAttribute(IDictionary<string, object> htmlAtrributes)
        {
            _htmlAtrributes = htmlAtrributes;
        }

        public IDictionary<string, object> GetHtmlAttributes()
        {
            return _htmlAtrributes;
        }
    }
}