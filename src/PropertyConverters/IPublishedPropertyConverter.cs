using JsonContractSimplifier.Services.ConverterLocator;
using Umbraco.Core.Models.PublishedContent;

namespace USerializer.PropertyConverters
{
    public class IPublishedPropertyConverter : IObjectConverter<IPublishedProperty>
    {
        public object Convert(IPublishedProperty target)
        {
            if (target == null)
                return null;

            return target.GetValue();
        }
    }
}
