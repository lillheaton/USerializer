using JsonContractSimplifier;
using JsonContractSimplifier.Services.ConverterLocator;
using System;
using System.Linq;
using Umbraco.Core.Composing;
using USerializer.Utils;

namespace USerializer
{
    public class ConverterLocatorService : IConverterLocatorService
    {
        private readonly DeriveTypeCompare _deriveTypeComparer = new DeriveTypeCompare();
        
        private (IConverter Converter, Type Target)[] _converters;
        private (IConverter Converter, Type Target)[] Converters
        {
            get
            {
                if (_converters == null)
                {
                    _converters = LoadConverters();
                }

                return _converters;
            }
        }

        private static string LocalAssemblyName =>
            string.Join(".", typeof(ConverterLocatorService).Assembly.GetName().Name.Split('.').Take(2));

        private static bool IsLocalAssemblyConverter(Type type) => type.Assembly.FullName.Contains(LocalAssemblyName);

        private (IConverter Converter, Type Target)[] LoadConverters()
        {
            var allConverters = Current.Factory.GetAllInstances<IConverter>().ToArray();            

            var localAssemblyConverters = allConverters
                .Where(x => IsLocalAssemblyConverter(x.GetType()));

            var otherAssemblyConverters = allConverters
                .Where(x => IsLocalAssemblyConverter(x.GetType()) == false);

            return otherAssemblyConverters
                .Concat(
                    localAssemblyConverters
                    .Where(x => otherAssemblyConverters.Any(y => y.Equals(x)) == false)
                )
                .Select(converter =>
                    (converter, converter.GetType().GetInterfaces()[0].GetGenericArguments()[0])
                )
                .ToArray();
        }


        public object Convert(object target, IConverter converter)
        {
            Type converterType = converter.GetType();

            if (!converterType.ImplementsGenericInterface(typeof(IObjectConverter<>)))
            {
                throw new ArgumentException($"Converter type {converterType.Name} does not implement IObjectConverter");
            }

            var method = converterType.GetMethod("Convert");
            return method.Invoke(converter, new[] { target });
        }

        public bool TryFindConverterFor(Type type, out IConverter converter)
        {
            bool typeIsEqual((IConverter Converter, Type Target) tuple)
                => tuple.Target == type || type.IsSubclassOf(tuple.Target) || tuple.Target.IsAssignableFrom(type);

            var converters = Converters.Where(typeIsEqual).OrderBy(x => x.Target, _deriveTypeComparer);

            bool exist = converters.Any();

            if (!exist)
            {
                converter = null;
                return false;
            }

            converter = converters.First().Converter;
            return true;
        }
    }
}
