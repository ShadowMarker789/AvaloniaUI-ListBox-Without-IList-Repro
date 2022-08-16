using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1
{
    public partial class QuickCollectionWithIList : IReadOnlyList<QuickModel>, INotifyCollectionChanged, IList
    {
        private Nito.AsyncEx.AsyncReaderWriterLock _lock = new Nito.AsyncEx.AsyncReaderWriterLock();
        private InternalCollection _items = new();

        public QuickModel this[int index] => _items[index];

        public int Count => _items.Count;

        public bool IsFixedSize => false;

        public bool IsReadOnly => true;

        public bool IsSynchronized => true;

        public object SyncRoot => this;

        object? IList.this[int index] { get => this[index]; set => throw new NotImplementedException(); }

        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public IEnumerator<QuickModel> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public void AddItem(QuickModel item)
        {
            using var lockyLock = _lock.WriterLock();
            _items.Add(item);
            int index = _items.IndexOf(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                 NotifyCollectionChangedAction.Add,
                 item,
                 index
                ));
        }

        public bool RemoveItem(QuickModel item)
        {
            using var lockyLock = _lock.WriterLock();
            int index = _items.IndexOf(item);
            var retval = _items.Remove(item);
            if(retval)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, item, index));
            return retval;
        }

        public int Add(object? value)
        {
            if(value is QuickModel qm)
            {
                AddItem(qm);
                return _items.IndexOf(qm);
            }
            throw new ArgumentException($"{nameof(value)} is not {nameof(QuickModel)}");
        }

        public void Clear()
        {
            using var lockyLock = _lock.WriterLock();
            _items.Clear();
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(object? value)
        {
            if (value is QuickModel qm)
                return _items.Contains(qm);
            return false;
        }

        public int IndexOf(object? value)
        {
            if (value is QuickModel qm)
                return _items.IndexOf(qm);
            return -1;
        }

        public void Insert(int index, object? value)
        {
            using var lockyLock = _lock.WriterLock();
            if (value is QuickModel qm)
            {
                _items.Insert(index, qm);
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                     NotifyCollectionChangedAction.Add, qm, _items.IndexOf(qm)));
                return;
            }
            throw new ArgumentException($"{nameof(value)} is not {nameof(QuickModel)}");
        }

        public void Remove(object? value)
        {
            using var lockyLock = _lock.WriterLock();
            if (value is QuickModel qm)
            {
                var index = _items.IndexOf(qm);
                _items.RemoveAt(index);
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                     NotifyCollectionChangedAction.Remove,
                     qm, index));
                return;
            }
            throw new ArgumentException($"{nameof(value)} is not {nameof(QuickModel)}");
        }

        public void RemoveAt(int index)
        {
            using var lockyLock = _lock.WriterLock();
            var item = _items[index];
            _items.Remove(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(
                     NotifyCollectionChangedAction.Remove,
                     item, index));
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
