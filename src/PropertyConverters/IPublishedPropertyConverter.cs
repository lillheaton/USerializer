using JsonContractSimplifier.Services.ConverterLocator;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace USerializer.PropertyConverters
{
    public class IPublishedPropertyConverter : IObjectConverter<IPublishedProperty>
    {
        public object Convert(IPublishedProperty target)
        {
            if (target == null)
                return null;

            var value = target.GetValue();

            switch (target.PropertyType.EditorAlias)
            {
                case "Umbraco.MediaPicker":
                    if (value is IPublishedContent)
                    {
                        return (value as IPublishedContent).Url;
                    }
                    else if (value is IEnumerable<IPublishedContent>)
                    {
                        return ((IEnumerable<IPublishedContent>)value).Select(s => s.Url).ToArray();
                    }
                    return null;
                    
                default:
                    return value;
            }
        }
    }
}