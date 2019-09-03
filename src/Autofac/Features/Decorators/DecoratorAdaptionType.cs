namespace Autofac.Features.Decorators
{
    public enum DecoratorAdaptionType
    {
        /// <summary>
        /// Decorator takes a direct dependency on the service type
        /// </summary>
        None,

        /// <summary>
        /// Decorator takes a dependency on a Func instance of the service type
        /// </summary>
        Func,

        /// <summary>
        /// Decorator takes a dependency on a Lazy instance of the service type
        /// </summary>
        Lazy
    }
}