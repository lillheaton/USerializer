using Newtonsoft.Json;
using System.Collections.Generic;

namespace USerializer
{
    public interface IContentSerializer
    {
        JsonSerializerSettings JsonSerializerSettings { get; }
        string Serialize(object target);
        Dictionary<string, object> Convert(object target);
    }
}
