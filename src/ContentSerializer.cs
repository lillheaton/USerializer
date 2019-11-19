using JsonContractSimplifier.Services.ConverterLocator;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using USerializer.Utils;

namespace USerializer
{
    public class ContentSerializer : IContentSerializer
    {
        private readonly IConverterLocatorService _converterLocatorService;

        public JsonSerializerSettings JsonSerializerSettings { get; private set; }

        public ContentSerializer(IConverterLocatorService converterLocatorService)
        {
            _converterLocatorService = converterLocatorService;

            JsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new JsonContractSimplifier.ContractResolver(converterLocatorService, null)
                {
                    ShouldCache = false,
                },
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
        
        public string Serialize(object target)
        {
            return JsonConvert.SerializeObject(target, JsonSerializerSettings);
        }

        public Dictionary<string, object> Convert(object target)
        {
            Type targetType = target.GetType();

            // Could not find any converter
            if(!_converterLocatorService.TryFindConverterFor(targetType, out IConverter converter))
            {
                var properties = targetType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.HasAttribute<JsonIgnoreAttribute>() == false);

                return ObjectUtils.ConvertProperties(target, properties);
            }

            var convertedTarget = _converterLocatorService.Convert(target, converter);

            // Converted target is already dictionary
            if(convertedTarget is IDictionary<string, object>)
            {
                return new Dictionary<string, object>(convertedTarget as IDictionary<string, object>);
            }
            
            var convertedTargetType = convertedTarget.GetType();

            var convertedTargetProperties = targetType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(x => x.HasAttribute<JsonIgnoreAttribute>() == false);

            return ObjectUtils.ConvertProperties(target, convertedTargetProperties);
        }

    }
}
