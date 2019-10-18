using JsonContractSimplifier.Services.ConverterLocator;
using Umbraco.Core.Composing;
using USerializer.PropertyConverters;

namespace USerializer
{
    public class USerializerComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Register(typeof(IConverterLocatorService), typeof(ConverterLocatorService), Lifetime.Singleton);
            composition.Register(typeof(IContentSerializer), typeof(ContentSerializer), Lifetime.Singleton);
            
            composition.RegisterUniqueFor<IPublishedContentConverter, IConverter>();
            composition.RegisterUniqueFor<IPublishedPropertyConverter, IConverter>();
            composition.RegisterUniqueFor<IPublishedElementConverter, IConverter>();
        }
    }
}
