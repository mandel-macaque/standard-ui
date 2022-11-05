namespace Microsoft.ComponentModelEx.ReactiveData.Sequence
{
    public interface IItemsSequence<T> : INonreactiveSequence<T>
    {
        SequenceImmutableArray<T> Items { get; }

        int ItemCount { get; }
    }
}
