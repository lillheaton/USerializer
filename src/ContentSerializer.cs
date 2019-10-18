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
        public JsonSerializerSettings JsonSerializerSettings { get; private set; }
        
        public ContentSerializer(IConverterLocatorService converterLocatorService)
        {
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

            var properties = 
                targetType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.HasAttribute<JsonIgnoreAttribute>() == false);

            return ObjectUtils.ConvertProperties(target, properties);
        }

    }
}
