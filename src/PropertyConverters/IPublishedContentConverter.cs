using JsonContractSimplifier.Services.ConverterLocator;
using System.Collections.Generic;
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

        private Dictionary<string, object> ParseDocumentType(IPublishedContent target)
        {
            var dictionary = target.Properties?.ToDictionary(x => x.Alias, x => _publishedPropertyConverter.Convert(x) ?? null);
            dictionary.Add("Level", target.Level);
            dictionary.Add("ContentTypeAlias", target.ContentType.Alias);
            dictionary.Add("Url", target.Url);
            dictionary.Add("Name", target.Name);
            dictionary.Add("Id", target.Id);

            return dictionary;
        }

        public object Convert(IPublishedContent target)
        {
            switch (target.ItemType)
            {
                case PublishedItemType.Media:
                    return target.Url;

                default:
                    return ParseDocumentType(target);
            }
        }
    }
}
