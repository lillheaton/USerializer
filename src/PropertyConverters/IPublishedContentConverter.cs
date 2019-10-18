using JsonContractSimplifier.Services.ConverterLocator;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;

namespace USerializer.PropertyConverters
{
    public class IPublishedContentConverter : IObjectConverter<IPublishedContent>
    {
        private readonly IPublishedPropertyConverter _publishedPropertyConverter;

        public IPublishedContentConverter()
        {
            _publishedPropertyConverter = new IPublishedPropertyConverter();
        }

        public object Convert(IPublishedContent target)
        {
            return new
            {
                target.Id,
                target.Name,
                target.Url,
                target.Level,
                ContentTypeAlias = target.ContentType.Alias,
                Properties = target.Properties?.ToDictionary(x => x.Alias, x => _publishedPropertyConverter.Convert(x) ?? null)
            };
        }
    }
}
