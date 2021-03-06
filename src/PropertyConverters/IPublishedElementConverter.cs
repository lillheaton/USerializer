﻿using JsonContractSimplifier.Services.ConverterLocator;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;

namespace USerializer.PropertyConverters
{
    public class IPublishedElementConverter : IObjectConverter<IPublishedElement>
    {
        private readonly IPublishedPropertyConverter _publishedPropertyConverter;

        public IPublishedElementConverter()
        {
            _publishedPropertyConverter = new IPublishedPropertyConverter();
        }

        public object Convert(IPublishedElement target)
        {
            var dictionary = target?.Properties.ToDictionary(x => x.Alias, x => _publishedPropertyConverter.Convert(x) ?? null);
            dictionary.Add("ContentTypeAlias", target.ContentType.Alias);
            return dictionary;
        }
    }
}
