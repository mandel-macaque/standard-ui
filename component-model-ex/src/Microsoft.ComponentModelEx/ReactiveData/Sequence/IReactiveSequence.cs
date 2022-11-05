namespace Microsoft.ComponentModelEx.ReactiveData.Sequence
{
    public interface IReactiveSequence<T> : ISequence<T>, IReactive<INonreactiveSequence<T>>
    {
    }
}
