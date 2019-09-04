using System.Collections.Generic;

namespace Autofac.Features.Decorators
{
    public static class DecoratorAdaptionTypeExtensions
    {
        private static readonly Dictionary<EDecoratorAdaptionType, DecoratorAdaptionType> DecoratorAdaptionTypes = new Dictionary<EDecoratorAdaptionType, DecoratorAdaptionType>
        {
            { EDecoratorAdaptionType.None, DecoratorAdaptionType.None },
            { EDecoratorAdaptionType.Func, DecoratorAdaptionType.Func },
            { EDecoratorAdaptionType.Lazy, DecoratorAdaptionType.Lazy }
        };

        internal static DecoratorAdaptionType Value(this EDecoratorAdaptionType type)
        {
            return DecoratorAdaptionTypes[type];
        }
    }
}