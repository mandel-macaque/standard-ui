using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.StandardUI.WinUI
{
    public class UIElementCollection<TNativeUIElment, TStandardUIElement> : IList, IList<TNativeUIElment>
        where TNativeUIElment : UIElement where TStandardUIElement : IUIElement
    {
        private Microsoft.UI.Xaml.Controls.UIElementCollection _collection;

        public UIElementCollection(Microsoft.UI.Xaml.FrameworkElement parent)
        {
            // TODO: Figure out the best way to implement this
            _collection = null; //  new Microsoft.UI.Xaml.Controls.UIElementCollection();
        }

        public IUIElement this[int index]
        {
            get => (IUIElement)_collection[index];
            set => _collection[index] = (Microsoft.UI.Xaml.UIElement)value;
        }

        T IList<T>.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Count => _collection.Count;

        public bool IsReadOnly => false;

        public void Add(IUIElement item)
        {
            _collection.Add((Microsoft.UI.Xaml.UIElement) item);
        }

        public void Clear() => _collection.Clear();

        public bool Contains(IUIElement item) => _collection.Contains((Microsoft.UI.Xaml.UIElement)item);

        public void CopyTo(IUIElement[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IUIElement> GetEnumerator() => new Enumerator(_collection.GetEnumerator());

        public int IndexOf(IUIElement item) => _collection.IndexOf((Microsoft.UI.Xaml.UIElement)item);

        public void Insert(int index, IUIElement item) => _collection.Insert(index, (Microsoft.UI.Xaml.UIElement)item);

        public bool Remove(IUIElement item)
        {
            int index = _collection.IndexOf((Microsoft.UI.Xaml.UIElement)item);
            if (index == -1)
                return false;
            _collection.RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index) => _collection.RemoveAt(index);

        void ICollection<T>.Add(T item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        int IList<T>.IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        private class Enumerator : IEnumerator<IUIElement>
        {
            private IEnumerator _enumerator;

            public Enumerator(IEnumerator enumerator)
            {
                _enumerator = enumerator;
            }

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
            public IUIElement? Current => (IUIElement?)_enumerator.Current;
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).

            object? IEnumerator.Current => _enumerator.Current;

            public void Dispose() { }

            public bool MoveNext() => _enumerator.MoveNext();

            public void Reset() => _enumerator.Reset();
        }
    }
}
