namespace Autofac.Features.Decorators
{
    internal class DecorationResult
    {
        private DecorationResult(bool decorated, bool containsDeferredExecution, object undecoratedInstance, object decoratedInstance)
        {
            Decorated = decorated;
            ContainsDeferredExecution = containsDeferredExecution;
            UndecoratedInstance = undecoratedInstance;
            DecoratedInstance = decoratedInstance;
        }

        public bool Decorated { get; }

        public bool ContainsDeferredExecution { get; }

        public object UndecoratedInstance { get; }

        public object DecoratedInstance { get; }

        internal static DecorationResult UndecoratedResult { get; } = new DecorationResult(false, false, null, null);

        internal static DecorationResult GetDecoratedResult(object undecorated, object decorated)
        {
            return new DecorationResult(true, false, undecorated, decorated);
        }

        internal static DecorationResult GetDeferredDecoratedResult(object decorated)
        {
            return new DecorationResult(true, true, null, decorated);
        }
    }
}