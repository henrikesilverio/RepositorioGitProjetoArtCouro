using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;

namespace ProjetoArtCouro.Web.Extensions.HtmlHelperExtensions
{
    public class MetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected ModelMetadata CreateMetadataPrototype(IEnumerable<Attribute> attributes,
                                                        Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            var additionalValues = attributes.OfType<HtmlPropertiesAttribute>().FirstOrDefault();
            if (additionalValues != null)
            {
                metadata.AdditionalValues.Add("HtmlAttributes", additionalValues);
            }
            return metadata;
        }
    }
}